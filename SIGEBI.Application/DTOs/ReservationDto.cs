
namespace SIGEBI.Application.DTOs
{
    public class ReservationDto //Mostrar info de reservacion
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        //public int Id { get; set; }
        //public required string Usuario { get; set; }
        //public required string Libro { get; set; }
        //public required string FechaReserva { get; set; }
        //public required string FechaVencimiento { get; set; }
        //public required string Estado { get; set; }

    }
}
