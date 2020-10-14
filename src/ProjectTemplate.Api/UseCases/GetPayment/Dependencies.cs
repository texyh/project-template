using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectTemplate.Application.Crypto;
using ProjectTemplate.Domain.Abstractions;
using ProjectTemplate.Domain.Payments;
using ProjectTemplate.Infrastructure;
using ProjectTemplate.Infrastructure.Repositories;

namespace ProjectTemplate.Api.UseCases.GetPayment
{
    public static class Dependencies
    {
        public static IServiceCollection AddGetPaymentUseCase(this IServiceCollection services) 
        {

            services.TryAddScoped<ICryptoService, CryptoService>();
            services.TryAddScoped<IPaymentRepository, PaymentRepository>();
            services.TryAddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));

            return services;
        }
    }
}