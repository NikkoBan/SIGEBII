using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Dtos.AuthorDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SIGEBI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/Author
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _authorService.GetAllAsync();
            return result.Success
                ? Ok(result.Data)
                : StatusCode(500, new { Message = result.Message ?? "Error al obtener los autores." });
        }

        // GET: api/Author/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _authorService.GetByIdAsync(id);

            if (result.Success)
                return result.Data != null ? Ok(result.Data) : NotFound($"Autor con ID {id} no encontrado.");

            return StatusCode(500, new { Message = result.Message ?? $"Error al obtener el autor con ID {id}." });
        }

        // POST: api/Author
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authorService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result.Message ?? "Fallo al crear el autor.");

            return result.Data is AuthorDTO author
                ? CreatedAtAction(nameof(GetById), new { id = author.AuthorId }, author)
                : Ok(result.Data);
        }

        // PUT: api/Author/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAuthorDTO dto)
        {
            if (id <= 0)
                return BadRequest("ID de autor inválido.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authorService.UpdateAsync(id, dto);

            if (result.Success)
                return result.Data != null ? Ok(result.Data) : NoContent();

            return result.Message?.ToLower().Contains("no encontrado") == true
                ? NotFound(result.Message)
                : BadRequest(result.Message ?? $"Fallo al actualizar el autor con ID {id}.");
        }

        // DELETE: api/Author/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authorService.DeleteAsync(id);

            if (result.Success)
                return NoContent();

            return result.Message?.ToLower().Contains("no encontrado") == true
                ? NotFound(result.Message)
                : StatusCode(500, new { Message = result.Message ?? $"Error al eliminar el autor con ID {id}." });
        }

        // GET: api/Author/{id}/genres
        [HttpGet("{id:int}/genres")]
        public async Task<IActionResult> GetGenresByAuthor(int id)
        {
            var genres = await _authorService.GetGenresByAuthorAsync(id);
            return Ok(genres);
        }
    }
}