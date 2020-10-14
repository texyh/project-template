using ProjectTemplate.Application.Abstractions.Queries;

namespace ProjectTemplate.Application.Payments.GetPayment
{
    public class GetPaymentQuery : IQuery<GetPaymentResult>
    {
        public string PaymentId { get; set; }
    }
}