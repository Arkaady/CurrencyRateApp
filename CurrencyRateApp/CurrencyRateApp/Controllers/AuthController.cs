using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyRateApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController<AuthController>
    {
        public AuthController(ILogger<AuthController> logger) : base(logger)
        {
        }

        [HttpPut]
        public async Task<ActionResult<string>> GenerateApiKeyAsync()
        {
            throw new NotImplementedException();
        }
    }
}
