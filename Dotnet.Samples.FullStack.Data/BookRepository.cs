using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Transactions;


namespace Dotnet.Samples.FullStack.Data
{
    public class BookRepository : IRepository<Book>
    {
        protected readonly DbContext Context;
        protected readonly DbSet<Book> Set;

        public BookRepository(DbContext context)
        {
            this.Context = context;
            this.Set = this.Context.Set<Book>();
        }

        public virtual IQueryable<Book> GetQueryable()
        {
            return Context.Set<Book>().AsQueryable();
        }

        public virtual IList<Book> Get(
            Expression<Func<Book, bool>> filter = null,
            Func<IQueryable<Book>, IOrderedQueryable<Book>> orderBy = null,
            params Expression<Func<Book, object>>[] includes)
        {
            return GetQuery(filter, orderBy, includes).ToList();
        }

        public PagedQueryResult<Book> GetPaged(
            int pageNumber,
            int pageSize,
            Expression<Func<Book, bool>> filter = null,
            Func<IQueryable<Book>, IOrderedQueryable<Book>> orderBy = null,
            params Expression<Func<Book, object>>[] includes)
        {
            IQueryable<Book> query = this.GetQuery(filter, orderBy, includes);

            long count = query.LongCount();
            int totalPages = (int)Math.Ceiling((0D + count) / pageSize);

            query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            return new PagedQueryResult<Book>()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItems = count,
                TotalPages = totalPages,

                Items = query.ToList()
            };
        }

        public PagedQueryResult<Book> GetPaged(
            int pageNumber,
            int pageSize,
            string filter,
            Func<IQueryable<Book>, IOrderedQueryable<Book>> orderBy = null,
            params Expression<Func<Book, object>>[] includes)
        {
            IQueryable<Book> query = this.GetQuery(filter, orderBy, includes);

            long count = query.LongCount();
            int totalPages = (int)Math.Ceiling((0D + count) / pageSize);

            if (orderBy != null)
            {
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }

            return new PagedQueryResult<Book>()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItems = count,
                TotalPages = totalPages,

                Items = query.ToList()
            };
        }

        protected virtual IQueryable<Book> GetQuery(
            string filter,
            Func<IQueryable<Book>,
            IOrderedQueryable<Book>> orderBy = null,
            params Expression<Func<Book, object>>[] includes)
        {
            IQueryable<Book> query = this.Set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        protected virtual IQueryable<Book> GetQuery(
            Expression<Func<Book, bool>> filter = null,
            Func<IQueryable<Book>, IOrderedQueryable<Book>> orderBy = null,
            params Expression<Func<Book, object>>[] includes)
        {
            IQueryable<Book> query = this.Set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual Book GetById(params object[] keyValues)
        {
            return this.Set.Find(keyValues);
        }

        public virtual void Insert(Book entity)
        {
            this.Set.Add(entity);
        }

        public virtual void Delete(params object[] keyValues)
        {
            Book entity = this.Set.Find(keyValues);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public virtual void Delete(Book entity)
        {
            this.Set.Remove(entity);
        }

        public void SaveChangesWithScope(TransactionScope scope)
        {
            using (scope)
            {
                this.Context.SaveChanges();
            }
        }

        public virtual void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}
