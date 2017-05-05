using Dotnet.Samples.FullStack.Data;
using System.Collections.Generic;

namespace Dotnet.Samples.FullStack.Services
{
    // TODO: Define more illustrative/granular methods, e.g. RetrievePublishedAfter(DateTime date)
    public interface IBookService
    {
        // Create
        int Create(Book book);

        // Retrieve
        Book RetrieveByIsbn(string isbn);
        List<Book> RetrieveAll();
        List<Book> RetrieveAllInStock();

        // Update
        void Update(Book book);

        // Delete
        void Delete(string isbn);

        // Validation
        bool IsValidIsbn(string isbn);
    }
}
