using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ReadynessTests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadyController : ControllerBase
    {
        private static DateTimeOffset _startDT = DateTimeOffset.UtcNow;
        private static TimeSpan _interval = new TimeSpan(0, 0, 45);
        private readonly ILogger<ReadyController> _logger;

        public ReadyController(ILogger<ReadyController> logger)
        {
            _logger = logger;
        }

        // GET api/ready
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var totalSec = (int)(DateTimeOffset.UtcNow - _startDT).TotalSeconds;
            var ready = (totalSec / (int)_interval.TotalSeconds) % 2 == 0;
            var guid = Guid.NewGuid();

            _logger.LogWarning($"[start] receiving request /api/ready at {DateTimeOffset.UtcNow} - {totalSec}sec");

            var request = HttpContext.Request;
            var url = $"{request.Scheme}//{request.Host}{request.Path}{request.QueryString}";
            var headers = string.Join("][", request.Headers.Select(x => $"{x.Key}:{x.Value}").ToArray());

            if (ready)
            {
                var msg1 = $"[ready] at {DateTimeOffset.UtcNow} - {totalSec}sec - {url} - headers: {headers}";
                _logger.LogWarning(msg1);
                return msg1;
            }

            await Task.Delay(20000);

            var msg2 = $"[NOT ready] at {DateTimeOffset.UtcNow} - {totalSec}sec - {url} - headers: {headers}";
            _logger.LogWarning(msg2);
            return msg2;
        }
    }
}