using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.Services;
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
            var dtos = publishers.Select(p => p.ToDto()).ToList();
            return Ok(dtos);
        }

        // GET: api/Publishers/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var publisher = await _publishersService.GetByIdAsync(id);
            if (publisher == null)
                return NotFound(new OperationResult { Success = false, Message = "Editorial no encontrada." });

            return Ok(publisher.ToDto());
        }

        // POST: api/Publishers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PublisherCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = dto.ToEntity();
            var result = await _publishersService.CreateAsync(entity);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = entity.ID }, entity.ToDto());
        }

        // PUT: api/Publishers/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PublisherCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _publishersService.GetByIdAsync(id);
            if (entity == null)
                return NotFound(new OperationResult { Success = false, Message = "Editorial no encontrada." });

            dto.MapToEntity(entity);
            var result = await _publishersService.UpdateAsync(entity);
            if (!result.Success)
                return BadRequest(result);

            return Ok(entity.ToDto());
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _publishersService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}