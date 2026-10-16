using System;
using Microsoft.AspNetCore.Mvc;

namespace netcore.Controllers
{
    /// <summary>
    /// 公开时间接口：GET /api/time 返回当前 UTC/本地时间戳。
    /// </summary>
    [ApiController]
    [Route("api/time")]
    public class TimeController : ControllerBase
    {
        /// <summary>
        /// 以 JSON 返回服务器时间（无需鉴权）。
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var localTz = TimeZoneInfo.Local;
            var localNow = TimeZoneInfo.ConvertTime(utcNow, localTz);

            return Ok(new
            {
                // UTC 时刻，ISO-8601 往返格式
                utc = utcNow.ToString("o"),
                // 同一时刻转换到主机本地时区
                local = localNow.ToString("o"),
                // 主机本地时区的 IANA/Windows 标识
                timeZone = localTz.Id,
                // Unix 纪元毫秒（UTC）
                unixMs = utcNow.ToUnixTimeMilliseconds()
            });
        }
    }
}
