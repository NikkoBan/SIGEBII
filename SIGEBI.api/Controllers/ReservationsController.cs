using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Contracts.Services;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SIGEBI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequestDto request)
        { 

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await reservationService.CreateReservationAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    message = result.Message,
                });
            }
            return Ok(new
            {
                message = result.Message,
                data = result.Data
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            var result = await reservationService.GetReservationByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(new
                {
                    message = result.Message,
                });
            }
            return Ok(new
            {
                message = result.Message,
                data = result.Data
            });
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllReservations()
        {
            var result = await reservationService.GetAllReservationsAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }
            
            return Ok(result.Data);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] UpdateReservationDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            request.Id = id; // Set the ID from the route parameter

            var result = await reservationService.UpdateReservationAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    message = result.Message,
                });
            }
            return Ok(new
            {
                message = result.Message,
                data = result.Data
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var result = await reservationService.DeleteReservationAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(new
                {
                    message = result.Message,
                });
            }
            return NoContent();
        }

        [HttpGet("ByUser/{userId:int}")]
        public async Task<IActionResult> GetReservationsByUserId(int userId)
        {
            var result = await reservationService.GetReservationByIdAsync(userId); 
            if (!result.IsSuccess)
            {
                return NotFound(new
                {
                    message = result.Message,
                });
            }
;
            return Ok(new
            {
                message = "Reservations retrieved successfully.",
                data = result.Data,
            });
        }

    }
}
