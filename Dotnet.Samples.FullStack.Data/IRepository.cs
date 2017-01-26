using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Dotnet.Samples.FullStack.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetQueryable();

        IList<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        PagedQueryResult<TEntity> GetPaged(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);


        PagedQueryResult<TEntity> GetPaged(
          int pageNumber,
          int pageSize,
          string filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          params Expression<Func<TEntity, object>>[] includes);

        TEntity GetById(params object[] keyValues);

        void Insert(TEntity entity);

        void Delete(params object[] keyValues);

        void Delete(TEntity entity);

        int SaveChanges();
    }
}
