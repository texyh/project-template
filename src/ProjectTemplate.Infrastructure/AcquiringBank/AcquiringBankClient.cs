using Microsoft.Extensions.Options;
using ProjectTemplate.Domain.AcquiringBank;
using ProjectTemplate.Domain.Helpers;
using ProjectTemplate.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace ProjectTemplate.Infrastructure.AcquiringBank
{
    public class AcquiringBankClient : IAquiringBankClient
    {
        private readonly HttpClient _httpClient;

        private readonly AcquiringBankSettings _acquiringBankSettings;

        private readonly ILogger _logger;

        public AcquiringBankClient(
            HttpClient httpClient,
            IOptionsMonitor<AcquiringBankSettings> bankSettings,
            ILogger logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(bankSettings.CurrentValue.ApiUrl);
            _acquiringBankSettings = bankSettings.CurrentValue;
            _logger = logger;
        }

        public async Task<BankPaymentResponse> ProcessPayment(BankPaymentRequest requestModel)
        {
            using var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_acquiringBankSettings.ApiUrl}/api/payment"),
                Method = HttpMethod.Post,
                Content = requestModel.ToStringContent()
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.Error($"acquring bank payment failed. error: {response.ReasonPhrase}");
                return new BankPaymentResponse
                {
                    PaymentStatus = PaymentStatus.Failed
                };
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent.FromJson<BankPaymentResponse>();
        }
    }
}
