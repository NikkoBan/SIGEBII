using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Validations;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookRepository.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var book = await _bookRepository.GetEntityByIdAsync(id);
            if (book == null)
                return NotFound($"No se encontró el libro con ID {id}");

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                ISBN = dto.ISBN,
                PublicationDate = dto.PublicationDate,
                CategoryId = dto.CategoryId,
                PublisherId = dto.PublisherId,
                Language = dto.Language,
                Summary = dto.Summary,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.AvailableCopies,
                GeneralStatus = dto.GeneralStatus
            };

            if (!RepoValidation.ValidarBook(book))
                return BadRequest("Datos del libro inválidos.");

            var result = await _bookRepository.SaveEntityAsync(book);
            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = book.ID }, book);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Book book)
        {
            if (!RepoValidation.ValidarID(id) || !RepoValidation.ValidarBook(book))
                return BadRequest("Datos inválidos.");

            var existing = await _bookRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el libro con ID {id}");

            book.ID = id;
            var result = await _bookRepository.UpdateEntityAsync(book);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var existing = await _bookRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el libro con ID {id}");

            var result = await _bookRepository.RemoveEntityAsync(existing);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}
