using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOsAplication.LoanHistoryDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Persistence.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanHistoryController : ControllerBase
    {
        private readonly ILoanHistoryService _loanHistoryService;

        public LoanHistoryController(ILoanHistoryService loanHistoryService)
        {
            _loanHistoryService = loanHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _loanHistoryService.GetAllAsync();
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
                var result = await _loanHistoryService.GetByIdAsync(id);
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
        public async Task<IActionResult> Create([FromBody] LoanHistoryCreationDTO dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            try
            {
                var result = await _loanHistoryService.CreateAsync(dto);
                if (!result.Success)
                    return BadRequest(result.Message);

                return CreatedAtAction(nameof(GetById), new { id = result.Data?.HistoryId }, result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] LoanHistoryUpdateDTO dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            try
            {
                var result = await _loanHistoryService.UpdateAsync(id, dto);
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
                var result = await _loanHistoryService.DeleteAsync(id);
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
