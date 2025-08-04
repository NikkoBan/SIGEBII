using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOsAplication.LoanDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Persistence.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _loanService.GetAllAsync();
                if (!result.Success)
                    return BadRequest(result.Message);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _loanService.GetByIdAsync(id);
                if (!result.Success)
                    return NotFound(result.Message);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoanCreationDTO dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            try
            {
                var result = await _loanService.CreateAsync(dto);
                if (!result.Success)
                    return BadRequest(result.Message);

                return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] LoanUpdateDTO dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            try
            {
                var result = await _loanService.UpdateAsync(id, dto);
                if (!result.Success)
                    return BadRequest(result.Message);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _loanService.DeleteAsync(id);
                if (!result.Success)
                    return BadRequest(result.Message);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
