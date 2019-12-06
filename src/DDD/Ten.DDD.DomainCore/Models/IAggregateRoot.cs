namespace Ten.DDD.DomainCore.Models
{
    /// <summary>
    /// 定义聚合根
    /// 它的主键可能不是“Id”，也可能具有复合主键
    /// </summary>
    public interface IAggregateRoot:IEntity
    {
        
    }

    /// <summary>
    /// 使用具有“Id”属性的单个主键定义聚合根
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IAggregateRoot<TKey> : IEntity<TKey>
    {

    }

}