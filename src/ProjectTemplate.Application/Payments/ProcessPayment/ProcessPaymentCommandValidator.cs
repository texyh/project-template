using FluentValidation;
using ProjectTemplate.Domain.Payments.Commands;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace ProjectTemplate.Application.Payments.ProcessPayment
{
    public class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
    {
        private const byte MinimumAmount = 0;
        private  string[] AcceptedCurrencies = new string[] { "EUR", "USD" };
        public ProcessPaymentCommandValidator()
        {
            RuleFor(x => x.CardNumber).NotNull().NotEmpty().CreditCard();
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(MinimumAmount).WithMessage("amount must be greater than 0");
            RuleFor(x => x.Currency).NotNull().Must(x => AcceptedCurrencies.Contains(x)).WithMessage("The currency is not support");
            RuleFor(x => x.CardExpiryYear).NotEmpty().NotNull().Must((x, y) => ValidateCardExpiryDate(x)).WithMessage("The credit card is expired");
            RuleFor(x => x.CVV).NotEmpty().NotNull();
            RuleFor(x => x.MerchantId).NotNull().NotEmpty();
            RuleFor(x => x.CardExpiryYear).NotEmpty().NotNull().Must(x => int.TryParse(x, out var value)).WithMessage("expiry year is not valid");
            RuleFor(x => x.CardExpiryMonth).NotEmpty().NotNull().Must(x => int.TryParse(x, out var value)).WithMessage("expiry month is not valid");

        }

        private bool ValidateCardExpiryDate(ProcessPaymentCommand command)
        {
            var isValidYear = int.TryParse(command.CardExpiryYear, out var year);
            var isValidMonth = int.TryParse(command.CardExpiryMonth, out var month);
            var today = DateTime.UtcNow;

            if(isValidYear && year > today.Year)
            {
                return true;
            }

            else if (isValidMonth && isValidYear && year == today.Year &&  month > today.Month) 
            {
                return true; // The same year.
            }

            else
            {
                return false;
            }
        }
    }
}
