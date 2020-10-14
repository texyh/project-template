using Moq;
using ProjectTemplate.Api.UseCases.ProcessPayment;
using ProjectTemplate.Domain.AcquiringBank;
using ProjectTemplate.Domain.Payments;
using ProjectTemplate.Domain.Payments.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Serilog;
using ProjectTemplate.Application.Crypto;
using ProjectTemplate.Domain.Abstractions;

namespace ProjectTemplate.UnitTests.UseCases.ProcessPayment
{
    public class PaymentProcessHandlerShould
    {

        [Fact]
        public async Task Proccess_And_Persist_Payment()
        {
            var someId = Guid.NewGuid().ToString();
            var mockPaymentRepository = new Mock<IAggregateStore<Payment>>();
            var mockCyptoService = new Mock<ICryptoService>();
            var mockBankClient = new Mock<IAquiringBankClient>();
            var mockLogger = new Mock<ILogger>();
            mockBankClient.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                .ReturnsAsync(new BankPaymentResponse { PaymentIdentifier = someId, PaymentStatus = PaymentStatus.Success });
            mockCyptoService.Setup(x => x.Encrypt(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("__encripted__");
            var sut = new ProcessPaymentCommandHandler(mockPaymentRepository.Object, mockCyptoService.Object, mockBankClient.Object, mockLogger.Object);
            ProcessPaymentCommand command = new ProcessPaymentCommand
            {
                Amount = 100,
                Currency = "EUR",
                CardExpiryYear = "24",
                CardExpiryMonth = "4",
                CardNumber = "5564876598743467",
                CVV = "782",
            };

            await sut.Handle(command, new System.Threading.CancellationToken());

            mockCyptoService.Verify(x => x.Encrypt(command.CardNumber, It.IsAny<string>()), Times.Once);
            mockPaymentRepository.Verify(x => x.AppendChanges(It.Is<Payment>(y => y.CardNumber == "__encripted__")), Times.Once);
            mockPaymentRepository.Verify(x => x.AppendChanges(It.Is<Payment>(y => y.BankPaymentIdentifier == someId)), Times.Once);
            mockPaymentRepository.Verify(x => x.AppendChanges(It.Is<Payment>(y => y.PaymentStatus == PaymentStatus.Success)), Times.Once);
        }

        [Fact]
        public async Task Proccess_And_Persist_Payment_When_Acquiring_Bank_Fails_To_Process_Payment()
        {
            var someId = Guid.NewGuid().ToString();
            var mockPaymentRepository = new Mock<IAggregateStore<Payment>>();
            var mockCyptoService = new Mock<ICryptoService>();
            var mockBankClient = new Mock<IAquiringBankClient>();
            var mockLogger = new Mock<ILogger>();
            mockBankClient.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                .ReturnsAsync(new BankPaymentResponse { PaymentIdentifier = someId, PaymentStatus = PaymentStatus.Failed });
            mockCyptoService.Setup(x => x.Encrypt(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("__encripted__");
            var sut = new ProcessPaymentCommandHandler(mockPaymentRepository.Object, mockCyptoService.Object, mockBankClient.Object, mockLogger.Object);
            ProcessPaymentCommand command = new ProcessPaymentCommand
            {
                Amount = 100,
                Currency = "EUR",
                CardExpiryYear = "24",
                CardExpiryMonth = "4",
                CardNumber = "5564876598743467",
                CVV = "782",
            };

            await sut.Handle(command, new System.Threading.CancellationToken());

            mockCyptoService.Verify(x => x.Encrypt(command.CardNumber, It.IsAny<string>()), Times.Once);
            mockPaymentRepository.Verify(x => x.AppendChanges(It.Is<Payment>(y => y.CardNumber == "__encripted__")), Times.Once);
            mockPaymentRepository.Verify(x => x.AppendChanges(It.Is<Payment>(y => y.BankPaymentIdentifier == someId)), Times.Once);
            mockPaymentRepository.Verify(x => x.AppendChanges(It.Is<Payment>(y => y.PaymentStatus == PaymentStatus.Failed)), Times.Once);
        }

    }
}
