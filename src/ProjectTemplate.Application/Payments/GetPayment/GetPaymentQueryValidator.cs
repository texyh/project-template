using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTemplate.Application.Payments.GetPayment
{
    public class GetPaymentQueryValidator : AbstractValidator<GetPaymentQuery>
    {
        public GetPaymentQueryValidator()
        {
            RuleFor(x => x.PaymentId).NotEmpty().NotNull();
        }
    }
}
