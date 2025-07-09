using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.DTOs.PublishersDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;


namespace SIGEBI.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : ControllerBase
    {
        private readonly IPublishersService _publishersService;

        public PublishersController(IPublishersService publishersService)
        {
            _publishersService = publishersService;
        }

        // GET: api/Publishers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var publishers = await _publishersService.GetAllAsync();
            return Ok(publishers);
        }

        // GET: api/Publishers/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var publisher = await _publishersService.GetByIdAsync(id);
            if (publisher == null)
                return NotFound(new OperationResult { Success = false, Message = "Editorial no encontrada." });

            return Ok(publisher);
        }

        // POST: api/Publishers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreationPublisherDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _publishersService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            
            return Ok(result);
        }

        // PUT: api/Publishers/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePublisherDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Id != id)
                return BadRequest(new OperationResult { Success = false, Message = "El ID de la URL no coincide con el del cuerpo." });

            var result = await _publishersService.UpdateAsync(dto);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, [FromBody] RemovePublisherDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Id != id)
                return BadRequest(new OperationResult { Success = false, Message = "El ID de la URL no coincide con el del cuerpo." });

            var result = await _publishersService.DeleteAsync(dto);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}