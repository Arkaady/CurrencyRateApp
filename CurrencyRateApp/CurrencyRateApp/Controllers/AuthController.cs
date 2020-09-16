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
        public IAuthService AuthService { get; set; }
        public AuthController(ILogger<AuthController> logger, IAuthService authService) : base(logger)
        {
            AuthService = authService;
        }

        [HttpPut]
        public async Task<ActionResult<string>> GenerateApiKeyAsync()
        {
            Logger.LogInformation("Started generation new api Key");
            var apiKey = await AuthService.GenerateApiKeyAsync();
            Logger.LogInformation("Generated new api Key");

            return Ok(apiKey);
        }
    }
}
