using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.circulation
{
    public  class Reservation : AuditEntity<int>
    {
        public override int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }    
        public int StatusId { get; set; } 
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string? ReservationStatus { get; set; } 
    }
}
