using Dotnet.Samples.FullStack.Data;
using Dotnet.Samples.FullStack.Services;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace Dotnet.Samples.FullStack.Api.Controllers
{
    /// <summary>
    /// ASP.NET Web API 2 Controller for Books.
    /// </summary>
    /// <remarks>
    /// http://www.restapitutorial.com/lessons/httpmethods.html
    /// https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html
    /// </remarks>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BookController : ApiController
    {
        private IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        /// <summary>
        /// The GET method means retrieve whatever information
        /// (in the form of an entity) is identified by the Request-URI.
        /// </summary>
        /// <returns>
        /// A collection of Books and status code 200 (OK),
        /// or status code 204 (No Content).
        /// </returns>
        [Route("api/v1/books")]
        public IHttpActionResult Get()
        {
            var books = this.bookService.RetrieveAll();

            if (books == null)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            else
            {
                return Ok(books);
            }

        }

        /// <summary>
        /// The POST method is used to request that the origin server accept
        /// the entity enclosed in the request as a new subordinate of the
        /// resource identified by the Request-URI in the Request-Line.
        /// </summary>
        /// <param name="book">A Book.</param>
        /// <returns>
        /// A Location header with link to new Book and status code 
        /// 201 (Created), or status code 409 (Conflict) if the Book already
        /// exists, or status code 400 (Bad Request) if the ISBN is invalid.
        /// </returns>
        [Route("api/v1/books")]
        public IHttpActionResult Post([FromBody]Book book)
        {
            if (!this.bookService.IsValidIsbn(book.Isbn))
            {
                return BadRequest();
            }
            else
            {
                var existing = this.bookService.RetrieveByIsbn(book.Isbn);

                if (existing != null)
                {
                    return Conflict();
                }
                else
                {
                    this.bookService.Create(book);
                    var location = string.Format("/book/{0}", book.Isbn);
                    return Created<Book>(location, book);

                }
            }

        }

        /// <summary>
        /// The GET method means retrieve whatever information
        /// (in the form of an entity) is identified by the Request-URI.
        /// </summary>
        /// <param name="isbn">The ISBN of a Book.</param>
        /// <returns>
        /// A Book matching the provided ISBN and status code 200 (OK),
        /// or 404 (Not Found) if the ISBN was not found or is invalid.
        /// </returns>
        [Route("api/v1/book/{isbn}")]
        public IHttpActionResult Get(string isbn)
        {
            var book = this.bookService.RetrieveByIsbn(isbn);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }

        /// <summary>
        /// The PUT method requests that the enclosed entity be stored under 
        /// the supplied Request-URI. If the Request-URI refers to an already
        /// existing resource, the enclosed entity SHOULD be considered as a
        /// modified version of the one residing on the origin server. If the
        /// resource could not be created or modified with the Request-URI,
        /// an appropriate error response SHOULD be given that reflects the
        /// nature of the problem. 
        /// </summary>
        /// <param name="isbn">The ISBN of a Book.</param>
        /// <param name="book">A Book.</param>
        /// <returns>
        /// A status code 204 (No Content) if the Books is successfully updated,
        /// or 404 (Not Found) if the ISBN was not found,
        /// or 400 (Bad Request) if the ISBN was invalid.
        /// </returns>
        [Route("api/v1/book/{isbn}")]
        public IHttpActionResult Put(string isbn, [FromBody]Book book)
        {
            if (!this.bookService.IsValidIsbn(isbn)
                || !this.bookService.IsValidIsbn(book.Isbn)
                || (isbn != book.Isbn))
            {
                return BadRequest();
            }
            else
            {
                var existing = this.bookService.RetrieveByIsbn(isbn);

                if (existing == null)
                {
                    return NotFound();
                }
                else
                {
                    this.bookService.Update(book);
                    return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.NoContent));
                }
            }


        }

        /// <summary>
        /// The DELETE method requests that the origin server delete the 
        /// resource identified by the Request-URI.This method MAY be 
        /// overridden by human intervention(or other means) on the origin 
        /// server.The client cannot be guaranteed that the operation has been
        /// carried out, even if the status code returned from the origin server
        /// indicates that the action has been completed successfully. However,
        /// the server SHOULD NOT indicate success unless, at the time the
        /// response is given, it intends to delete the resource or move it to
        /// an inaccessible location.
        /// </summary>
        /// <param name="isbn">The ISBN of a Book.</param>
        [Route("api/v1/book/{isbn}")]
        public IHttpActionResult Delete(string isbn)
        {
            if (!this.bookService.IsValidIsbn(isbn))
            {
                return NotFound();
            }
            else
            {
                this.bookService.Delete(isbn);
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.NoContent));
            }

        }
    }
}
