using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
//using Microsoft.ApplicationInsights;
//using Microsoft.ApplicationInsights.DataContracts
namespace netcore.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        //private readonly TelemetryClient telemetry;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
            //this.telemetry = telemetry;
        }

        public void OnGet()
        {
            //throw new Exception("Testing application insights");
            //this.telemetry.TrackEvent("PrivacyPageRequested");
            //this.telemetry.TrackTrace("Critical Trace", SeverityLevel.Critical);

        }
    }
}
