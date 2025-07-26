using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Dtos.BooksDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SIGEBI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookService.GetAllAsync();
            return result.Success
                ? Ok(result)
                : StatusCode(500, new { Message = result.Message ?? "Error al obtener los libros." });
        }

        // GET: api/Book/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookService.GetByIdAsync(id);
            if (result.Success)
                return result != null ? Ok(result) : NotFound($"Libro con ID {id} no encontrado.");

            return StatusCode(500, new { Message = result.Message ?? $"Error al obtener el libro con ID {id}." });
        }

        // POST: api/Book
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBookDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isbnCheck = await _bookService.CheckDuplicateISBNAsync(dto.ISBN);
            if (isbnCheck.Success && isbnCheck.Data is bool exists && exists)
                return BadRequest($"Ya existe un libro con el ISBN: {dto.ISBN}");

            var result = await _bookService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result.Message ?? "Fallo al crear el libro.");

            if (result.Data is BookDTO bookDto)
                return CreatedAtAction(nameof(GetById), new { id = bookDto.BookId }, bookDto);

            return Ok("Libro creado exitosamente, pero no se pudo obtener el ID.");
        }

        // PUT: api/Book/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto dto)
        {
            if (id <= 0)
                return BadRequest("ID de libro inválido.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bookService.UpdateAsync(id, dto);

            if (result.Success)
                return result != null ? Ok(result) : NoContent();

            return result.Message?.ToLower().Contains("no encontrado") == true
                ? NotFound(result.Message)
                : BadRequest(result.Message ?? $"Fallo al actualizar el libro con ID {id}.");
        }

        // DELETE: api/Book/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.DeleteAsync(id);

            if (result.Success)
                return NoContent();

            return result.Message?.ToLower().Contains("no encontrado") == true
                ? NotFound(result.Message)
                : StatusCode(500, new { Message = result.Message ?? $"Error al eliminar el libro con ID {id}." });
        }

        // GET: api/Book/category/{categoryId}
        [HttpGet("category/{categoryId:int}")]
        public async Task<IActionResult> GetBooksByCategory(int categoryId)
        {
            var result = await _bookService.GetBooksByCategoryAsync(categoryId);
            return result.Success ? Ok(result.Data) : NotFound(result.Message);
        }

        // GET: api/Book/publisher/{publisherId}
        [HttpGet("publisher/{publisherId:int}")]
        public async Task<IActionResult> GetBooksByPublisher(int publisherId)
        {
            var result = await _bookService.GetBooksByPublisherAsync(publisherId);
            return result.Success ? Ok(result.Data) : NotFound(result.Message);
        }

        // GET: api/Book/available
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableBooks()
        {
            var result = await _bookService.GetAvailableBooksAsync();
            return result.Success ? Ok(result.Data) : NotFound(result.Message);
        }

        // GET: api/Book/search?term=algo
        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string term)
        {
            var result = await _bookService.SearchBooksAsync(term);
            return result.Success ? Ok(result.Data) : NotFound(result.Message);
        }

        // POST: api/Book/check-duplicate-title
        [HttpPost("check-duplicate-title")]
        public async Task<IActionResult> CheckDuplicateTitle([FromBody] string title)
        {
            var result = await _bookService.CheckDuplicateBookTitleAsync(title);
            return Ok(new { IsDuplicate = result.Success });
        }

        // POST: api/Book/check-duplicate-isbn
        [HttpPost("check-duplicate-isbn")]
        public async Task<IActionResult> CheckDuplicateISBN([FromBody] string isbn)
        {
            var result = await _bookService.CheckDuplicateISBNAsync(isbn);
            return Ok(new { IsDuplicate = result.Success });
        }
    }
}