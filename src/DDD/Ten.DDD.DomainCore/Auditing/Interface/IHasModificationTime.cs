using System;

namespace Ten.DDD.DomainCore.Auditing.Interface
{
    /// <summary>
    /// 将最后修改时间增加到实体中的接口
    /// </summary>
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
}