using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ten.DDD.DomainCore;
using Ten.DDD.DomainCore.Models;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Z.BulkOperations;

namespace Ten.DDD.EFCore
{
    public class RepositoryExtensions<TDbContext, TEntity, TPrimaryKey> : EfCoreRepository<TDbContext, TEntity, TPrimaryKey>,
        IRepositoryExtension<TEntity, TPrimaryKey>
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        private readonly IMapper _mapper;
        public RepositoryExtensions(IDbContextProvider<TDbContext> dbContextProvider, IMapper mapper) : base(dbContextProvider)
        {
            _mapper = mapper;
        }

        public List<TResult> FindList<TResult>(Expression<Func<TEntity, bool>> condition = null)
        {
            if (condition == null)
            {
                //return DbContext.Set<TEntity>().ProjectTo<TResult>(Mapper).ToList();
                return _mapper.ProjectTo<TResult>(DbContext.Set<TEntity>()).ToList();
            }
            return _mapper.ProjectTo<TResult>(DbContext.Set<TEntity>().Where(condition)).ToList();
        }

        public async Task<List<TResult>> FindListAsync<TResult>(Expression<Func<TEntity, bool>> condition = null)
        {
            if (condition == null)
            {
                return await _mapper.ProjectTo<TResult>(DbContext.Set<TEntity>()).ToListAsync();
            }
            return await _mapper.ProjectTo<TResult>(DbContext.Set<TEntity>().Where(condition)).ToListAsync();
        }

        public List<TResult> FindList<TResult>(Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction)
        {
            var query = DbContext.Set<TEntity>() as IQueryable<TEntity>;
            if (query == null)
                throw new Exception($"{typeof(TEntity)}对象query为空！");
            if (filterAction != null)
                query = filterAction(query);
            return _mapper.ProjectTo<TResult>(query.AsNoTracking()).ToList();
        }

