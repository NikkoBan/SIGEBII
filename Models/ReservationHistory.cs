using System;

namespace Models {
    public class ReservationHistory
    {
        public int ReservationHistoryId { get; set; }
        public int OriginalReservationId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? FinalStatus { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}