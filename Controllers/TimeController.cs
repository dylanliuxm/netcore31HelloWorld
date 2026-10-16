using System;
using Microsoft.AspNetCore.Mvc;

namespace netcore.Controllers
{
    [ApiController]
    [Route("api/time")]
    public class TimeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var localTz = TimeZoneInfo.Local;
            var localNow = TimeZoneInfo.ConvertTime(utcNow, localTz);

            return Ok(new
            {
                utc = utcNow.ToString("o"),
                local = localNow.ToString("o"),
                timeZone = localTz.Id,
                unixMs = utcNow.ToUnixTimeMilliseconds()
            });
        }
    }
}
