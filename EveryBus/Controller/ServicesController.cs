using System.Collections.Generic;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BAMCIS.GeoJSON;
using System;

namespace EveryBus.Controller
{
    [Controller]
    [Route("api/routes")]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        public List<KeyValuePair<string, string>> GetAllRoutes()
        {
            var routes = _routeService.GetRoutes();
            var results = new List<KeyValuePair<string, string>>();

            foreach (var route in routes)
            {
                var item = new KeyValuePair<string, string>(route.Name, $"{route.Name} | {route.Description}");
                results.Add(item);
            }

            return results;
        }
    }
}