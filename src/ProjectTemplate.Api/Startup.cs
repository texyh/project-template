using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ILogger = Serilog.ILogger;
using ProjectTemplate.Api.Middleware;
using MediatR;
using ProjectTemplate.Application.Abstractions.Commands;

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

            services
                .AddSwaggerGen()
                .AddFluentValidation()
                .AddControllers();

            services.AddMediatR(typeof(Startup), typeof(ICommand))
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
