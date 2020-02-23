using EveryBus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EveryBus.Controller
{
    [Controller]
    [Route("api/polling")]
    public class PollingController : ControllerBase
    {
        private readonly IPollingService pollingService;

        public PollingController(IPollingService _pollingService)
        {
            pollingService = _pollingService;
        }

        [HttpGet]
        [Route("start")]
        public ActionResult<PollingStatus> Start()
        {
            var status = pollingService.Start();
            return Ok(status);
        }

        [HttpGet]
        [Route("status")]
        public ActionResult<PollingStatus> Status()
        {
            var status = pollingService.Status();
            return Ok(status);
        }

        [HttpGet]
        [Route("stop")]
        public ActionResult<PollingStatus> Stop()
        {
            var status = pollingService.Stop();
            return Ok(status);
        }
    }
}