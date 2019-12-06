using System;

namespace Ten.DDD.DomainCore.Auditing.Interface
{
    /// <summary>
    /// 可以实现此接口来存储删除信息（谁删除的以及删除时间）
    /// </summary>
    public interface IDeletionAuditedObject:IHasDeletionTime
    {
        Guid? DeleterId { get; set; }
    }

    public interface IDeletionAuditedObject<TUser> : IDeletionAuditedObject
    {
        TUser Deleter { get; set; }
    }

}