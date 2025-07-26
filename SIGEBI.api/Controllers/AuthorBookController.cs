using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Dtos.BookAuthorDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SIGEBI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorBookController : ControllerBase
    {
        private readonly IAuthorBookService _authorBookService;

        public AuthorBookController(IAuthorBookService authorBookService)
        {
            _authorBookService = authorBookService;
        }

        // GET: api/AuthorBook
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _authorBookService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        // GET: api/AuthorBook/{bookId}/{authorId}
        [HttpGet("{bookId:int}/{authorId:int}")]
        public async Task<IActionResult> GetByIds(int bookId, int authorId)
        {
            var result = await _authorBookService.CheckDuplicateBookAuthorCombinationAsync(bookId, authorId);
            return Ok(result); // este método devuelve true/false
        }

        // POST: api/AuthorBook
        [HttpPost]
        public async Task<IActionResult> AddRelation([FromBody] CreateBookAuthorDTO dto)
        {
            var result = await _authorBookService.AddBookAuthorAsync(dto.BookId, dto.AuthorId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/AuthorBook/{bookId}/{authorId}
        [HttpDelete("{bookId:int}/{authorId:int}")]
        public async Task<IActionResult> DeleteRelation(int bookId, int authorId)
        {
            var result = await _authorBookService.DeleteByBookAndAuthorAsync(bookId, authorId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // GET: api/AuthorBook/books-by-author/{authorId}
        [HttpGet("books-by-author/{authorId:int}")]
        public async Task<IActionResult> GetBooksByAuthor(int authorId)
        {
            var result = await _authorBookService.GetBooksByAuthorAsync(authorId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // GET: api/AuthorBook/authors-by-book/{bookId}
        [HttpGet("authors-by-book/{bookId:int}")]
        public async Task<IActionResult> GetAuthorsByBook(int bookId)
        {
            var result = await _authorBookService.GetAuthorsByBookAsync(bookId);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}