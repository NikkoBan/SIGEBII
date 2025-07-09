using Microsoft.AspNetCore.Mvc;
using SIGEBI.api.Services;
using SIGEBI.Application.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SIGEBI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationApiService _reservationApiService; // Injecting the ReservationApiService

        public ReservationController(ReservationApiService reservationApiService)
        {
            _reservationApiService = reservationApiService;
        }

        // GET: api/<ReservationController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAll()
        {
            var reservation = await _reservationApiService.GetAllReservationsAsync(); //linea 24
            return Ok(reservation);

        }

        // GET api/<ReservationController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetById(int id)
        {
            try
            {
                var reservation = await _reservationApiService.GetReservationByIdAsync(id);
                return Ok(reservation);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // Return 404 if reservation not found
            }
        }

        // POST api/<ReservationController>
        [HttpPost]
        public async Task<ActionResult<ReservationDto>> Create([FromBody] CreateReservationRequestDto requestDto)
        {
            try
            {
                var createdReservation = await _reservationApiService.CreateReservationAsync(requestDto);
                return CreatedAtAction(nameof(GetById), new { id = createdReservation.ReservationId }, createdReservation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return 400 if creation fails
            }
           
        }

        // DELETE api/<ReservationController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string deletedBy)
        {
            try
            {
                await _reservationApiService.DeleteReservationAsync(id, deletedBy);
                return NoContent(); // Return 204 
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // Return 404 

            }
        }
    }
}

