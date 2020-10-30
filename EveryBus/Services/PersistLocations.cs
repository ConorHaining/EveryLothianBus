using System;
using System.Collections.Generic;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EveryBus.Services
{
    public class PersistLocations : IObserver<List<VehicleLocation>>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IDisposable unsubscriber;
        private readonly Dictionary<string, VehicleLocation> _latest;

        public PersistLocations(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
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

                    if (update.ReportTime > existingRecord?.ReportTime)
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