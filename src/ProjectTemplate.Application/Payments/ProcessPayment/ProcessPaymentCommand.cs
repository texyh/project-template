using ProjectTemplate.Application.Abstractions.Commands;

namespace ProjectTemplate.Domain.Payments.Commands
{
    public class ProcessPaymentCommand : ICommand<ProcessPaymentResult>
    {
        public string CardNumber { get; set; }

        public string CardExpiryMonth { get; set; }

        public string CardExpiryYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CVV { get; set; }

        public string MerchantId { get; set; }
    }
}
