using System;

namespace ProjectTemplate.Domain.Abstractions
{
    public interface IDomainEvent
    {
        Guid Id { get; }

        DateTime OccurredOn { get; }
    }
}