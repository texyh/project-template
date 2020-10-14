using System;
using System.Collections.Generic;

namespace ProjectTemplate.Application.Payments.GetPayment
{
    public abstract class GetPaymentResult
    {

    }

    public class SuccessResult : GetPaymentResult
    {
        public string CardNumber { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Currency { get; set; }

        public string PaymentStatus { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SuccessResult result &&
                   CardNumber == result.CardNumber &&
                   Amount == result.Amount &&
                   PaymentDate == result.PaymentDate &&
                   Currency == result.Currency &&
                   PaymentStatus == result.PaymentStatus;
        }

        public override int GetHashCode()
        {
            int hashCode = 137802613;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CardNumber);
            hashCode = hashCode * -1521134295 + Amount.GetHashCode();
            hashCode = hashCode * -1521134295 + PaymentDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Currency);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PaymentStatus);
            return hashCode;
        }
    }

    public class ErrorResult : GetPaymentResult
    {
        public string Message { get; }

        public ErrorResult(string message)
        {
            Message = message;
        }
    }
}