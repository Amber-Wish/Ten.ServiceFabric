using System;

namespace Ten.DDD.DomainCore.Auditing.Interface
{
    public interface IHasDeletionTime:ISoftDelete
    {
        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}