using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTemplate.Domain.Abstractions
{
    public class Entity<T> : IEquatable<Entity<T>>
    {
        public T Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<T>);
        }

        public bool Equals(Entity<T> other)
        {
            return other != null &&
                   EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<T>.Default.GetHashCode(Id);
        }
    }
}
