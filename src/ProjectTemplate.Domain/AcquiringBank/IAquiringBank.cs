using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Domain.AcquiringBank
{
    public interface IAquiringBankClient
    {
        Task<BankPaymentResponse> ProcessPayment(BankPaymentRequest request);
    }
}
