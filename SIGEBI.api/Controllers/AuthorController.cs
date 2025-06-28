using Microsoft.AspNetCore.Mvc;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Validations;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authorRepository.GetAllAsync();
            return Ok(authors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var author = await _authorRepository.GetEntityByIdAsync(id);
            if (author == null)
                return NotFound($"No se encontró el autor con ID {id}");

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Author author)
        {
            if (!RepoValidation.ValidarAuthor(author))
                return BadRequest("Datos del autor inválidos.");

            var result = await _authorRepository.SaveEntityAsync(author);
            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = author.ID }, author);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Author author)
        {
            if (!RepoValidation.ValidarID(id) || !RepoValidation.ValidarAuthor(author))
                return BadRequest("Datos inválidos.");

            var existing = await _authorRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el autor con ID {id}");

            author.ID = id;
            var result = await _authorRepository.UpdateEntityAsync(author);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var existing = await _authorRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el autor con ID {id}");

            var result = await _authorRepository.RemoveEntityAsync(existing);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}
