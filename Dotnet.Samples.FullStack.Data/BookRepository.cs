using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Transactions;


namespace Dotnet.Samples.FullStack.Data
{
    /// <remarks>
    /// TODO: Refactor this
    /// http://rob.conery.io/2014/03/04/repositories-and-unitofwork-are-not-a-good-idea/
    /// </remarks>
    public class BookRepository : IRepository<Book>
    {
        protected readonly DbContext Context; // Repository
        protected readonly DbSet<Book> Set; // Unit of Work

        public BookRepository(DbContext context)
        {
            this.Context = context;
            this.Set = this.Context.Set<Book>();
        }

        public virtual void Create(Book book)
        {
            this.Set.Add(book);
        }

        public virtual Book Retrieve(params object[] keyValues)
        {
            return this.Set.Find(keyValues);
        }

        public virtual IList<Book> Retrieve()
        {
            return this.Set as IList<Book>;
        }

        public IList<Book> Retrieve(Expression<Func<Book, bool>> predicate)
        {
            var query = this.Set as IQueryable<Book>;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query as IList<Book>;
        }

        public virtual void Update(Book book)
        {
            this.Context.Entry(book).State = EntityState.Modified;
        }

        public virtual void Delete(params object[] keyValues)
        {
            var entity = this.Set.Find(keyValues);

            if (entity != null)
            {
                this.Set.Remove(entity);
            }
        }

        public virtual int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

    }
}
