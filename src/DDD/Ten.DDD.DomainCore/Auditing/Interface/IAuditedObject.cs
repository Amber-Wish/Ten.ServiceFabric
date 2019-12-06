namespace Ten.DDD.DomainCore.Auditing.Interface
{
    /// <summary>
    /// 实现此接口可以将(新增，修改)审计属性添加到类中
    /// </summary>
    public interface IAuditedObject:ICreationAuditedObject,
        IModificationAuditedObject
    {
        
    }

    public interface IAuditedObject<TUser> : IAuditedObject,
        ICreationAuditedObject<TUser>,
        IModificationAuditedObject<TUser>
    {

    }


}