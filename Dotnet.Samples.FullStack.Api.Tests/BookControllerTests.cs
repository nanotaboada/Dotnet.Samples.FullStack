using Dotnet.Samples.FullStack.Api.Controllers;
using Dotnet.Samples.FullStack.Data;
using Dotnet.Samples.FullStack.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Results;

namespace Dotnet.Samples.FullStack.Api.Tests
{
    [TestClass]
    public class BookControllerTests
    {
        private static readonly int SIZE = 10;

        #region /books

        #region GET

        [TestMethod]
        public void GivenHttpGetVerb_WhenRequestHasNoParameterAndServiceHasBooks_ThenShouldReturnAListOfBooksAndStatusOk()
        {
            // Arrange
            var books = BookStub.CreateListOfSize(SIZE);
            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveAll()).Returns(books);
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Get() as OkNegotiatedContentResult<List<Book>>;

            // Assert
            service.Verify(s => s.RetrieveAll(), Times.Exactly(1));
            result.Should().BeOfType<OkNegotiatedContentResult<List<Book>>>();
            result.Content.ShouldAllBeEquivalentTo(books);
        }

        [TestMethod]
        public void GivenHttpGetVerb_WhenRequestHasNoParameterAndServiceHasNoBooks_ThenShouldReturnStatusNoContent()
        {
            // Arrange
            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(It.IsAny<string>()));
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Get() as ResponseMessageResult;

