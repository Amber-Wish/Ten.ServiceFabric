using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ten.DDD.DomainCore.Models;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Z.BulkOperations;

namespace Ten.DDD.DomainCore
{
    public interface IRepositoryExtension<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        List<TResult> FindList<TResult>(Expression<Func<TEntity, bool>> condition = null);
        Task<List<TResult>> FindListAsync<TResult>(Expression<Func<TEntity, bool>> condition = null);

        List<TResult> FindList<TResult>(Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction);

        Task<List<TResult>> FindListAsync<TResult>(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction);

        /// <summary>
        /// 查询单个指定字段的对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        TResult Find<TResult>(Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// 查询单个指定字段的对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<TResult> FindAsync<TResult>(Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// 根据表达式直接删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="condition"></param>
        void RemoveDirect(Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// 根据表达式直接删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="condition"></param>
        Task RemoveDirectAsync(Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="filterAction">构建查询条件方法</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        List<TEntity> GetPageList(Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, PageRequest pagination);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="filterAction"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetPageListAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, PageRequest pagination);

        /// <summary>
        /// 分页、关联查询指定字段对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="includeExpression"></param>
        /// <param name="filterAction"></param>
        /// <param name="selector"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        List<TResult> GetPageListByInclude<TResult>(Expression<Func<TEntity, object>> includeExpression,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, Expression<Func<TEntity, TResult>> selector,
            PageRequest pagination);

