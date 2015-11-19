using Dotnet.Samples.FullStack.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Dotnet.Samples.FullStack.Services.Mocks
{
    public class BookServiceMock : IBookService
    {
        // TODO: Provide a fully functional implementation

        private static readonly int QUANTITY = 20;
        private List<Book> books = BookStub.CreateListOfSize(QUANTITY);

        public void Create(Book book)
        {
            return;
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

        public void Delete(Book book)
        {
            return;
        }
    }
}
