using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTemplate.Api.UseCases.ProcessPayment
{
    public class PaymentResponse : IEquatable<PaymentResponse>
    {
     
        public string PaymentId { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as PaymentResponse);
        }

        public bool Equals(PaymentResponse other)
        {
            return other != null &&
                   PaymentId == other.PaymentId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PaymentId);
        }
    }
}
