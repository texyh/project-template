using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectTemplate.Api.UseCases.GetPayment;
using ProjectTemplate.Application.Payments.GetPayment;
using ProjectTemplate.Domain.Payments;
using Xunit;

namespace ProjectTemplate.UnitTests.UseCases.GetPayment
{
    public class PaymentControllerShould
    {
        
        [Fact]
        public async Task GetPayment()
        {
            var mockqueryHandler = new Mock<IMediator>();
            var expectedValue = new SuccessResult 
            {
                CardNumber = "***************3467",
                Amount = 100,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Success,
                Currency = "EUR"
            };
            mockqueryHandler.Setup(x => x.Send(It.IsAny<GetPaymentQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedValue);
            var sut = new PaymentController(mockqueryHandler.Object);
            var paymentId = Guid.NewGuid().ToString();

            var response = await sut.GetPayment(paymentId);

            var httpResponse = response as OkObjectResult;
            httpResponse.Value.Should().Be(expectedValue);
        }

        [Fact]
        public async Task Return_Error_If_No_Payment_Is_Found() 
        {
            var paymentId = Guid.NewGuid().ToString();
            var mockqueryHandler = new Mock<IMediator>();
            mockqueryHandler.Setup(x => x.Send(It.IsAny<GetPaymentQuery>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(new ErrorResult($"There is no payment with id: {paymentId}"));
            var sut = new PaymentController(mockqueryHandler.Object);
            
            var response = await sut.GetPayment(paymentId);

            var httpResponse = response as NotFoundObjectResult;
            httpResponse.Value.Should().Be($"There is no payment with id: {paymentId}");
        }
    }
}