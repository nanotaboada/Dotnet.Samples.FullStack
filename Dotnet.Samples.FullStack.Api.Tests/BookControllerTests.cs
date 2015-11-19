using Dotnet.Samples.FullStack.Api.Controllers;
using Dotnet.Samples.FullStack.Data;
using Dotnet.Samples.FullStack.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace Dotnet.Samples.FullStack.Api.Tests
{
    [TestClass]
    public class BookControllerTests
    {
        private static readonly int SIZE = 10;

        [TestMethod]
        public void GivenHttpGetVerb_WhenRequestHasNoParametersAndServiceHasBooks_ThenShouldReturnAllBooksAndStatusOk()
        {
            // Arrange
            var Books = BookStub.CreateListOfSize(SIZE);
            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveAll())
                .Returns(Books);
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Get();
            var response = result as OkNegotiatedContentResult<List<Book>>;

            // Assert
            service.Verify(s => s.RetrieveAll(), Times.Exactly(1));
            result.Should().BeOfType<OkNegotiatedContentResult<List<Book>>>();
            response.Content.ShouldAllBeEquivalentTo(Books);
        }

        [TestMethod]
        public void GivenHttpGetVerb_WhenRequestHasNoParametersAndServiceHasNoBooks_ThenShouldReturnStatusNotFound()
        {
            // Arrange
            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveAll())
                .Returns(new List<Book>());
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Get();

            // Assert
            service.Verify(s => s.RetrieveAll(), Times.Exactly(1));
            result.Should().BeOfType<NotFoundResult>();
        }


        [TestMethod]
        public void GivenHttpGetVerb_WhenRequestHasEmptyIsbnParameter_ThenShouldReturnStatusBadRequest()
        {
            // Arrange
            var isbn = string.Empty;
            var service = new Mock<IBookService>();
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Get(isbn);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void GivenHttpGetVerb_WhenRequestHasValidIsbnParameter_ThenShouldReturnTheBookAndStatusOk()
        {
            // Arrange
            var book = BookStub.CreateNew();
            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(book.Isbn))
                .Returns(book);
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Get(book.Isbn);
            var response = result as OkNegotiatedContentResult<Book>;

            // Assert
            service.Verify(s => s.RetrieveByIsbn(book.Isbn), Times.Exactly(1));
            result.Should().BeOfType<OkNegotiatedContentResult<Book>>();
            response.Content.ShouldBeEquivalentTo(book);
        }

        [TestMethod]
        public void GivenHttpGetVerb_WhenRequestHasInvalidIsbnParameter_ThenShouldReturnStatusNotFound()
        {
            // Arrange
            var isbn = "FOOBAR";
            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(isbn))
                .Returns(null as Book);
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Get(isbn);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

    }
}
