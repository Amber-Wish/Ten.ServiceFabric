using System;

namespace Ten.DDD.DomainCore.Auditing.Interface
{
    /// <summary>
    /// 可以实现此接口来存储修改信息（谁修改的以及修改时间）
    /// </summary>
    public interface IModificationAuditedObject:IHasModificationTime
    {
        Guid? LastModifierId { get; set; }
    }

    public interface IModificationAuditedObject<TUser> : IModificationAuditedObject
    {
        TUser LastModifier { get; set; }
    }


}