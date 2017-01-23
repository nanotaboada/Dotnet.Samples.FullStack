using Dotnet.Samples.FullStack.Data;
using System.Collections.Generic;

namespace Dotnet.Samples.FullStack.Services
{
    public class BookService : IBookService
    {
        private IRepository<Book> bookRepository;
        private static readonly string BooksService_RetrieveAllInStock_CacheKey = "BookService_RetrieveAllInStock_CacheKey";

        public BookService(IRepository<Book> bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        #region Create

        public int Create(Book book)
        {
            this.bookRepository.Insert(book);
            return this.bookRepository.SaveChanges();
        }

        #endregion

        #region Retrieve

        public Book RetrieveByIsbn(string isbn)
        {
            // TODO: Discuss this: ISBN is a natural Key of the Book domain but
            // SqlServer favors int over string values for PKs so we might have
            // to implement a Guid as PK approach in the future to avoid issues
            return this.bookRepository.GetById(isbn);
        }

        public List<Book> RetrieveAll()
        {
            return this.bookRepository.Get() as List<Book>;
        }

        public List<Book> RetrieveAllInStock()
        {
            return this.bookRepository.Get(book => book.InStock == true, null, null) as List<Book>;
        }

        public List<Book> RetrieveAllInStockToday()
        {
            var books = new List<Book>();
            var cache = ObjectCacheService.Get<List<Book>>(BooksService_RetrieveAllInStock_CacheKey);

            if (cache != null)
            {
                books = cache;
            }
            else
            {
                books = this.bookRepository.Get(book => book.InStock == true, null, null) as List<Book>;
                ObjectCacheService.Add(books, BooksService_RetrieveAllInStock_CacheKey);
            }

            return books;
        }

        #endregion

        #region Update

        public void Update(Book book)
        {
            var current = this.bookRepository.GetById(book.Isbn);

            if (current != null)
            {
                current = book;
                this.bookRepository.SaveChanges();
            }
        }

        #endregion

        #region Delete

        public void Delete(string isbn)
        {
            this.bookRepository.Delete(isbn);
            this.bookRepository.SaveChanges();
        }

        #endregion

        #region Validation

        public bool IsValidIsbn(string isbn)
        {
            // TODO: Improve this implementation with real ISBN validation.
            return !string.IsNullOrWhiteSpace(isbn);
        }

        #endregion

    }
}
