using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyRateApp.Controllers
{
    [ApiController]
    public abstract class BaseController<T> : ControllerBase where T: class
    {
        protected ILogger<T> Logger { get; private set; }

        public BaseController(ILogger<T> logger)
        {
            Logger = logger;
        }
    }
}
