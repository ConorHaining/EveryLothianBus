using System.Collections.Generic;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EveryBus.Controller
{
    [Controller]
    [Route("api/locations")]
    public class LiveLocationsController : ControllerBase
    {
        private readonly BusContext _busContext;

        public LiveLocationsController(BusContext busContext)
        {
            _busContext = busContext;
        }

        [HttpGet]
        public ActionResult<List<VehicleLocation>> GetAllLocations()
        {
            var lastestReports = _busContext.VehicleLocations
                                    .GroupBy(x => x.VehicleId)
                                    .Select(x => new { VehicleId = x.Key, LastestReport = x.Max(x => x.LastGpsFix) })
                                    .AsEnumerable();

            var result = from locations in _busContext.VehicleLocations
                         join latest in lastestReports on 
                            new {x1 = locations.VehicleId, x2 = locations.LastGpsFix} equals new { x1 = latest.VehicleId, x2 = latest.LastestReport}
                        select locations;

            return result.ToList();
        }

        [HttpGet]
        [Route("{VehicleId}")]
        public ActionResult<VehicleLocation> GetSpecificLocations(string VehicleId)
        {
            var lastestReports = _busContext.VehicleLocations
                                    .Where(x => x.VehicleId == VehicleId)
                                    .OrderByDescending(x => x.LastGpsFix)
                                    .First();

            return lastestReports;
        }
    }
}