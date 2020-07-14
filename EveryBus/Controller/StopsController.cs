using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EveryBus.Controller
{
    [Route("api/stops")]
    [ApiController]
    public class StopsController : ControllerBase
    {
        private readonly IStopsService _stopService;
        private readonly IVehicleLocationsService _vehicleLocationsService;

        public StopsController(IStopsService stopService, IVehicleLocationsService vehicleLocationsService)
        {
            _stopService = stopService;
            _vehicleLocationsService = vehicleLocationsService;
        }

        [HttpGet("id/{StopId}")]
        public Stop GetByStopId(int StopId)
        {
            return _stopService.GetByStopId(StopId);
        }

        [HttpGet("atoccode/{AtocCode}")]
        public Stop GetByAtocCode(string AtocCode)
        {
            return _stopService.GetByAtocCode(AtocCode);
        }
    }
}
