using FluentAssertions;
using ProjectTemplate.Application.Payments.ProcessPayment;
using ProjectTemplate.Domain.Payments.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ProjectTemplate.UnitTests
{
    public class ProcessPaymentCommandValidatorShould
    {

        [Fact]
        public void Not_Allow_Non_Supported_Currencies()
        {
            var command = new ProcessPaymentCommand
            {
                Amount = 100,
                Currency = "AUS",
                CardExpiryYear = "2021",
                CardExpiryMonth = "04",
            };

            var validator = new ProcessPaymentCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Select(x => x.ErrorMessage).Should().Contain("The currency is not support");
        }

        [Fact]
        public void Not_Allow_Expired_CreditCard()
        {
            var command = new ProcessPaymentCommand
            {
                Amount = 100,
                Currency = "AUS",
                CardExpiryYear = "2020",
                CardExpiryMonth = "04",
            };

            var validator = new ProcessPaymentCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Select(x => x.ErrorMessage).Should().Contain("The credit card is expired");
        }

        [Fact]
        public void Not_Allow_String_As_CardYear_And_CardMonth()
        {
            var command = new ProcessPaymentCommand
            {
                Amount = 100,
                Currency = "AUS",
                CardExpiryYear = "xxxx",
                CardExpiryMonth = "xxx",
            };

            var validator = new ProcessPaymentCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Select(x => x.ErrorMessage).Should().Contain("expiry year is not valid");
            result.Errors.Select(x => x.ErrorMessage).Should().Contain("expiry month is not valid");
        }

        [Fact]
        public void Not_Allow_Negative_Amount()
        {
            var command = new ProcessPaymentCommand
            {
                Amount = -1,
                Currency = "AUS",
                CardExpiryYear = "xxxx",
                CardExpiryMonth = "xxx",
            };

            var validator = new ProcessPaymentCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Select(x => x.ErrorMessage).Should().Contain("amount must be greater than 0");
        }


    }
}
