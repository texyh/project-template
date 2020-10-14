using FluentValidation;
using Marten;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Npgsql;
using ProjectTemplate.Application.Abstractions.Commands;
using ProjectTemplate.Application.Decorators;

namespace ProjectTemplate.Api
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Payment Gateway Api",
                    Version = "v1",
                    Description = "This is an api that allow merchants process and manage payments"
                });
            });

            return services;
        }

        public static IServiceCollection AddPostgresHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddNpgSql(GetConnectionString(configuration));

            return services;
        }

        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            AssemblyScanner
                .FindValidatorsInAssembly(typeof(ICommand).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

            return services;
        }

        public static IServiceCollection AddCommandHandlerDecorators(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingCommandHandlerDecorator<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationCommandHandlerDecorator<,>));

            return services;
        }

        public static IServiceCollection AddMartenDB(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = GetConnectionString(configuration);
            var options = new StoreOptions();
            options.Connection(connectionString);
            //options.Events.InlineProjections.AggregateStreamsWith<>();
            services.AddMarten(options);

            return services;
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = configuration.GetValue<string>("POSTGRES_HOST"),
                Port = int.Parse(configuration.GetValue<string>("POSTGRES_PORT", "5432")),
                SslMode = SslMode.Prefer,
                Username = configuration.GetValue<string>("POSTGRES_USERNAME"),
                Password = configuration.GetValue<string>("POSTGRES_PASSWORD"),
                Database = configuration.GetValue<string>("POSTGRES_DB_NAME"),
                TrustServerCertificate = true
            };

            return  connectionStringBuilder.ConnectionString;
        }
    }
}
