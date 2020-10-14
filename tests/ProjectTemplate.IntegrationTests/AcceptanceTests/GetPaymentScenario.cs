using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using ProjectTemplate.Api;
using ProjectTemplate.Api.UseCases.GetPayment;
using ProjectTemplate.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xbehave;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using Serilog;
using Xunit;
using ProjectTemplate.Application.Payments.GetPayment;
using System.Threading;

namespace ProjectTemplate.IntegrationTests.AcceptanceTests
{
    [Collection("Acceptance-Tests")]
    public class GetPaymentScenario : IClassFixture<TestServerFixture>
    {
        private TestServerFixture _fixture;

        public GetPaymentScenario(TestServerFixture testServerFixture)
        {
            testServerFixture.CreateTestEnvironment(services => 
            {
                services.AddGetPaymentUseCase();
            });

            _fixture = testServerFixture;
        }

        [Scenario]
        public void Get_Payment(SuccessResult payment, Payment existingPayment)
        {
            "Given an existing payment"
                .x(async () => existingPayment = await CreatePayment());

            "When it is retrieved"
                .x(async () =>
                {
                    payment = await GetPayment(existingPayment.Id.ToString());
                });

            "Then it should have payment details"
                .x(() =>  {
                    payment.Currency.Should().Be(existingPayment.Currency);
                    payment.PaymentStatus.Should().Be(existingPayment.PaymentStatus);
                    payment.Amount.Should().Be(existingPayment.Amount);
                    payment.PaymentDate.Date.Should().Be(existingPayment.CreatedDate.Date);
                  }); 
        }

        private async Task<SuccessResult> GetPayment(string paymentId)
        {
            var cancellation = new CancellationTokenSource();
            cancellation.CancelAfter(TimeSpan.FromSeconds(30));
            return await _fixture.GetAsync<SuccessResult>($"api/payments/{paymentId}", cancellation.Token);
        }

        private async Task<Payment> CreatePayment()
        {
            var payment = new Payment("7bedd30c790f45c6410b7389a58d2cbe.a159e99d6f97d3fbc60fe88161d54edac66a5c323521b7b2281465824bdcbae0",
                                      "c3bae8e66e8cf282b77ce798874e5b22.9cc552bdb6f06ebd08c7a2705fa40d28",
                                      "1dbf11b97e97ea71d51bc8cf0ffee21f.2088cf0b4b3522a5230beebeaea0132a", 100, "EUR",
                                      "e6cbdaf95f6f694ba2b83d24b7b9403a.b07288ff12c714ccfd5ca281e84da132",
                                      "20ba9d3d123141c8b0ae4df0a3383f7e",
                                      "d4920d4e-c6e0-4b6e-a259-cab69db9f1c5",
                                      PaymentStatus.Success);
            var paymentRepository = _fixture.GetService<IPaymentRepository>();
            await paymentRepository.AppendChanges(payment);

            return payment;
        }

        
    }
}
