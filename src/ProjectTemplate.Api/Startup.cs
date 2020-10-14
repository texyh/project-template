using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectTemplate.Infrastructure.AcquiringBank;
using ILogger = Serilog.ILogger;
using ProjectTemplate.Api.Middleware;
using ProjectTemplate.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MediatR;
using ProjectTemplate.Application.Payments.GetPayment;

namespace ProjectTemplate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AcquiringBankSettings>(options =>  options.ApiUrl = Configuration.GetValue("BANK_API_URL", string.Empty));

            services
                .AddSwaggerGen()
                .AddFluentValidation()
                .AddControllers();

            services.AddMediatR(typeof(Startup), typeof(GetPaymentQuery))
                    .AddCommandHandlerDecorators();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IServiceProvider serviceProvider)
        {
            app.UseHsts();
            
            var logger = serviceProvider.GetService<ILogger>();

            app.UseExceptionHandler(builder => builder.HandleExceptions(logger, environment));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
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
