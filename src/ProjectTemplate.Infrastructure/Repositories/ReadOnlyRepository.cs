using Marten;
using ProjectTemplate.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Infrastructure.Repositories
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T>
    {
        private readonly IDocumentStore _store;
        public ReadOnlyRepository(IDocumentStore store)
        {
            _store = store;
        }

        public async Task<T> FindBy(Guid id)
        {
            using(var session = _store.OpenSession())
            {
                return await session.LoadAsync<T>(id);
            }
        }
    }
}
