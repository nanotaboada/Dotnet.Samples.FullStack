using Dotnet.Samples.FullStack.Services;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Dotnet.Samples.FullStack.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BookController : ApiController
    {
        private IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        // GET

        public IHttpActionResult Get()
        {
            var books = bookService.RetrieveAll();

            if (books.Count > 0)
            {
                return Ok(books);
            }
            else
            {
                return NotFound();
            }

        }

        public IHttpActionResult Get(string isbn)
        {
            if (!string.IsNullOrWhiteSpace(isbn))
            {
                var book = bookService.RetrieveByIsbn(isbn);

                if (book != null)
                {
                    return Ok(book);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }

        }

        // POST
        public IHttpActionResult Post([FromBody]string value)
        {
            return InternalServerError();
        }

        // PUT
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return InternalServerError();
        }

        // DELETE
        public IHttpActionResult Delete(int id)
        {
            return InternalServerError();
        }
    }
}
