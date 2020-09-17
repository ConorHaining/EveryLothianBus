using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EveryBus.Controller
{
    [Route("api/departures")]
    [ApiController]
    public class DeparturesController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DeparturesController> _logger;
        private readonly IRouteColourService _routeColourService;
        private readonly IStopsService _stopsService;

        public DeparturesController(
            ILogger<DeparturesController> logger,
            IHttpClientFactory _httpClientFactory,
            IRouteColourService routeColourService,
            IStopsService stopsService
            )
        {

            _httpClient = _httpClientFactory.CreateClient("polling");
            _logger = logger;
            _routeColourService = routeColourService;
            _stopsService = stopsService;
        }

        [HttpGet]
        [Route("{atcoCode}")]
        public async Task<DepartureInformation[]> GetLiveDeparturesAsync(string atcoCode)
        {
            var stop = _stopsService.GetByAtocCode(atcoCode);
            var stopId = stop.StopId;

            HttpResponseMessage result;
            try
            {
                result = await _httpClient.GetAsync($"http://tfe-opendata.com/api/v1/live_bus_times/{stopId}");
                result.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                _logger.LogWarning("Request has been unsucessful.");
                return default;
            }

            var responseString = await result.Content.ReadAsStringAsync();
            var departureInformation = JsonSerializer.Deserialize<DepartureInformation[]>(responseString);

            foreach (var departure in departureInformation)
            {
                var colours = _routeColourService.Get(departure.RouteName);
                departure.RouteColour = colours.Colour;
                departure.TextColour = colours.TextColor;
            }

            return departureInformation;
        }
    }
}
