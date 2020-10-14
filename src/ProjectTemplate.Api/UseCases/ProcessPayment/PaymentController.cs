using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate.Domain.Payments;
using ProjectTemplate.Domain.Payments.Commands;

namespace ProjectTemplate.Api.UseCases.ProcessPayment
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody]PaymentRequest request)
        {
            var command = new ProcessPaymentCommand
            {
                Amount = request.Amount,
                Currency = request.Currency,
                CardExpiryYear = request.CardExpiryYear,
                CardExpiryMonth = request.CardExpiryMonth,
                CardNumber = request.CardNumber,
                CVV = request.CVV,
                MerchantId = request.MerchantId
            };

            return Result.For(await _mediator.Send(command));
        }
    }
}