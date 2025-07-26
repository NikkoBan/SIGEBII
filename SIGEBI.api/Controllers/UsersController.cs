using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOsAplication.UserDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserAccountService _userAccountService;

        public UsersController(IUserService userService, IUserAccountService userAccountService)
        {
            _userService = userService;
            _userAccountService = userAccountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDisplayDto>>> GetUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDisplayDto>> GetUser(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }
            return Ok(result.Data);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDisplayDto>> GetUserByEmail(string email)
        {
            var result = await _userService.GetUserByEmailAsync(email);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }
            return Ok(result.Data);
        }

        [HttpPost("register")]
        public async Task<ActionResult<OperationResult<UserDisplayDto>>> RegisterUser([FromBody] UserCreationDto userDto)
        {
            var result = await _userService.CreateUserAsync(userDto);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }
            return CreatedAtAction(nameof(GetUser), new { id = result.Data!.UserId }, result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<OperationResult<bool>>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(OperationResult<bool>.Fail("Email y contrase�a son requeridos."));
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers["User-Agent"].ToString();

            var result = await _userAccountService.LoginAsync(request.Email, request.Password, ipAddress ?? "N/A", userAgent ?? "N/A");

            if (result.Success)
            {
                /* Cuando el login es exitoso, dirige al usuario al men� principal de la biblioteca. */
                return Ok(result);
            }
            return Unauthorized(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody] UserUpdateDto userDto)
        {
            var result = await _userService.UpdateUserAsync(id, userDto);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode ?? 500, result);
            }
            return NoContent();
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}