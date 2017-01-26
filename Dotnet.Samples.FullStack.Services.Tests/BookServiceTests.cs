using Dotnet.Samples.FullStack.Data;
using Dotnet.Samples.FullStack.Services.Stubs;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Dotnet.Samples.FullStack.Services.Tests
{
    [TestClass]
    public class BookServiceTests
    {
        private static readonly int SIZE = 10;
        private static readonly bool IN_STOCK = true;

        #region Create

        [TestMethod]
        public void GivenCreate_WhenInvoked_ThenShouldInsertBookInRepository()
        {
            // Arrange
            var repository = new Mock<IRepository<Book>>();

            var service = new BookService(repository.Object);

            // Act
            service.Create(It.IsAny<Book>());

            // Assert
            repository.Verify(r => r.Insert(It.IsAny<Book>()), Times.Exactly(1));
            repository.Verify(r => r.SaveChanges(), Times.Exactly(1));
        }

        #endregion

        #region Retrieve

        [TestMethod]
        public void GivenRetrieveByIsbn_WhenInvoked_ThenShouldReturnMatchingBook()
        {
            // Arrange
            var book = BookStub.CreateNew();

            var repository = new Mock<IRepository<Book>>();
            repository.Setup(r => r.GetById(book.Isbn))
                .Returns(book);

            var service = new BookService(repository.Object);

            // Act
            var result = service.RetrieveByIsbn(book.Isbn);

            // Assert
            result.ShouldBeEquivalentTo(book);
            repository.Verify(r => r.GetById(book.Isbn), Times.Exactly(1));
        }

        [TestMethod]
        public void GivenRetrieveAll_WhenInvoked_ThenShouldReturnAllBooksInRepository()
        {
            // Arrange
            var books = BookStub.CreateListOfSize(SIZE);

            var repository = new Mock<IRepository<Book>>();
            repository.Setup(r => r.Get(null, null))
                .Returns(books);

            var service = new BookService(repository.Object);

            // Act
            var results = service.RetrieveAll();

            // Assert
            results.ShouldAllBeEquivalentTo(books);
            repository.Verify(r => r.Get(null, null), Times.Exactly(1));
        }

        [TestMethod]
        public void GivenRetrieveAllInStock_WhenInvoked_ThenShouldReturnAllBooksInStock()
        {
            // Arrange
            var books = BookStub.CreateListOfSize(SIZE, IN_STOCK);

            var repository = new Mock<IRepository<Book>>();
            repository.Setup(r => r.Get(book => book.InStock == true, null, null))
                .Returns(books);

            var service = new BookService(repository.Object);

            // Act
            var results = service.RetrieveAllInStock();

            // Assert
            results.ShouldAllBeEquivalentTo(books);
            repository.Verify(r => r.Get(book => book.InStock == true, null, null), Times.Exactly(1));
        }

        [TestMethod]
        public void GivenRetrieveAllInStockToday_WhenInvokedTwice_ThenShouldReturnAllBooksInCache()
        {
            // Arrange
            var books = BookStub.CreateListOfSize(SIZE, IN_STOCK);

            var repository = new Mock<IRepository<Book>>();
            repository.Setup(r => r.Get(book => book.InStock == true, null, null))
                .Returns(books);

            var service = new BookService(repository.Object);

            // Act
            var results = service.RetrieveAllInStockToday();
            var cachedResults = service.RetrieveAllInStockToday();

            // Assert
            cachedResults.ShouldAllBeEquivalentTo(results);
            repository.Verify(r => r.Get(book => book.InStock == true, null, null), Times.Exactly(1));
        }

        #endregion

        #region Update

        [TestMethod]
        public void GivenUpdate_WhenInvoked_ThenShouldUpdateBookInRepository()
        {
            // Arrange
            var book = BookStub.CreateNew();

            var repository = new Mock<IRepository<Book>>();
            repository.Setup(r => r.GetById(book.Isbn))
                .Returns(book);

            var service = new BookService(repository.Object);

            // Act
            service.Update(book);

            // Assert
            repository.Verify(r => r.SaveChanges(), Times.Exactly(1));
        }

        #endregion

        #region Delete

        [TestMethod]
        public void GivenDelete_WhenInvoked_ThenShouldDeleteBookInRepository()
        {
            // Arrange
            var repository = new Mock<IRepository<Book>>();

            var service = new BookService(repository.Object);

            // Act
            service.Delete(It.IsAny<string>());

            // Assert
            repository.Verify(r => r.Delete(It.IsAny<string>()), Times.Exactly(1));
            repository.Verify(r => r.SaveChanges(), Times.Exactly(1));
        }

        #endregion
    }
}
