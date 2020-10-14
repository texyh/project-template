using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTemplate.Api.UseCases.ProcessPayment
{
    public class PaymentRequest
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
