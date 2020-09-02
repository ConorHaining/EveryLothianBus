using System;
using System.Collections.Generic;
using BAMCIS.GeoJSON;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Hubs;
using EveryBus.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace EveryBus.Services
{
    public class BroadcastLocations : IObserver<List<VehicleLocation>>
    {
        private IHubContext<BusHub> _hubContext;
        private readonly BusLocationsProvider _busLocationsProvider;
        private readonly IRouteColourService _routeColourService;
        private IDisposable unsubscriber;
        private readonly Dictionary<String, VehicleLocation> _latest;

        public BroadcastLocations(IHubContext<BusHub> hubContext, BusLocationsProvider busLocationsProvider, IRouteColourService routeColourService)
        {
            _hubContext = hubContext;
            _busLocationsProvider = busLocationsProvider;
            _routeColourService = routeColourService;
            _latest = new Dictionary<string, VehicleLocation>();

            // unsubscriber = _busLocationsProvider.Subscribe(this);
        }

        public void OnCompleted()
        {
            //
        }

        public void OnError(Exception error)
        {
            //
        }

        public async void OnNext(List<VehicleLocation> vehicleUpdates)
        {
            if (vehicleUpdates == null){
                return;
            }
            foreach (var update in vehicleUpdates)
            {
                var vehicleId = update.VehicleId;
                VehicleLocation existingRecord;
                var recordExists = _latest.TryGetValue(vehicleId, out existingRecord);

                if (!recordExists)
                {
                    _latest.TryAdd(vehicleId, update);
                }

                if (update.LastGpsFix > existingRecord?.LastGpsFix && (update.ServiceName != null || update.JourneyId != null))
                {
                    var timestamp = CreateLocalTimestamp(update);
                    var properties = new Dictionary<string, object>();
                    properties.Add("heading", update.Heading);
                    properties.Add("colour", _routeColourService.Get(update.ServiceName)?.Colour);
                    properties.Add("text_colour", _routeColourService.Get(update.ServiceName)?.TextColor);
                    properties.Add("name", update.ServiceName);
                    properties.Add("vehicleId", update.VehicleId);
                    properties.Add("last_update", timestamp);
                    properties.Add("destination", update.Destination);

                    var position = new Position(update.Longitude, update.Latitude);
                    var point = new Point(position);
                    var feature = new Feature(point, properties);
                    var collection = new FeatureCollection( new List<Feature> { feature });

                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", collection.ToJson());

                    _latest[vehicleId] = update;
                }

            }
        }

        public void Unsubscribe()
        {
            unsubscriber.Dispose();
        }

        private DateTime CreateLocalTimestamp(VehicleLocation location)
        {
            var datetime = DateTime.UnixEpoch.AddSeconds(location.LastGpsFix);
            return datetime;
        }
    }
}
