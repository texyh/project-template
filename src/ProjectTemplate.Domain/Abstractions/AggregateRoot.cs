using System;
using System.Collections.Generic;

namespace ProjectTemplate.Domain.Abstractions
{
    public abstract class AggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents;

        public Guid Id { get; protected set; }

        //public int Version { get; private set; }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent @event)
        {
            _domainEvents.Add(@event);
        }

        protected AggregateRoot()
        {
            _domainEvents = new List<IDomainEvent>();

            //Version = -1;
        }
    }
}