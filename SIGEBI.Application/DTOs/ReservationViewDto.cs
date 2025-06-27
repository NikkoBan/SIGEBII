
namespace SIGEBI.Application.DTOs
{
    public class ReservationViewDto
    {
        public int ReservationId { get; set; }
        public string Usuario { get; set; }
        public string Libro { get; set; }
        public string FechaReserva { get; set; }
        public string FechaVencimiento { get; set; }
        public string Estado { get; set; }

    }
}
