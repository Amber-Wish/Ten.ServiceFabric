using System;
using System.Reflection;
using Ten.ServiceFabric.Util.Extensions;

namespace Ten.DDD.DomainCore.Models
{
    /// <summary>
    /// 定义领域实体基类
    /// </summary>
    [Serializable]
    public abstract class Entity:IEntity
    {
        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Keys = {GetKeys().JoinAsString(", ")}";
        }
        public abstract object[] GetKeys();
    }

    /// <summary>
    /// 定义领域实体泛型基类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    [Serializable]
    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public TKey Id { get; }

        protected Entity()
        {

        }

        protected Entity(TKey id)
        {
            Id = id;
        }

        /// <summary>
        /// 重写方法 相等运算
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TKey>))
            {
                return false;
            }

            //相同实例必须视为相等
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = (Entity<TKey>)obj;

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType().GetTypeInfo();
            var typeOfOther = other.GetType().GetTypeInfo();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        /// <summary>
        /// 重写方法 实体比较 ==
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Entity<TKey> a, Entity<TKey> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// 重写方法 实体比较 !=
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Entity<TKey> a, Entity<TKey> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// 获取哈希
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        /// <summary>
        /// 获取唯一标识
        /// </summary>
        /// <returns></returns>
        public override object[] GetKeys()
        {
            return new object[] { Id };
        }
    }


}