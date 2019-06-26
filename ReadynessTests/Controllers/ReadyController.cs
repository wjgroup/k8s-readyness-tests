﻿using System;
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
            _logger.LogInformation($"{new string('-', 10)} receiving request /api/ready");

            if (DateTimeOffset.UtcNow - _dt > _interval)
            {
                _isReady = !_isReady;
                _dt = DateTimeOffset.UtcNow;
            }

            var request = HttpContext.Request;
            var url = $"{request.Scheme}//{request.Host}{request.Path}{request.QueryString}";
            var headers = string.Join("][", request.Headers.Select(x => $"{x.Key}:{x.Value}").ToArray());

            var msg = $"{new string('-', 10)} I am {(_isReady ? "ready" : "not ready")} - {DateTimeOffset.UtcNow - _dt} - {url} - headers: {headers}";

            _logger.LogWarning(msg);

            if (_isReady)
            {
                return msg;
            }

            await Task.Delay(30);

            return msg;
        }
    }
}