namespace Ten.DDD.DomainCore.Models
{
    /// <summary>
    /// 基类
    /// </summary>
    public interface IEntity
    {
        object[] GetKeys();
    }

    /// <summary>
    /// 泛型 基类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }
    }

}