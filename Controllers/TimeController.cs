using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace netcore.Controllers
{
    /// <summary>
    /// 公开时间接口：GET /api/time 返回当前 UTC/本地时间戳。
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("api/time")]
    public class TimeController : ControllerBase
    {
        private readonly ILogger<TimeController> _logger;

        public TimeController(ILogger<TimeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 以 JSON 返回服务器时间（无需鉴权）；每次请求写入一条文件日志作示范。
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var localTz = TimeZoneInfo.Local;
            var localNow = TimeZoneInfo.ConvertTime(utcNow, localTz);

            // 示范：每次访问写入文件日志，证明 FileLoggerProvider 可用
            _logger.LogInformation(
                "GET /api/time 已处理：utc={Utc}, local={Local}, timeZone={TimeZone}, unixMs={UnixMs}",
                utcNow.ToString("o"),
                localNow.ToString("o"),
                localTz.Id,
                utcNow.ToUnixTimeMilliseconds());

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
