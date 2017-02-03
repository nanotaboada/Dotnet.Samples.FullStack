using Dotnet.Samples.FullStack.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            this.bookRepository.Create(book);
            return this.bookRepository.SaveChanges();
        }

        #endregion

        #region Retrieve

        public Book RetrieveByIsbn(string isbn)
        {
            // TODO: Discuss this: ISBN is a natural Key of the Book domain but
            // SqlServer favors int over string values for PKs so we might have
            // to implement a Guid as PK approach in the future to avoid issues
            return this.bookRepository.Retrieve(isbn);
        }

        public List<Book> RetrieveAll()
        {
            return this.bookRepository.Retrieve() as List<Book>;
        }

        public List<Book> RetrieveAllInStock()
        {
            return this.bookRepository.Retrieve(book => book.InStock == true) as List<Book>;
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
                books = this.bookRepository.Retrieve(book => book.InStock == true) as List<Book>;
                ObjectCacheService.Add(books, BooksService_RetrieveAllInStock_CacheKey);
            }

            return books;
        }

        #endregion

        #region Update

        public void Update(Book book)
        {
            var current = this.bookRepository.Retrieve(book.Isbn);

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
            // https://www.safaribooksonline.com/library/view/regular-expressions-cookbook/9781449327453/ch04s13.html
            var pattern = @"
                ^
                (?:ISBN(?:-1[03])?:?\ )?    # Optional ISBN/ISBN-10/ISBN-13 identifier.
                (?=                         # Basic format pre-checks (lookahead):
                [0-9X]{10}$                 # Require 10 digits/Xs (no separators).
                |                           # Or:
                (?=(?:[0-9]+[-\ ]){3})      # Require 3 separators
                [-\ 0-9X]{13}$              # out of 13 characters total.
                |                           # Or:
                97[89][0-9]{10}$            # 978/979 plus 10 digits (13 total).
                |                           # Or:
                (?=(?:[0-9]+[-\ ]){4})      # Require 4 separators
                [-\ 0-9]{17}$               # out of 17 characters total.
                )                           # End format pre-checks.
                (?:97[89][-\ ]?)?           # Optional ISBN-13 prefix.
                [0-9]{1,5}[-\ ]?            # 1-5 digit group identifier.
                [0-9]+[-\ ]?[0-9]+[-\ ]?    # Publisher and title identifiers.
                [0-9X]                      # Check digit.
                $
                ";
            var regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);

            return regex.IsMatch(isbn);
        }

        #endregion
    }
}
