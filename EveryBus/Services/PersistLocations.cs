using EveryBus.Domain;
using EveryBus.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace EveryBus.Services
{
    public class PersistLocations : IObserver<List<VehicleLocation>>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly BusContext busContext;
        private readonly Dictionary<string, VehicleLocation> _latest;

        public PersistLocations(
            IServiceScopeFactory scopeFactory,
            BusContext busContext)
        {
            _scopeFactory = scopeFactory;
            this.busContext = busContext;
            _latest = new Dictionary<string, VehicleLocation>();
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
            if (vehicleUpdates == null)
            {
                return;
            }

            //using var busContext = new BusContext();
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

                if (update.ReportTime > existingRecord?.ReportTime)
                {
                    busContext.VehicleLocations.Add(update);
                    _latest[vehicleId] = update;
                }

            }

            busContext.SaveChanges();

        }
    }
}