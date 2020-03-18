using System.Collections.Generic;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BAMCIS.GeoJSON;

namespace EveryBus.Controller
{
    [Controller]
    [Route("api/locations")]
    public class LiveLocationsController : ControllerBase
    {
        private readonly IVehicleLocationsService _vehicleLocationsService;
        private readonly IRouteColourService _routeColourService;

        public LiveLocationsController(IVehicleLocationsService vehicleLocationsService, IRouteColourService routeColourService)
        {
            _vehicleLocationsService = vehicleLocationsService;
            _routeColourService = routeColourService;
        }

        [HttpGet]
        public IActionResult GetAllLocations([FromQuery]bool activeOnly = true)
        {
            var locations =  _vehicleLocationsService.GetAllLatestLocations(activeOnly);

            var features = new List<Feature>();

            foreach (var location in locations)
            {
                var properties = new Dictionary<string, object>();
                properties.Add("heading", location.Heading);
                properties.Add("colour", _routeColourService.Get(location.ServiceName)?.Colour);
                properties.Add("name", location.ServiceName);
                properties.Add("vehicleId", location.VehicleId);

                var position = new Position(location.Longitude, location.Latitude);
                var point = new Point(position);
                var feature = new Feature(point, properties);

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