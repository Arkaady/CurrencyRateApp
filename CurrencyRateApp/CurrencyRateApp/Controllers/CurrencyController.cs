using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyRateApp.Dto;
using CurrencyRateApp.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyRateApp.Controllers
{
    [Route("api/[controller]")]
    [ApiKeyAuthorization]
    [ApiController]
    public class CurrencyController : BaseController<AuthController>
    {
        public CurrencyController(ILogger<AuthController> logger) : base(logger)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<CurrencyRateResponseDto>>> GetCurrencyRateAsync([FromQuery] CurrencyRateFilter currencyRateFilter)
        {
            return Ok();
        }
    }
}
