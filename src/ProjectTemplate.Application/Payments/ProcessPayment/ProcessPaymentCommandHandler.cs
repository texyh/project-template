using MediatR;
using ProjectTemplate.Application.Abstractions.Commands;
using ProjectTemplate.Application.Crypto;
using ProjectTemplate.Domain.Abstractions;
using ProjectTemplate.Domain.AcquiringBank;
using ProjectTemplate.Domain.Payments;
using ProjectTemplate.Domain.Payments.Commands;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectTemplate.Api.UseCases.ProcessPayment
{
    public class ProcessPaymentCommandHandler : ICommandHandler<ProcessPaymentCommand, ProcessPaymentResult>
    {
        private readonly IAggregateStore<Payment> _paymentRepository;

        private readonly ICryptoService _cryptoService;

        private readonly IAquiringBankClient _acuquiryBank;

        private readonly ILogger _logger;

        public ProcessPaymentCommandHandler(
            IAggregateStore<Payment> paymentRepository,
            ICryptoService cryptoService,
            IAquiringBankClient acuquiryBank,
            ILogger logger)
        {
            _paymentRepository = paymentRepository;
            _cryptoService = cryptoService;
            _acuquiryBank = acuquiryBank;
            _logger = logger;
        }

        public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken)
        {
            _logger.Information("starting acquring bank payment request");
            var bankPayemntResult = await _acuquiryBank.ProcessPayment(new BankPaymentRequest 
            {
                Amount = command.Amount,
                Currency = command.Currency,
                CardExpiryYear = command.CardExpiryYear,
                CardExpiryMonth = command.CardExpiryMonth,
                CardNumber = command.CardNumber,
                CVV = command.CVV,
                MerchantId = Guid.NewGuid().ToString()
            });

            var encriptionKey = Guid.NewGuid().ToString("N");
            var encriptedCardNumber = _cryptoService.Encrypt(command.CardNumber, encriptionKey);
            var encriptedCardMonth = _cryptoService.Encrypt(command.CardExpiryMonth, encriptionKey);
            var encriptedCardYear = _cryptoService.Encrypt(command.CardExpiryYear, encriptionKey);
            var encriptedCardCVV = _cryptoService.Encrypt(command.CVV, encriptionKey);

            var payment = new Payment(encriptedCardNumber,
                                      encriptedCardMonth,
                                      encriptedCardYear,
                                      command.Amount,
                                      command.Currency,
                                      encriptedCardCVV,
                                      encriptionKey,
                                      bankPayemntResult.PaymentIdentifier,
                                      bankPayemntResult.PaymentStatus);


            await _paymentRepository.AppendChanges(payment);


            if(bankPayemntResult.PaymentStatus == PaymentStatus.Success) 
            {
                _logger.Information("bank payment successful");
                return new SuccessResult(payment.Id.ToString());
            } 
            else 
            {
                _logger.Error("The Bank was unable to process the payment");
                return new ErrorResult("The Bank was unable to process the payment");
            }

        }

    }
}