        public async Task<List<TResult>> FindListAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction)
        {
            var query = DbContext.Set<TEntity>() as IQueryable<TEntity>;
            if (query == null)
                throw new Exception($"{typeof(TEntity)}对象query为空！");
            if (filterAction != null)
                query = filterAction(query);
            return await _mapper.ProjectTo<TResult>(query.AsNoTracking()).ToListAsync();
        }

        /// <summary>
        /// 查询单个指定字段的对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        public TResult Find<TResult>(Expression<Func<TEntity, bool>> condition)
        {
            return _mapper.ProjectTo<TResult>(DbContext.Set<TEntity>().Where(condition)).ToList().FirstOrDefault();
        }

        /// <summary>
        /// 查询单个指定字段的对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<TResult> FindAsync<TResult>(Expression<Func<TEntity, bool>> condition)
        {
            return (await _mapper.ProjectTo<TResult>(DbContext.Set<TEntity>().Where(condition)).ToListAsync()).FirstOrDefault();
        }

        /// <summary>
        /// 根据表达式直接删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="condition"></param>
        public void RemoveDirect(Expression<Func<TEntity, bool>> condition)
        {
            DbContext.Set<TEntity>().Where(condition).DeleteFromQuery();
        }

        /// <summary>
        /// 根据表达式直接删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="condition"></param>
        public async Task RemoveDirectAsync(Expression<Func<TEntity, bool>> condition)
        {
            await DbContext.Set<TEntity>().Where(condition).DeleteFromQueryAsync();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="filterAction"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<TEntity> GetPageList(Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, PageRequest pagination)
        {
            var context = DbContext;
            var query = context.Set<TEntity>() as IQueryable<TEntity>;
            if (query == null)
                throw new Exception($"{typeof(TEntity)}对象query为空！");
            if (filterAction != null)
                query = filterAction(query);
            pagination.Total = query.Count();
            var pageIndex = pagination.PageIndex;
            var pageSize = pagination.PageNumber;
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            pagination.SortField = pagination.SortField.IsNullOrWhiteSpace() ? "CreationTime" : pagination.SortField;
            query = OrderBy(query, pagination);
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="filterAction"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetPageListAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, PageRequest pagination)
        {
            var context = DbContext;
            var query = context.Set<TEntity>() as IQueryable<TEntity>;
            if (query == null)
                throw new Exception($"{typeof(TEntity)}对象query为空！");
            if (filterAction != null)
                query = filterAction(query);
            pagination.Total = await query.CountAsync();
            var pageIndex = pagination.PageIndex;
            var pageSize = pagination.PageNumber;
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            pagination.SortField = pagination.SortField.IsNullOrWhiteSpace() ? "Id" : pagination.SortField;
            query = OrderBy(query, pagination);
            return await query.AsNoTracking().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        /// <summary>
        /// 分页、关联查询指定字段对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="includeExpression"></param>
        /// <param name="filterAction"></param>
        /// <param name="selector"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<TResult> GetPageListByInclude<TResult>(Expression<Func<TEntity, object>> includeExpression, Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, Expression<Func<TEntity, TResult>> selector, PageRequest pagination)
        {
            if (includeExpression == null)
                throw new Exception($"{typeof(TEntity)}对象includeExpression为空！");
            var query = WithDetails(includeExpression);
            if (query == null)
                throw new Exception($"{typeof(TEntity)}对象query为空！");
            if (filterAction != null)
                query = filterAction(query);
            pagination.Total = query.Count();
            var pageIndex = pagination.PageIndex;
            var pageSize = pagination.PageNumber;
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            pagination.SortField = pagination.SortField.IsNullOrWhiteSpace() ? "CreationTime" : pagination.SortField;
            query = OrderBy(query, pagination);
            if (selector == null)
            {
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking();
                return _mapper.ProjectTo<TResult>(query).ToList();
            }
            else
            {
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().Select(selector).ToList();
            }
                
        }

        /// <summary>
        /// 分页、关联查询指定字段对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="includeExpression"></param>
        /// <param name="filterAction"></param>
        /// <param name="selector"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<List<TResult>> GetPageListByIncludeAsync<TResult>(Expression<Func<TEntity, object>> includeExpression, Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, Expression<Func<TEntity, TResult>> selector, PageRequest pagination)
        {
            if (includeExpression == null)
                throw new Exception($"{typeof(TEntity)}对象includeExpression为空！");
            var query = WithDetails(includeExpression);
            if (query == null)
                throw new Exception($"{typeof(TEntity)}对象query为空！");
            if (filterAction != null)
                query = filterAction(query);
            pagination.Total = await query.CountAsync();
            var pageIndex = pagination.PageIndex;
            var pageSize = pagination.PageNumber;
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            pagination.SortField = pagination.SortField.IsNullOrWhiteSpace() ? "CreationTime" : pagination.SortField;
            query = OrderBy(query, pagination);
            if (selector == null)
            {
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking();
                return await _mapper.ProjectTo<TResult>(query).ToListAsync();
            }
            else
            {
                return await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().Select(selector).ToListAsync();
            }
                
        }

        #region 分页查询方法

        public List<TEntity> GetPageList(int pageSize, int pageIndex, out long total,
         Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction)
        {
            var query = DbContext.Set<TEntity>() as IQueryable<TEntity>;
            if (query == null)
                throw new Exception($"{typeof(TEntity)}对象query为空！");
            query = filterAction(query);
            total = query.Count();
            if (pageIndex == 0)
            {
                pageIndex = 1;

            }


            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
        }


        public List<TEntity> GetPageList<TOrderKey>(int pageSize, int pageIndex, out long total,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, bool isAsc, Expression<Func<TEntity, TOrderKey>> orderByLambda)
        {
            var query = DbContext.Set<TEntity>() as IQueryable<TEntity>;
            if (query == null)
                throw new Exception($"{typeof(TEntity)}对象query为空！");
            query = filterAction(query);
            total = query.Count();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (isAsc)
            {
                return query.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
            }
            return query.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
        }
        public List<TResult> GetPageList<TOrderKey, TResult>(int pageSize, int pageIndex, out long total,
         Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, bool isAsc, Expression<Func<TEntity, TOrderKey>> orderByLambda)
        {
            var query = DbContext.Set<TEntity>() as IQueryable<TEntity>;
            if (query == null)
                throw new Exception($"{typeof(TEntity)}对象query为空！");
            query = filterAction(query);
            total = query.Count();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (isAsc)
            {
                query = query.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking();
                return _mapper.ProjectTo<TResult>(query).ToList();
            }

            query = query.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .AsNoTracking();
            return _mapper.ProjectTo<TResult>(query).ToList();
        }


        /// <summary>
        /// 多条件查询，不分页
        /// </summary>
        /// <typeparam name="TOrderKey"></typeparam>
        /// <param name="filterAction"></param>
        /// <param name="isAsc"></param>
        /// <param name="orderByLambda"></param>
        /// <returns></returns>
        public List<TEntity> GetPageListNoPage<TOrderKey>(Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, bool isAsc, Expression<Func<TEntity, TOrderKey>> orderByLambda)
        {
            var query = DbContext.Set<TEntity>() as IQueryable<TEntity>;
            if (query == null)
                throw new Exception($"{typeof(TEntity)}对象query为空！");
            query = filterAction(query);
            if (isAsc)
            {
                return query.OrderBy(orderByLambda).ToList();
            }
            return query.OrderByDescending(orderByLambda).ToList();
        }
        /// <summary>
        /// 多条件查询，不分页
        /// </summary>
        /// <typeparam name="TOrderKey"></typeparam>
        /// <param name="filterAction"></param>
        /// <param name="isAsc"></param>
        /// <param name="orderByLambda"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetPageListNoPageAsync<TOrderKey>(Func<IQueryable<TEntity>, IQueryable<TEntity>> filterAction, bool isAsc, Expression<Func<TEntity, TOrderKey>> orderByLambda)
        {
            var query = DbContext.Set<TEntity>() as IQueryable<TEntity>;
            if (query == null)
                throw new Exception($"{typeof(TEntity)}对象query为空！");
            query = filterAction(query);
            if (isAsc)
            {
                return await query.OrderBy(orderByLambda).ToListAsync();
            }
            return await query.OrderByDescending(orderByLambda).ToListAsync();
        }

        #endregion

        /// <summary>
        /// 新增实体列表
        /// </summary>
        /// <param name="entities">新增实体列表</param>
        /// <param name="isCommit">是否提交保存到数据库</param>
        public bool Add(TEntity[] entities, bool isCommit = false)
        {
            entities.ToList().ForEach(m =>
            {
                var obj = Insert(m);
            });
            if (isCommit)
            {
                return DbContext.SaveChanges() > 0;
            }
            return true;
            //var result =  GetDbContext().SaveChanges();
            //return result > 0;
        }

        /// <summary>
        /// 批量新增实体列表
        /// </summary>
        /// <param name="entities">新增实体列表</param>
        /// <param name="isCommit">是否提交保存到数据库</param>
        public async Task<bool> AddAsync(TEntity[] entities, bool isCommit = false)
        {
            entities.ToList().ForEach(m =>
            {
                var obj = Insert(m);
            });
            if (isCommit)
            {
                return (await DbContext.SaveChangesAsync()) > 0;
            }
            return true;
            //var result =  GetDbContext().SaveChanges();
            //return result > 0;
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        /// <param name="isCommit">是否提交保存到数据库</param>
        public bool Add(TEntity entities, bool isCommit = false)
        {
            Insert(entities);
            if (isCommit)
            {
                return DbContext.SaveChanges() > 0;
            }
            return true;
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        /// <param name="isCommit">是否提交保存到数据库</param>
        public async Task<bool> AddAsync(TEntity entities, bool isCommit = false)
        {
            Insert(entities);
            if (isCommit)
            {
                return (await DbContext.SaveChangesAsync()) > 0;
            }
            return true;
        }

        /// <summary>
        /// BulkInsert方式批量写入实体列表(自动提交事务)
        /// </summary>
        /// <param name="entities">新增实体列表</param>
        public void BulkInsert(TEntity[] entities)
        {
            var context = DbSet.GetContext();
            context.BulkInsert(entities);
        }

        public async Task BulkInsertAsync(TEntity[] entities)
        {
            throw new NotImplementedException();
        }

        public void UpdateDirect(TEntity entity, Expression<Func<TEntity, object>> updatedProperties = null, bool isCommit = false)
        {
            throw new NotImplementedException();
        }

        public void Update(TPrimaryKey id, Action<TEntity> updateAction, bool isCommit = false)
        {
            throw new NotImplementedException();
        }

        public void BulkUpdate(TEntity[] entities, Action<BulkOperation<TEntity>> bulkOperationFactory)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(TEntity[] entitys, Action<TEntity> updateAction, bool isCommit = false)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCommit(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCommitAsync(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entitie, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity[] entities, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCommit(TEntity[] entities, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCommitAsync(TEntity[] entities, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(TPrimaryKey id, Action<TEntity> updateAction, bool isCommit = false)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(TEntity[] entities, bool isCommit = false)
        {
            throw new NotImplementedException();
        }

        public void UpdateDirect(TEntity[] entitys, Expression<Func<TEntity, object>> updatedProperties = null, bool isCommit = false)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateDirectAsync(TEntity[] entitys, Expression<Func<TEntity, object>> updatedProperties = null, bool isCommit = false)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateDirectAsync(TEntity entity, Expression<Func<TEntity, object>> updatedProperties = null, bool isCommit = false)
        {
            throw new NotImplementedException();
        }

        public void UpdateFromQuery(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateFormQueryAsync(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            throw new NotImplementedException();
        }

        private IQueryable<TEntity> OrderBy(IQueryable<TEntity> source, PageRequest pagination)
        {
            if (!pagination.SortField.IsNullOrWhiteSpace())
            {
                var param = Expression.Parameter(typeof(TEntity), "p");
                var property = typeof(TEntity).GetProperty(pagination.SortField);
                var propertyAccessExpression = Expression.MakeMemberAccess(param, property ?? throw new Exception($"{pagination.SortField}属性不存在"));
                var le = Expression.Lambda(propertyAccessExpression, param);
                var type = typeof(TEntity);
                var orderByStr = pagination.Sort == "asc" ? "OrderBy" : "OrderByDescending";
                var resultExp = Expression.Call(typeof(Queryable), orderByStr, new[] { type, property.PropertyType }, source.Expression, Expression.Quote(le));
                return source.Provider.CreateQuery<TEntity>(resultExp);
            }
            return source;
        }

    }
}