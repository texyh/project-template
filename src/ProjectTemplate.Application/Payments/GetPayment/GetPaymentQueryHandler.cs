using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectTemplate.Application.Abstractions.Queries;
using ProjectTemplate.Application.Crypto;
using ProjectTemplate.Application.Payments.GetPayment;
using ProjectTemplate.Domain.Helpers;
using ProjectTemplate.Domain.Payments;
using Serilog;

namespace ProjectTemplate.Api.UseCases.GetPayment
{
    public class GetPaymentQueryHandler : IQueryHandler<GetPaymentQuery, GetPaymentResult>
    {
        private readonly IPaymentRepository _paymentRepository;
        
        private readonly ICryptoService _cryptoService;

        private readonly ILogger _logger;

        public GetPaymentQueryHandler(
            IPaymentRepository paymentRepository,
            ICryptoService cryptoService,
            ILogger logger)
        {
            _paymentRepository = paymentRepository;
            _cryptoService = cryptoService;
            _logger = logger;
        }

        public async Task<GetPaymentResult> Handle(GetPaymentQuery query, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.Load(Guid.Parse(query.PaymentId));
           
            if(payment == null) 
            {
                _logger.Error($"There is no payment with id: {query.PaymentId}");
                return new ErrorResult($"There is no payment with id: {query.PaymentId}");
            }

            var cardNumber = _cryptoService.Decrypt(payment.CardNumber, payment.EncriptionKey);

            return new SuccessResult 
            {
                CardNumber = cardNumber.Mask(),
                Amount = payment.Amount,
                PaymentDate = payment.CreatedDate,
                PaymentStatus = payment.PaymentStatus,
                Currency = payment.Currency
            };
        }
    }
}