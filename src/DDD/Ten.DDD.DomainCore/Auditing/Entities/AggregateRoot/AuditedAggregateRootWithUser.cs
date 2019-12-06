using System;
using Ten.DDD.DomainCore.Auditing.Interface;
using Ten.DDD.DomainCore.Models;

namespace Ten.DDD.DomainCore.Auditing.Entities.AggregateRoot
{
    [Serializable]
    public abstract class AuditedAggregateRootWithUser<TUser>:
        AuditedAggregateRoot,
        IAuditedObject<TUser> 
        where TUser:IEntity<long>
    {
        public virtual TUser Creator { get; set; }
        public virtual TUser LastModifier { get; set; }
    }

    [Serializable]
    public abstract class AuditedAggregateRootWithUser<TKey, TUser> : AuditedAggregateRoot<TKey>, IAuditedObject<TUser>
        where TUser : IEntity<long>
    {
        public virtual TUser Creator { get; set; }

        public virtual TUser LastModifier { get; set; }

        protected AuditedAggregateRootWithUser()
        {

        }

        protected AuditedAggregateRootWithUser(TKey id)
            : base(id)
        {

        }
    }

}