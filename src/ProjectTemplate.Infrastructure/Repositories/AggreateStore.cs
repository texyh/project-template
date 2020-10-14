using Marten;
using ProjectTemplate.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Infrastructure.Repositories
{
    public class AggreateStore<T> : IAggregateStore<T> where T : AggregateRoot
    {
        private readonly IDocumentStore _documentStore;

        public AggreateStore(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task AppendChanges(T aggregate)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Events.Append(aggregate.Id, aggregate.GetDomainEvents());
                await session.SaveChangesAsync();
            }
        }

        public async Task<T> Load(Guid Id)
        {
            using (var session = _documentStore.OpenSession())
            {
                return await session.Events.AggregateStreamAsync<T>(Id);
            }
        }
    }
}
