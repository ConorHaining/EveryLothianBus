using System;
using System.Collections.Generic;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EveryBus.Services
{
    public class PersistLocations : IObserver<List<VehicleLocation>>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly BusLocationsProvider _busLocationsProvider;
        private IDisposable unsubscriber;
        private readonly Dictionary<String, VehicleLocation> _latest;

        public PersistLocations(IServiceScopeFactory scopeFactory, BusLocationsProvider busLocationsProvider)
        {
            _scopeFactory = scopeFactory;
            _busLocationsProvider = busLocationsProvider;
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

        public void OnNext(List<VehicleLocation> vehicleUpdates)
        {
            if (vehicleUpdates == null){
                return;
            }
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();
                foreach (var update in vehicleUpdates)
                {
                    var vehicleId = update.VehicleId;
                    VehicleLocation existingRecord;
                    var recordExists = _latest.TryGetValue(vehicleId, out existingRecord);

                    if (!recordExists)
                    {
                        busContext.VehicleLocations.Add(update);
                        _latest.TryAdd(vehicleId, update);
                    }

                    if (update.LastGpsFix > existingRecord?.LastGpsFix)
                    {
                        busContext.VehicleLocations.Add(update);
                        _latest[vehicleId] = update;
                    }

                }

                busContext.SaveChanges();
            }
        }

        public void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
    }
}