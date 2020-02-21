using System;
using System.Collections.Generic;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;

namespace EveryBus.Services
{
    public class PersistLocations : IObserver<VehicleLocation>
    {
        private readonly BusContext _busContext;
        private readonly IPollingService _pollingService;
        private IDisposable unsubscriber;
        private readonly Dictionary<String, VehicleLocation> _latest;
        
        public PersistLocations(BusContext busContext, IPollingService pollingService)
        {
            _busContext = busContext;
            _pollingService = pollingService;
            _latest = new Dictionary<string, VehicleLocation>();

            unsubscriber = _pollingService.Subscribe(this);
        }

        public void OnCompleted()
        {
            //
        }

        public void OnError(Exception error)
        {
            //
        }

        public void OnNext(VehicleLocation vehicle)
        {
            var hasRecord = _latest.ContainsValue(vehicle);

            if (hasRecord)
            {
                _busContext.VehicleLocations.Add(vehicle);
                _busContext.SaveChanges();

                var id = vehicle.Id;
                _latest.Add(id, vehicle);
            }
        }

        public void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
    }
}