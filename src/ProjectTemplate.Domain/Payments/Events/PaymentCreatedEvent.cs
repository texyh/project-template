using ProjectTemplate.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTemplate.Domain.Payments.Events
{
    public class PaymentCreatedEvent : IDomainEvent
    {

        public PaymentCreatedEvent(string cardNumber,
                       string cardExpiryMonth,
                       string cardExpiryYear,
                       decimal amount,
                       string currency,
                       string cvv,
                       string encriptionKey,
                       string bankIdentifier, string paymentStatus)
        {
            Id = Guid.NewGuid();
            CardNumber = cardNumber;
            CardExpiryMonth = cardExpiryMonth;
            CardExpiryYear = cardExpiryYear;
            Amount = amount;
            Currency = currency;
            CVV = cvv;
            EncriptionKey = encriptionKey;
            BankPaymentIdentifier = bankIdentifier;
            PaymentStatus = paymentStatus;
        }

        

        public Guid Id { get; }

        public DateTime OccurredOn => DateTime.UtcNow;

        public string CardNumber { get; }

        public string CardExpiryMonth { get; }

        public string CardExpiryYear { get; }

        public decimal Amount { get; }

        public string Currency { get; }

        public string CVV { get; }

        public string EncriptionKey { get; }

        public string BankPaymentIdentifier { get; }

        public string PaymentStatus { get;  }
    }
}
