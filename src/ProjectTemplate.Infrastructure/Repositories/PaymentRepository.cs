using Marten;
using Microsoft.EntityFrameworkCore;
using ProjectTemplate.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Infrastructure
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDocumentStore _documentStore;

        public PaymentRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task AppendChanges(Payment payment)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Events.Append(payment.Id, payment.GetDomainEvents());
                await session.SaveChangesAsync();
            }
        }

        public async Task<Payment> Load(Guid Id)
        {
            using (var session = _documentStore.OpenSession())
            {
                return await session.Events.AggregateStreamAsync<Payment>(Id);
            }
        }
    }
}
