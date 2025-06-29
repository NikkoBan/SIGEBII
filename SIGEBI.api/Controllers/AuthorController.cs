using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SIGEBI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        // GET: api/<AuthorController>
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _authorService.GetAllAsync(); 
            if (result.Success)
            {
                return Ok(result.Data); 
            }
            
            return StatusCode(500, new { Message = result.Message ?? "Error al obtener los autores." });
        }

        // GET api/<AuthorController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _authorService.GetByIdAsync(id); 
            if (result.Success)
            {
                if (result.Data != null)
                {
                    return Ok(result.Data); 
                }
                
                return NotFound($"Autor con ID {id} no encontrado."); 
            }
          
            return StatusCode(500, new { Message = result.Message ?? $"Error al obtener el autor con ID {id}." });
        }

        // POST api/<AuthorController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAuthorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authorService.CreateAsync(dto);

            if (result.Success)
            {
                if (result.Data is AuthorDTO createdAuthor)
                {
                    return CreatedAtAction(nameof(Get), new { id = createdAuthor.AuthorId }, createdAuthor);
                }

                return Ok(result.Data);
            }

            return BadRequest(result.Message ?? "Fallo al crear el autor.");
        }
        // PUT api/<AuthorController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateAuthorDTO dto)
        {
            if (id <= 0)
            {
                return BadRequest("ID de autor inválido.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authorService.UpdateAsync(id, dto);

            if (result.Success)
            {
                if (result.Data != null)
                {
                    return Ok(result.Data);
                }

                return NoContent();
            }

            if (!result.Success && result.Message?.ToLower().Contains("no encontrado") == true)
            {
                return NotFound(result.Message);
            }

            return BadRequest(result.Message ?? $"Fallo al actualizar el autor con ID {id}.");
        }


        // DELETE api/<AuthorController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authorService.DeleteAsync(id); 
            if (result.Success)
            {
                return NoContent(); 
            }
           
            if (!result.Success && result.Message.Contains("no encontrado")) 
            {
                return NotFound(result.Message); 
            }
            
            return StatusCode(500, new { Message = result.Message ?? $"Error al eliminar el autor con ID {id}." });
        }
    }

}

