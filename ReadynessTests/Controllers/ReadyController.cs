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
        private static DateTimeOffset _dt = DateTimeOffset.UtcNow;
        private static bool _isReady = true;
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
            var guid = Guid.NewGuid();
            _logger.LogWarning($"{new string('-', 4)} {guid} - receiving request /api/ready at {DateTimeOffset.UtcNow}");

            if (DateTimeOffset.UtcNow - _dt > _interval)
            {
                _isReady = !_isReady;
                _dt = DateTimeOffset.UtcNow;
            }

            var request = HttpContext.Request;
            var url = $"{request.Scheme}//{request.Host}{request.Path}{request.QueryString}";
            var headers = string.Join("][", request.Headers.Select(x => $"{x.Key}:{x.Value}").ToArray());

            if (_isReady)
            {
                var msg1 = $"{new string('-', 4)} {guid} - I am {(_isReady ? "ready" : "not ready")} at {DateTimeOffset.UtcNow} - {DateTimeOffset.UtcNow - _dt} - {url} - headers: {headers}";

                _logger.LogWarning(msg1);
                return msg1;
            }

            await Task.Delay(30000);

            var msg2 = $"{new string('-', 4)} {guid} - I am {(_isReady ? "ready" : "not ready")} at {DateTimeOffset.UtcNow} - {DateTimeOffset.UtcNow - _dt} - {url} - headers: {headers}";
            _logger.LogWarning(msg2);
            return msg2;
        }
    }
}