using Microsoft.AspNetCore.Mvc;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Domain.Validations;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanHistoryController : ControllerBase
    {
        private readonly ILoanHistoryRepository _loanHistoryRepository;

        public LoanHistoryController(ILoanHistoryRepository loanHistoryRepository)
        {
            _loanHistoryRepository = loanHistoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _loanHistoryRepository.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var history = await _loanHistoryRepository.GetEntityByIdAsync(id);
            if (history == null)
                return NotFound($"No se encontró el historial con ID {id}");

            return Ok(history);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoanHistory history)
        {
            if (!RepoValidation.ValidarLoanHistory(history))
                return BadRequest("Datos del historial de préstamo inválidos.");

            var result = await _loanHistoryRepository.SaveEntityAsync(history);
            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = history.ID }, history);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] LoanHistory history)
        {
            if (!RepoValidation.ValidarID(id) || !RepoValidation.ValidarLoanHistory(history))
                return BadRequest("Datos inválidos.");

            var existing = await _loanHistoryRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el historial con ID {id}");

            history.ID = id;
            var result = await _loanHistoryRepository.UpdateEntityAsync(history);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var existing = await _loanHistoryRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el historial con ID {id}");

            var result = await _loanHistoryRepository.RemoveEntityAsync(existing);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}
