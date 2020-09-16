using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyRateApp.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CurrencyRateApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration oConfiguration)
        {
            Configuration = oConfiguration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EFContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
