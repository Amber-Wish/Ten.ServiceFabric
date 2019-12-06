using System;

namespace Ten.DDD.DomainCore.Models
{
    [Serializable]
    public abstract class AggregateRoot:Entity,IAggregateRoot
    {
    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {
        protected AggregateRoot()
        {

        }

        protected AggregateRoot(TKey id)
            : base(id)
        {

        }
    }


}