        /// <summary>
        /// 分页、关联查询指定字段对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="includeExpression"></param>
        /// <param name="filterAction"></param>
        /// <param name="selector"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        Task<List<TResult>> GetPageListByIncludeAsync<TResult>(
            Expression<Func<TEntity, object>> includeExpression,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, Expression<Func<TEntity, TResult>> selector,
            PageRequest pagination);

        /// <summary>
        /// 获取分页数据(有out参数不能用async）
        /// </summary>
        /// <typeparam name="TOrderKey"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total">指定条件下的总数量</param>
        /// <param name="filterAction">条件过滤委托</param>
        /// <param name="isAsc">是否按指定条件的升序排列</param>
        /// <param name="orderByLambda">排序条件</param>
        /// <returns></returns>
        List<TEntity> GetPageList<TOrderKey>(int pageSize, int pageIndex, out long total,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, bool isAsc,
            Expression<Func<TEntity, TOrderKey>> orderByLambda);

        List<TResult> GetPageList<TOrderKey, TResult>(int pageSize, int pageIndex, out long total,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, bool isAsc,
            Expression<Func<TEntity, TOrderKey>> orderByLambda);

        /// <summary>
        /// 少量参数分页(有out参数不能用async）
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <param name="filterAction"></param>
        /// <returns></returns>
        List<TEntity> GetPageList(int pageSize, int pageIndex, out long total,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction);

        /// <summary>
        /// 多条件查询，不分页
        /// </summary>
        /// <typeparam name="TOrderKey"></typeparam>
        /// <param name="filterAction"></param>
        /// <param name="isAsc"></param>
        /// <param name="orderByLambda"></param>
        /// <returns></returns>
        List<TEntity> GetPageListNoPage<TOrderKey>(Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction,
            bool isAsc, Expression<Func<TEntity, TOrderKey>> orderByLambda);

        /// <summary>
        /// 多条件查询，不分页
        /// </summary>
        /// <typeparam name="TOrderKey"></typeparam>
        /// <param name="filterAction"></param>
        /// <param name="isAsc"></param>
        /// <param name="orderByLambda"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetPageListNoPageAsync<TOrderKey>(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, bool isAsc,
            Expression<Func<TEntity, TOrderKey>> orderByLambda);


        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        /// <param name="isCommit">是否提交保存到数据库</param>
        bool Add(TEntity[] entities, bool isCommit = false);

        /// <summary>
        /// 新增实体列表AddAsync
        /// </summary>
        /// <param name="entities">新增实体列表</param>
        /// <param name="isCommit">是否提交保存到数据库</param>
        Task<bool> AddAsync(TEntity[] entities, bool isCommit = false);

        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        /// <param name="isCommit">是否提交保存到数据库</param>
        bool Add(TEntity entities, bool isCommit = false);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        /// <param name="isCommit">是否提交保存到数据库</param>
        Task<bool> AddAsync(TEntity entities, bool isCommit = false);

        /// <summary>
        /// BulkInsert方式批量写入实体列表(自动提交事务)
        /// </summary>
        /// <param name="entities">新增实体列表</param>
        void BulkInsert(TEntity[] entities);

        /// <summary>
        /// BulkInsert方式批量写入实体列表(自动提交事务)
        /// </summary>
        /// <param name="entities">新增实体列表</param>
        Task BulkInsertAsync(TEntity[] entities);

        /// <summary>
        /// 直接更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updatedProperties">表达式：m => new { m.SellerName, m.SellerId }</param>
        /// <param name="isCommit">是否提交</param>
        void UpdateDirect(TEntity entity, Expression<Func<TEntity, object>> updatedProperties = null,
            bool isCommit = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateAction"></param>
        /// <param name="isCommit">是否提交更改到数据库</param>
        void Update(TPrimaryKey id, Action<TEntity> updateAction, bool isCommit = false);

        /// <summary>
        /// BulkUpdate方式批量更新实体列表(自动提交事务)
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="bulkOperationFactory"></param>
        void BulkUpdate(TEntity[] entities, Action<BulkOperation<TEntity>> bulkOperationFactory);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="updateAction"></param>
        /// <param name="isCommit"></param>
        /// <returns></returns>
        Task UpdateAsync(TEntity[] entitys, Action<TEntity> updateAction, bool isCommit = false);

        /// <summary>
        /// 更新，同时提交
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updatedProperties"></param>
        bool UpdateCommit(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties);

        Task<bool> UpdateCommitAsync(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties);
        void Update(TEntity entitie, params Expression<Func<TEntity, object>>[] updatedProperties);
        void Update(TEntity[] entities, params Expression<Func<TEntity, object>>[] updatedProperties);
        bool UpdateCommit(TEntity[] entities, params Expression<Func<TEntity, object>>[] updatedProperties);
        Task UpdateCommitAsync(TEntity[] entities, params Expression<Func<TEntity, object>>[] updatedProperties);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateAction"></param>
        /// <param name="isCommit">是否提交更改到数据库</param>
        /// <returns></returns>
        Task UpdateAsync(TPrimaryKey id, Action<TEntity> updateAction, bool isCommit = false);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        /// <param name="isCommit">是否提交更改到数据库</param>
        Task UpdateAsync(TEntity[] entities, bool isCommit = false);

        /// <summary>
        /// 直接批量更新，不先从数据库查询出实体对象
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="updatedProperties">表达式：m => new { m.SellerName, m.SellerId }</param>
        /// <param name="isCommit">是否提交</param>
        void UpdateDirect(TEntity[] entitys, Expression<Func<TEntity, object>> updatedProperties = null,
            bool isCommit = false);

        /// <summary>
        /// 直接批量更新，不先从数据库查询出实体对象
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="updatedProperties">表达式：m => new { m.SellerName, m.SellerId }</param>
        /// <param name="isCommit">是否提交</param>
        Task UpdateDirectAsync(TEntity[] entitys,
            Expression<Func<TEntity, object>> updatedProperties = null, bool isCommit = false);

        /// <summary>
        /// 直接更新，不先从数据库查询出实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updatedProperties">表达式：m => new { m.SellerName, m.SellerId }</param>
        /// <param name="isCommit">是否提交</param>
        Task UpdateDirectAsync(TEntity entity, Expression<Func<TEntity, object>> updatedProperties = null, bool isCommit = false);

        /// <summary>
        /// 查询并更新
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="updateExpression"></param>
        void UpdateFromQuery(Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TEntity>> updateExpression);

        /// <summary>
        /// 异步 查询并更新
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        Task UpdateFormQueryAsync(Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TEntity>> updateExpression);
    }
    public interface IRepositoryExtension<TEntity> : IRepositoryExtension<TEntity, int>
        where TEntity : class, IEntity<int>
    {
    }
}