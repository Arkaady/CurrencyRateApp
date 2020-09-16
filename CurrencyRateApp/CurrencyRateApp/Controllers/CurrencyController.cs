using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyRateApp.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyRateApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : BaseController<AuthController>
    {
        public CurrencyController(ILogger<AuthController> logger) : base(logger)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<CurrencyRateResponseDto>>> GetCurrencyRateAsync(
            [FromBody] CurrencyRateRequestDto currencyRateRequestDto, [FromHeader] string apiKey)
        {
            throw new NotImplementedException();
        }
    }
}
