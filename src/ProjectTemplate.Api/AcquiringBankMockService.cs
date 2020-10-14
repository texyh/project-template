using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProjectTemplate.Domain.AcquiringBank;
using ProjectTemplate.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WireMock.Logging;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;
using ProjectTemplate.Domain.Helpers;
using ProjectTemplate.Infrastructure.AcquiringBank;

namespace ProjectTemplate.Api
{
    public class AcquiringBankMockService : BackgroundService
    {
        private readonly AcquiringBankSettings _acquiringBankSettings;


        public AcquiringBankMockService(IOptionsMonitor<AcquiringBankSettings> bankSettings)
        {
            
            _acquiringBankSettings = bankSettings.CurrentValue;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var server = WireMockServer.Start(new WireMockServerSettings
            {
                Urls = new[] { _acquiringBankSettings.ApiUrl },
                StartAdminInterface = true,
                ReadStaticMappings = true,
                Logger = new WireMockConsoleLogger()
            });

            server
                .Given(Request.Create()
                    .WithPath($"/api/payment").UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody((new BankPaymentResponse { PaymentIdentifier = Guid.NewGuid().ToString(), PaymentStatus = PaymentStatus.Success }).ToJson()));

            return Task.CompletedTask;
        }
    }
}
