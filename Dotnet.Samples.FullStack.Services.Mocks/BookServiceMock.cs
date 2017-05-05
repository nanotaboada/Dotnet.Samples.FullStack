using Dotnet.Samples.FullStack.Data;
using Dotnet.Samples.FullStack.Data.Stubs;
using System.Collections.Generic;
using System.Linq;

namespace Dotnet.Samples.FullStack.Services.Mocks
{
    public class BookServiceMock : IBookService
    {
        private static readonly int QUANTITY = 20;
        private List<Book> books = BookStub.CreateListOfSize(QUANTITY);

        public int Create(Book book)
        {
            return 1;
        }

        public List<Book> RetrieveAll()
        {
            return books;
        }

        public List<Book> RetrieveAllInStock()
        {
            return books.Where(book => book.InStock == true) as List<Book>;
        }

        public Book RetrieveByIsbn(string isbn)
        {
            return books.Where(book => book.Isbn == isbn).SingleOrDefault();
        }

        public void Update(Book book)
        {
            return;
        }

        public void Delete(string isbn)
        {
            return;
        }

        public bool IsValidIsbn(string isbn)
        {
            // TODO: Improve this implementation with real ISBN validation.
            return true;
        }
    }
}
