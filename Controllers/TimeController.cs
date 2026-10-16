using System;
using Microsoft.AspNetCore.Mvc;

namespace netcore.Controllers
{
    /// <summary>
    /// Public time API: GET /api/time returns current UTC/local timestamps.
    /// </summary>
    [ApiController]
    [Route("api/time")]
    public class TimeController : ControllerBase
    {
        /// <summary>
        /// Returns server time as JSON (no auth required).
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var localTz = TimeZoneInfo.Local;
            var localNow = TimeZoneInfo.ConvertTime(utcNow, localTz);

            return Ok(new
            {
                // UTC instant in ISO-8601 round-trip format
                utc = utcNow.ToString("o"),
                // Same instant converted to the host's local time zone
                local = localNow.ToString("o"),
                // IANA/Windows id of the host local time zone
                timeZone = localTz.Id,
                // Unix epoch milliseconds (UTC)
                unixMs = utcNow.ToUnixTimeMilliseconds()
            });
        }
    }
}
