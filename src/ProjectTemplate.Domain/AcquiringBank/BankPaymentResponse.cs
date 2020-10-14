using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTemplate.Domain.AcquiringBank
{
    public class BankPaymentResponse : IEquatable<BankPaymentResponse>
    {
        public string PaymentIdentifier { get; set; }

        public string PaymentStatus { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BankPaymentResponse);
        }

        public bool Equals(BankPaymentResponse other)
        {
            return other != null &&
                   PaymentIdentifier == other.PaymentIdentifier &&
                   PaymentStatus == other.PaymentStatus;
        }

        public override int GetHashCode()
        {
            var hashCode = -1522317123;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PaymentIdentifier);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PaymentStatus);
            return hashCode;
        }
    }
}
