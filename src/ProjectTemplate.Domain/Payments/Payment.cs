using ProjectTemplate.Domain.Abstractions;
using ProjectTemplate.Domain.Payments.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTemplate.Domain.Payments
{
    public class Payment : AggregateRoot
    {

        public  Payment()
        {


        }

        public Payment(string cardNumber,
                       string cardExpiryMonth,
                       string cardExpiryYear,
                       decimal amount,
                       string currency, 
                       string cvv, 
                       string encriptionKey, 
                       string bankIdentifier, string paymentStatus)
        {
            var @event = new PaymentCreatedEvent(cardNumber,
                cardExpiryMonth,
                cardExpiryYear,
                amount,
                currency, cvv, encriptionKey, bankIdentifier, paymentStatus);
            AddDomainEvent(@event);
            Apply(@event);
        }

        public string CardNumber { get; private  set;}

        public string CardExpiryMonth { get; private set;}

        public string CardExpiryYear { get; private set;}

        public decimal Amount { get; private set;}

        public string Currency { get; private set;}

        public string CVV { get; private set;}

        public string EncriptionKey { get; private set;}

        public string BankPaymentIdentifier { get; private set;}

        public string PaymentStatus {get; private set;}

        public DateTime CreatedDate { get; private set;}

        public void Apply(PaymentCreatedEvent @event)
        {
            When((dynamic)@event);
        }

        private void When(PaymentCreatedEvent @event)
        {
            Id = @event.Id;
            CardNumber = @event.CardNumber;
            CardExpiryMonth = @event.CardExpiryMonth;
            CardExpiryYear = @event.CardExpiryYear;
            Amount = @event.Amount;
            Currency = @event.Currency;
            CVV = @event.CVV;
            EncriptionKey = @event.EncriptionKey;
            BankPaymentIdentifier = @event.BankPaymentIdentifier;
            PaymentStatus = @event.PaymentStatus;
            CreatedDate = @event.OccurredOn;
        }
    }
}
