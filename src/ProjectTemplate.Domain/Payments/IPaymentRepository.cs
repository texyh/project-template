using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Domain.Payments
{
    public interface IPaymentRepository
    {
        Task AppendChanges(Payment payment);

        Task<Payment> Load(Guid Id);
    }
}
