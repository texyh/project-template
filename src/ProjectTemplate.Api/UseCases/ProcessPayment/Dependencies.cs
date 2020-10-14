using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectTemplate.Application.Crypto;
using ProjectTemplate.Domain.Abstractions;
using ProjectTemplate.Domain.AcquiringBank;
using ProjectTemplate.Domain.Payments;
using ProjectTemplate.Domain.Payments.Commands;
using ProjectTemplate.Infrastructure;
using ProjectTemplate.Infrastructure.AcquiringBank;
using ProjectTemplate.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTemplate.Api.UseCases.ProcessPayment
{
    public static class Dependencies
    {
        public static IServiceCollection AddProcessPaymentUseCase(this IServiceCollection services)
        {
            services.TryAddScoped<ICryptoService, CryptoService>();
            services.TryAddScoped<IPaymentRepository, PaymentRepository>();
            services.TryAddScoped(typeof(IAggregateStore<>), typeof(AggreateStore<>));



            if (Environment.IsDevelopment())
            {
                services.AddHostedService<AcquiringBankMockService>();
            }

            services.AddHttpClient<IAquiringBankClient, AcquiringBankClient>()
                    .AddPolicyHandler(RetryPolicy.GetRetryPolicy(2));

            return services;
        }
    }
}
