using Microsoft.AspNetCore.Mvc;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Domain.Validations;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookAuthorController : ControllerBase
    {
        private readonly IBookAuthorRepository _bookAuthorRepository;

        public BookAuthorController(IBookAuthorRepository bookAuthorRepository)
        {
            _bookAuthorRepository = bookAuthorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _bookAuthorRepository.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var bookAuthor = await _bookAuthorRepository.GetEntityByIdAsync(id);
            if (bookAuthor == null)
                return NotFound($"No se encontró el registro con ID {id}");

            return Ok(bookAuthor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookAuthor bookAuthor)
        {
            if (!RepoValidation.ValidarBookAuthor(bookAuthor))
                return BadRequest("Datos de relación Book-Author inválidos.");

            var result = await _bookAuthorRepository.SaveEntityAsync(bookAuthor);
            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = bookAuthor.ID }, bookAuthor);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookAuthor bookAuthor)
        {
            if (!RepoValidation.ValidarID(id) || !RepoValidation.ValidarBookAuthor(bookAuthor))
                return BadRequest("Datos inválidos.");

            var existing = await _bookAuthorRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el registro con ID {id}");

            bookAuthor.ID = id;
            var result = await _bookAuthorRepository.UpdateEntityAsync(bookAuthor);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var existing = await _bookAuthorRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el registro con ID {id}");

            var result = await _bookAuthorRepository.RemoveEntityAsync(existing);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}
