using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOsAplication.UserHistoryDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Persistence.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserHistoryController : ControllerBase
    {
        private readonly IUserHistoryService _userHistoryService;

        public UserHistoryController(IUserHistoryService userHistoryService)
        {
            _userHistoryService = userHistoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserHistoryDisplayDto>>> GetAllUserHistory()
        {
            var result = await _userHistoryService.GetAllUserHistoryAsync();
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserHistoryDisplayDto>> GetUserHistory(int id)
        {
            var result = await _userHistoryService.GetUserHistoryByIdAsync(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }
            return Ok(result.Data);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserHistoryDisplayDto>>> GetUserHistoryByUserId(int userId)
        {
            var result = await _userHistoryService.GetUserHistoryByUserIdAsync(userId);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<OperationResult<UserHistoryDisplayDto>>> PostUserHistory([FromBody] UserHistoryCreationDto historyDto)
        {
            var result = await _userHistoryService.CreateUserHistoryAsync(historyDto);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }
            return CreatedAtAction(nameof(GetUserHistory), new { id = result.Data!.LogId }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserHistory(int id)
        {
            var result = await _userHistoryService.DeleteUserHistoryAsync(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }
            return NoContent();
        }
    }
}