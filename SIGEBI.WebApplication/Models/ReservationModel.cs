namespace SIGEBI.WebApplication.Models // hacer refactoring
{
    public class CreateReservationModel
    {
        //public int id { get; set; }
        public int bookId { get; set; }
        public int userId { get; set; }
        public int statusId { get; set; }
        public DateTime reservationDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }
        //public object updatedAt { get; set; }
        //public object updatedBy { get; set; }
        //public bool isDeleted { get; set; }
        //public object deletedAt { get; set; }
        //public object deletedBy { get; set; }
        //public object confirmationDate { get; set; }
        //public object confirmedBy { get; set; }
        //public object book { get; set; }
        //public object user { get; set; }
        //public object reservationStatus { get; set; }
    }



    public class ReservationResponse<T>
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }

    public class ReservationModel
    {
        public int id { get; set; }
        public int bookId { get; set; }
        public int userId { get; set; }
        public int statusId { get; set; }
        public DateTime reservationDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }
    }
}

    
