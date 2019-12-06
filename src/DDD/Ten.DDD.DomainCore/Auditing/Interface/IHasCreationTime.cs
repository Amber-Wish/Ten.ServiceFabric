using System;

namespace Ten.DDD.DomainCore.Auditing.Interface
{
    /// <summary>
    /// 将创建时间加入到实体中的 接口
    /// </summary>
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; set; }
    }
}