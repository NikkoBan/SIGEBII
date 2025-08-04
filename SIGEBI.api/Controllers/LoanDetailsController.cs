using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOsAplication.LoanDetailsDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Persistence.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanDetailsController : ControllerBase
    {
        private readonly ILoanDetailsService _loanDetailsService;

        public LoanDetailsController(ILoanDetailsService loanDetailsService)
        {
            _loanDetailsService = loanDetailsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoanDetailsCreationDTO dto)
        {
            var result = await _loanDetailsService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.LoanDetailId }, result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _loanDetailsService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _loanDetailsService.GetAllAsync();
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LoanDetailsUpdateDTO dto)
        {
            var result = await _loanDetailsService.UpdateAsync(id, dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loanDetailsService.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}
