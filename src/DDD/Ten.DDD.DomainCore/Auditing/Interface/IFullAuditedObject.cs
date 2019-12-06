namespace Ten.DDD.DomainCore.Auditing.Interface
{
    /// <summary>
    /// 实现此接口可以将全部审计属性添加到类中
    /// </summary>
    public interface IFullAuditedObject: IAuditedObject, 
        IDeletionAuditedObject
    {
        
    }

    public interface IFullAuditedObject<TUser> : IFullAuditedObject,
        IAuditedObject<TUser>,
        IDeletionAuditedObject<TUser>
    {

    }


}