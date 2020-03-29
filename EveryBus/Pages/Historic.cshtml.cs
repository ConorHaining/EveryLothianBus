using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EveryBus.Pages
{
    public class HistoricModel : PageModel
    {
        private readonly ILogger<HistoricModel> _logger;

        public HistoricModel(ILogger<HistoricModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
