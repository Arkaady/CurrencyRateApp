using System.Threading.Tasks;
using CurrencyRateApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyRateApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController<AuthController>
    {
        private readonly IAuthService _authService;
        public AuthController(ILogger<AuthController> logger, IAuthService authService) : base(logger)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GenerateApiKeyAsync()
        {
            Logger.LogInformation("Started generation new api Key");
            var apiKey = await _authService.GenerateApiKeyAsync();
            Logger.LogInformation("Generated new api Key");

            return Ok(apiKey);
        }
    }
}
