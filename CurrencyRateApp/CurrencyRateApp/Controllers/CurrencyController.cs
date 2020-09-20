using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyRateApp.Dto;
using CurrencyRateApp.Filters;
using CurrencyRateApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyRateApp.Controllers
{
    [Route("api/[controller]")]
    [ApiKeyAuthorization]
    [ApiController]
    public class CurrencyController : BaseController<AuthController>
    {
        private readonly IExchangeRateService _exchangeService;

        public CurrencyController(ILogger<AuthController> logger, IExchangeRateService exchangeService) 
            : base(logger)
        {
            _exchangeService = exchangeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CurrencyRatesDto>>> GetCurrencyRateAsync([FromQuery] CurrencyRateFilter currencyRateFilter)
        {
            Logger.LogInformation("Started fetching data for exchange rates");
            var result = await _exchangeService.GetCurrencyRatesAsync(currencyRateFilter);
            Logger.LogInformation("Fetched data for exchange rates");
            return Ok(result);
        }
    }
}
