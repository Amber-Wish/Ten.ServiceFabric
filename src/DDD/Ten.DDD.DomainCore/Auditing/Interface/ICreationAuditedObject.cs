using System;

namespace Ten.DDD.DomainCore.Auditing.Interface
{
    /// <summary>
    /// 可以实现此接口来存储创建信息（谁创建的以及创建时间）
    /// </summary>
    public interface ICreationAuditedObject:IHasCreationTime
    {
        Guid CreatorId { get; set; }
    }

    public interface ICreationAuditedObject<TUser> : ICreationAuditedObject
    {
        TUser Creator { get; set; }
    }


}