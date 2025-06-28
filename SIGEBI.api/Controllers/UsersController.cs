using Microsoft.AspNetCore.Mvc;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SIGEBI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAccountService _userAccountService;

        public UsersController(IUserRepository userRepository, IUserAccountService userAccountService)
        {
            _userRepository = userRepository;
            _userAccountService = userAccountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            var user = await _userAccountService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("Usuario no encontrado por email.");
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<OperationResult>> RegisterUser([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.InstitutionalEmail) || string.IsNullOrWhiteSpace(user.PasswordHash) || string.IsNullOrWhiteSpace(user.FullName))
            {
                return BadRequest(OperationResult.Fail("Email, contraseña y nombre completo son requeridos para el registro."));
            }

            var result = await _userAccountService.RegisterAsync(user);

            if (result.Success)
            {
                return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, OperationResult.Ok("Usuario registrado exitosamente."));
            }
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<OperationResult<bool>>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(OperationResult<bool>.Fail("Email y contraseña son requeridos."));
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers["User-Agent"].ToString();

            var result = await _userAccountService.LoginAsync(request.Email, request.Password, ipAddress ?? "N/A", userAgent ?? "N/A");

            if (result.Success)
            {
                /* 
                   Cuando el login es exitoso, dirige al usuario al menú principal de la biblioteca. 

                   Esto es un ejemplo conceptual. En una API REST, no "redireccionas" directamente a una página.
                   Generalmente, la API devolvería un token de autenticación (ej. JWT) y la aplicación cliente (web/móvil)
                   usaría ese token para acceder a recursos protegidos y manejaría la navegación a su "menú principal".
                   Para el propósito de esta solicitud, el "redireccionamiento" se traduce en devolver un 200 OK
                   que la aplicación cliente interpretaría como luz verde para mostrar la interfaz de la biblioteca.

                */
                return Ok(result);
            }
            return Unauthorized(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody] User user)
        {
            if (id != user.UserId)
            {
                return BadRequest(OperationResult.Fail("El ID de la ruta no coincide con el ID del usuario en el cuerpo de la solicitud."));
            }

            var result = await _userAccountService.UpdateUserAsync(user);

            if (result.Success)
            {
                return NoContent();
            }
            if (result.Message != null && result.Message.Contains("Usuario no encontrado"))
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userAccountService.DeleteUserAsync(id);

            if (result.Success)
            {
                return NoContent();
            }
            if (result.Message != null && result.Message.Contains("Usuario no encontrado"))
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}