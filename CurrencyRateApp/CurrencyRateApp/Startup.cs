using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyRateApp.Context;
using CurrencyRateApp.Exceptions;
using CurrencyRateApp.Filters;
using CurrencyRateApp.Repositories;
using CurrencyRateApp.Services;
using CurrencyRateApp.Services.Interfaces;
using FluentValidation.AspNetCore;
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

            services.AddMvc().AddFluentValidation(setup => setup.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddSwaggerGen(config =>
            {
                config.OperationFilter<ApiKeySwaggerAttribute>();
            });

            services.AddControllersWithViews(options =>
                options.Filters.Add(new ApiExceptionFilterAttribute()));

            services.AddScoped<IHashService, HashService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IDatabaseRepository, DatabaseRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Currency Rate API");
            });

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
