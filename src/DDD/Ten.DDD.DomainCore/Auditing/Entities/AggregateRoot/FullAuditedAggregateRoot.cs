using System;
using Ten.DDD.DomainCore.Auditing.Interface;

namespace Ten.DDD.DomainCore.Auditing.Entities.AggregateRoot
{
    [Serializable]
    public abstract class FullAuditedAggregateRoot:AuditedAggregateRoot,IFullAuditedObject
    {
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual Guid? DeleterId { get; set; }
    }

    [Serializable]
    public abstract class FullAuditedAggregateRoot<TKey> : AuditedAggregateRoot<TKey>, IFullAuditedObject
    {
        public virtual bool IsDeleted { get; set; }

        public virtual Guid? DeleterId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }

        protected FullAuditedAggregateRoot()
        {

        }

        protected FullAuditedAggregateRoot(TKey id)
            : base(id)
        {

        }
    }

}