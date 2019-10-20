using System;
using Microsoft.AspNetCore.Mvc;

namespace ZipPay.Services.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class HealthController : Controller
    {
        private readonly Func<DateTime> _now;

        public HealthController(Func<DateTime> now)
        {
            _now = now;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public string Ping()
        {
            return $"Hello Zip @ {_now():F}";
        }
    }
}
