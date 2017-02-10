using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Dotnet.Samples.FullStack.Data
{
    /// <remarks>
    /// TODO: Refactor this
    /// http://rob.conery.io/2014/03/04/repositories-and-unitofwork-are-not-a-good-idea/
    /// </remarks>
    /// <typeparam name="TEntity">The Type</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        void Create(TEntity entity);

        IList<TEntity> Retrieve();

        IList<TEntity> Retrieve(Expression<Func<TEntity, bool>> predicate);

        TEntity Retrieve(params object[] keyValues);

        void Update(TEntity entity);

        void Delete(params object[] keyValues);

        int SaveChanges();
    }
}
