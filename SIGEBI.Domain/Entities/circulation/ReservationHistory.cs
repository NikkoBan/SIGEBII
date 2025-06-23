namespace SIGEBI.Domain.Entities.circulation
{
    public class ReservationHistory // Guarga un registro de reservas ya finalizadas. Es una copia de la transaccion finalizada.
    {
        public int HistoryId { get; set; }
        public int ReservationId { get; set; }
        public int BookId { get; set; } 
        public int UserId { get; set; }
        public int StatusId  { get; set; } 
        public DateTime ReservationDate { get; set; } 
        public DateTime ExpirationDate { get; set; } 
 
           
    }
}
