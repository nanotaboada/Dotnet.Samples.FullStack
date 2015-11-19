using Dotnet.Samples.FullStack.Data;
using System.Collections.Generic;

namespace Dotnet.Samples.FullStack.Services
{
    // TODO: Define more illustrative/granular methods, e.g. RetrievePublishedAfter(DateTime date)
    public interface IBookService
    {
        // Create
        void Create(Book book);

        // Retrieve
        Book RetrieveByIsbn(string isbn);
        List<Book> RetrieveAll();
        List<Book> RetrieveAllInStock();

        // Update
        void Update(Book book);

        // Delete
        void Delete(Book book);
    }
}
