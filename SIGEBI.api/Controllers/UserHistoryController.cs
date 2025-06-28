using Microsoft.AspNetCore.Mvc;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SIGEBI.Persistence.Base;
using System;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserHistoryController : ControllerBase
    {
        private readonly IUserHistoryRepository _userHistoryRepository;

        public UserHistoryController(IUserHistoryRepository userHistoryRepository)
        {
            _userHistoryRepository = userHistoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserHistory>>> GetAllUserHistory()
        {
            var history = await _userHistoryRepository.GetAllAsync();
            return Ok(history);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserHistory>> GetUserHistory(int id)
        {
            var entry = await _userHistoryRepository.GetByIdAsync(id);
            if (entry == null)
            {
                return NotFound("Entrada de historial no encontrada.");
            }
            return Ok(entry);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserHistory>>> GetUserHistoryByUserId(int userId)
        {
            var history = await _userHistoryRepository.GetByUserIdAsync(userId);
            if (history == null || !history.Any())
            {
                return NotFound("No se encontró historial para el usuario especificado.");
            }
            return Ok(history);
        }

        [HttpPost]
        public async Task<ActionResult<OperationResult>> PostUserHistory([FromBody] UserHistory userHistory)
        {
            if (userHistory == null || userHistory.UserId <= 0 || string.IsNullOrWhiteSpace(userHistory.EnteredEmail))
            {
                return BadRequest(OperationResult.Fail("Datos de historial inválidos. UserId y EnteredEmail son requeridos."));
            }

            userHistory.AttemptDate = DateTime.Now;

            var result = await _userHistoryRepository.AddAsync(userHistory);

            if (result.Success)
            {
                return CreatedAtAction(nameof(GetUserHistory), new { id = ((UserHistory)result.Data!).LogId }, result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserHistory(int id)
        {
            var result = await _userHistoryRepository.RemoveAsync(id);

            if (result.Success)
            {
                return NoContent();
            }
            if (result.Message != null && result.Message.Contains("Entidad no encontrada"))
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }
    }
}