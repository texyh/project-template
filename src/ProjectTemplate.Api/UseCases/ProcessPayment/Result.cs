using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate.Domain.Payments;
using ProjectTemplate.Domain.Payments.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTemplate.Api.UseCases.ProcessPayment
{
    public class Result
    {
        public static IActionResult For(ProcessPaymentResult output) => output switch
        {
            SuccessResult result => new CreatedResult($"api/payments/{result.PaymentId}", new PaymentResponse { PaymentId = result.PaymentId }),
            ErrorResult error => new BadRequestObjectResult(error.Message),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}
