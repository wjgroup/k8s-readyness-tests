using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReadynessTests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveController : ControllerBase
    {
        private static DateTimeOffset _startDT = DateTimeOffset.UtcNow;
        private static readonly int _interval = 30;

        // GET api/live
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            if (DateTimeOffset.UtcNow > _startDT.AddSeconds(_interval))
            {
                await Task.Delay(10000);
                return "not live";
            }

            return $"live {DateTimeOffset.UtcNow - _startDT}";
        }
    }
}