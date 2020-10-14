using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate.Application.Payments.GetPayment;
using ErrorResult = ProjectTemplate.Application.Payments.GetPayment.ErrorResult;
using SuccessResult = ProjectTemplate.Application.Payments.GetPayment.SuccessResult;

namespace ProjectTemplate.Api.UseCases.GetPayment
{
    public static class Result
    {
        public static IActionResult For(GetPaymentResult result) 
        {
            return result switch
            {
                SuccessResult r => new OkObjectResult(r),
                ErrorResult e => new NotFoundObjectResult(e.Message),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
    }
}