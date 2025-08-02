using Microsoft.AspNetCore.Mvc;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Domain.Validations;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;

        public LoanController(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var loans = await _loanRepository.GetAllAsync();
            return Ok(loans);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var loan = await _loanRepository.GetLoanWithDetailsByIdAsync(id);
            if (loan == null)
                return NotFound($"No se encontró el préstamo con ID {id}");

            return Ok(loan);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Loan loan)
        {
            if (!RepoValidation.ValidarLoan(loan))
                return BadRequest("Datos del préstamo inválidos.");

            var result = await _loanRepository.SaveEntityAsync(loan);
            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = loan.ID }, loan);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Loan loan)
        {
            if (!RepoValidation.ValidarID(id) || !RepoValidation.ValidarLoan(loan))
                return BadRequest("Datos inválidos.");

            var existing = await _loanRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el préstamo con ID {id}");

            loan.ID = id;
            var result = await _loanRepository.UpdateEntityAsync(loan);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return BadRequest("ID inválido.");

            var existing = await _loanRepository.GetEntityByIdAsync(id);
            if (existing == null)
                return NotFound($"No se encontró el préstamo con ID {id}");

            var result = await _loanRepository.RemoveEntityAsync(existing);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}
