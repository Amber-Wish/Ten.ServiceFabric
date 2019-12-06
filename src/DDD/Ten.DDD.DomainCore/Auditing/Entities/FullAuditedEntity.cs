using System;
using Ten.DDD.DomainCore.Auditing.Interface;

namespace Ten.DDD.DomainCore.Auditing.Entities
{
    [Serializable]
    public abstract class FullAuditedEntity:AuditedEntity,IFullAuditedObject
    {
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual Guid? DeleterId { get; set; }
    }

    [Serializable]
    public abstract class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IFullAuditedObject
    {
        public virtual bool IsDeleted { get; set; }

        public virtual Guid? DeleterId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }

        protected FullAuditedEntity()
        {

        }

        protected FullAuditedEntity(TKey id)
            : base(id)
        {

        }
    }

}