            // Assert
            service.Verify(s => s.RetrieveAll(), Times.Exactly(1));
            result.Should().BeOfType<ResponseMessageResult>();
            result.Response.StatusCode.ShouldBeEquivalentTo(204);
        }

        #endregion

        #region POST

        [TestMethod]
        public void GivenHttpPostVerb_WhenRequestBodyIsBookWithInvalidIsbn_ThenShouldReturnStatusBadRequest()
        {
            // Arrange
            var book = BookStub.CreateNew();
            book.Isbn = null;
            var service = new Mock<IBookService>();
            service.Setup(s => s.Create(book));
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Post(book) as BadRequestResult;

            // Assert
            service.Verify(s => s.Create(It.IsAny<Book>()), Times.Never);
            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void GivenHttpPostVerb_WhenRequestBodyIsAlreadyExistingBook_ThenShouldReturnStatusConflict()
        {
            // Arrange
            var book = BookStub.CreateNew();
            var service = new Mock<IBookService>();
            service.Setup(s => s.IsValidIsbn13(book.Isbn)).Returns(true);
            service.Setup(s => s.RetrieveByIsbn(book.Isbn)).Returns(book);
            service.Setup(s => s.Create(book));
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Post(book) as ConflictResult;

            // Assert
            service.Verify(s => s.RetrieveByIsbn(It.IsAny<string>()), Times.Exactly(1));
            service.Verify(s => s.Create(It.IsAny<Book>()), Times.Never);
            result.Should().BeOfType<ConflictResult>();
        }

        [TestMethod]
        public void GivenHttpPostVerb_WhenRequestBodyHasValidBook_ThenShouldReturnStatusCreatedAndLocationHeader()
        {
            // Arrange
            var book = BookStub.CreateNew();
            var service = new Mock<IBookService>();
            service.Setup(s => s.IsValidIsbn13(book.Isbn)).Returns(true);
            service.Setup(s => s.RetrieveByIsbn(book.Isbn)).Returns(null as Book);
            service.Setup(s => s.Create(book)).Returns(1);
            var controller = new BookController(service.Object);
            var location = string.Format("/book/{0}", book.Isbn);

            // Act
            var result = controller.Post(book) as CreatedNegotiatedContentResult<Book>;

            // Assert
            service.Verify(s => s.RetrieveByIsbn(It.IsAny<string>()), Times.Exactly(1));
            service.Verify(s => s.Create(It.IsAny<Book>()), Times.Exactly(1));
            result.Should().BeOfType<CreatedNegotiatedContentResult<Book>>();
            result.Content.ShouldBeEquivalentTo(book);
            result.Location.ShouldBeEquivalentTo(location);
        }

        #endregion

        #endregion

        #region /book

        #region GET

        [TestMethod]
        public void GivenHttpGetVerb_WhenRequestParameterDoesNotIdentifyAnExistingBook_ThenShouldReturnStatusNotFound()
        {
            // Arrange
            string isbn = null;
            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(isbn)).Returns(null as Book);
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Get(isbn);

            // Assert
            service.Verify(s => s.RetrieveByIsbn(isbn), Times.Exactly(1));
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void GivenHttpGetVerb_WhenRequestParameterIdentifiesAnExistingBook_ThenShouldReturnStatusOkAndTheBook()
        {
            // Arrange
            var book = BookStub.CreateNew();
            var service = new Mock<IBookService>();
            service.Setup(s => s.RetrieveByIsbn(book.Isbn)).Returns(book);
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Get(book.Isbn) as OkNegotiatedContentResult<Book>;

            // Assert
            service.Verify(s => s.RetrieveByIsbn(It.IsAny<string>()), Times.Exactly(1));
            result.Should().BeOfType<OkNegotiatedContentResult<Book>>();
            result.Content.ShouldBeEquivalentTo(book);
        }

        #endregion

        #region PUT

        [TestMethod]
        public void GivenHttpPutVerb_WhenRequestParameterIsInvalid_ThenShouldReturnStatusBadRequest()
        {
            // Arrange
            string isbn = null;
            var book = BookStub.CreateNew();
            var service = new Mock<IBookService>();
            service.Setup(s => s.IsValidIsbn13(isbn)).Returns(false);
            service.Setup(s => s.Update(book));
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Put(isbn, book) as BadRequestResult;

            // Assert
            service.Verify(s => s.Update(It.IsAny<Book>()), Times.Never);
            result.Should().BeOfType<BadRequestResult>();
        }


        [TestMethod]
        public void GivenHttpPutVerb_WhenRequestParameterIsValidIsbnButBodyHasBookWithInvalidIsbn_ThenShouldReturnStatusBadRequest()
        {
            // Arrange
            var book = BookStub.CreateNew();
            var isbn = book.Isbn;
            book.Isbn = null;
            var service = new Mock<IBookService>();
            service.Setup(s => s.IsValidIsbn13(isbn)).Returns(false);
            service.Setup(s => s.Update(book));
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Put(isbn, book) as BadRequestResult;

            // Assert
            service.Verify(s => s.Update(It.IsAny<Book>()), Times.Never);
            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void GivenHttpPutVerb_WhenRequestParameterIsValidIsbnAndBodyHasValidIsbnButAreNotEqual_ThenShouldReturnStatusBadRequest()
        {
            // Arrange
            var book = BookStub.CreateNew();
            var isbn = BookStub.CreateNew().Isbn;
            book.Isbn = null;
            var service = new Mock<IBookService>();
            service.Setup(s => s.IsValidIsbn13(isbn)).Returns(false);
            service.Setup(s => s.Update(book));
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Put(isbn, book) as BadRequestResult;

            // Assert
            service.Verify(s => s.Update(It.IsAny<Book>()), Times.Never);
            result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public void GivenHttpPutVerb_WhenBothIsbnsAreValidButDoNotIdentifyAnExistingBook_ThenShouldReturnStatusNotFound()
        {
            // Arrange
            var book = BookStub.CreateNew();
            var isbn = book.Isbn;
            var service = new Mock<IBookService>();
            service.Setup(s => s.IsValidIsbn13(isbn)).Returns(true);
            service.Setup(s => s.Update(book));
            service.Setup(s => s.RetrieveByIsbn(book.Isbn)).Returns(null as Book);
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Put(isbn, book) as NotFoundResult;

            // Assert
            service.Verify(s => s.Update(It.IsAny<Book>()), Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void GivenHttpPutVerb_WhenBothIsbnsAreValidAndIdentifyAnExistingBook_ThenShouldReturnStatusNoContent()
        {
            // Arrange
            var book = BookStub.CreateNew();
            var isbn = book.Isbn;
            var service = new Mock<IBookService>();
            service.Setup(s => s.IsValidIsbn13(isbn)).Returns(true);
            service.Setup(s => s.Update(book));
            service.Setup(s => s.RetrieveByIsbn(book.Isbn)).Returns(book);
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Put(isbn, book) as ResponseMessageResult;

            // Assert
            service.Verify(s => s.Update(It.IsAny<Book>()), Times.Exactly(1));
            result.Should().BeOfType<ResponseMessageResult>();
            result.Response.StatusCode.ShouldBeEquivalentTo(204);
        }

        #endregion

        #region DELETE

        [TestMethod]
        public void GivenHttpDeleteVerb_WhenRequestParameterIsInvalidIsbn_ThenShouldReturnStatusNotFound()
        {
            // Arrange
            string isbn = null;
            var service = new Mock<IBookService>();
            service.Setup(s => s.Delete(isbn));
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Delete(isbn);

            // Assert
            service.Verify(s => s.Delete(It.IsAny<string>()), Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void GivenHttpDeleteVerb_WhenRequestParameterIsValidIsbn_ThenShouldReturnStatusNoContent()
        {
            // Arrange
            var isbn = BookStub.CreateNew().Isbn;
            var service = new Mock<IBookService>();
            service.Setup(s => s.IsValidIsbn13(isbn)).Returns(true);
            service.Setup(s => s.Delete(isbn));
            var controller = new BookController(service.Object);

            // Act
            var result = controller.Delete(isbn) as ResponseMessageResult;

            // Assert
            service.Verify(s => s.Delete(It.IsAny<string>()), Times.Exactly(1));
            result.Should().BeOfType<ResponseMessageResult>();
            result.Response.StatusCode.ShouldBeEquivalentTo(204);
        }

        #endregion

        #endregion


    }
}
