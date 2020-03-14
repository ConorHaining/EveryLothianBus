using System.Collections.Generic;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BAMCIS.GeoJSON;

namespace EveryBus.Controller
{
    [Controller]
    [Route("api/locations")]
    public class LiveLocationsController : ControllerBase
    {
        private readonly BusContext _busContext;
        private readonly IVehicleLocationsService _vehicleLocationsService;

        public LiveLocationsController(BusContext busContext, IVehicleLocationsService vehicleLocationsService)
        {
            _busContext = busContext;
            _vehicleLocationsService = vehicleLocationsService;
        }

        [HttpGet]
        public IActionResult GetAllLocations()
        {
            var locations =  _vehicleLocationsService.GetAllLatestLocations();

            var features = new List<Feature>();

            foreach (var location in locations)
            {
                var position = new Position(location.Longitude, location.Latitude);
                var point = new Point(position);
                var feature = new Feature(point);

                features.Add(feature);
            }

            var collection = new FeatureCollection(features);

            return Content(collection.ToJson(), "application/json");
        }

        [HttpGet]
        [Route("{VehicleId}")]
        public ActionResult<VehicleLocation> GetSpecificLocations(string VehicleId)
        {
            return _vehicleLocationsService.GetSpecificLatestLocation(VehicleId);
        }
    }
}