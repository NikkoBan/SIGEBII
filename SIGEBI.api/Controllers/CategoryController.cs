using Microsoft.AspNetCore.Mvc;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Validations;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var category = await _categoryRepository.GetEntityByIdAsync(id);
            if (category == null)
                return NotFound($"No se encontró la categoría con ID {id}");

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (!RepoValidation.ValidarCategory(category))
                return BadRequest("Datos de la categoría inválidos.");

            var result = await _categoryRepository.SaveEntityAsync(category);
            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = category.ID }, category);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (!RepoValidation.ValidarID(id) || !RepoValidation.ValidarCategory(category))
                return BadRequest("Datos inválidos.");

            var existing = await _categoryRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró la categoría con ID {id}");

            category.ID = id;
            var result = await _categoryRepository.UpdateEntityAsync(category);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var existing = await _categoryRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró la categoría con ID {id}");

            var result = await _categoryRepository.RemoveEntityAsync(existing);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}
