using Dotnet.Samples.FullStack.Data.Stubs;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;

namespace Dotnet.Samples.FullStack.Data.Tests
{
    // TODO: Improve Code Coverage
    [TestClass]
    public class BookRepositoryTests
    {
        [TestMethod]
        public void GivenCreate_WhenInvokedWithBook_ThenShouldAddBookToTheDbSet()
        {
            // Arrange
            var book = BookStub.CreateNew();
            var set = new Mock<DbSet<Book>>();
            var context = new Mock<DbContext>();
            context.Setup(c => c.Set<Book>()).Returns(set.Object);
            var repository = new BookRepository(context.Object);

            // Act
            repository.Create(book);

            // Assert
            set.Verify(s => s.Add(It.IsAny<Book>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GivenRetrieve_WhenInvokedWithIsbn_ThenShouldReturnAMatchingBook()
        {
            // Arrange
            var book = BookStub.CreateNew();
            var set = new Mock<DbSet<Book>>();
            set.Setup(s => s.Find(book.Isbn)).Returns(book);
            var context = new Mock<DbContext>();
            context.Setup(c => c.Set<Book>()).Returns(set.Object);

            var repository = new BookRepository(context.Object);

            // Act
            var result = repository.Retrieve(book.Isbn);

            // Assert
            set.Verify(s => s.Find(It.IsAny<object[]>()), Times.Exactly(1));
            result.ShouldBeEquivalentTo(book);
        }

    }
}
