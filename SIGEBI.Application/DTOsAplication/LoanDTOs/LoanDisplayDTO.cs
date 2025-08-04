namespace SIGEBI.Application.DTOsAplication.LoanDTOs
{
    public class LoanDisplayDTO
    {
        public int Id { get; set; }

        public DateTime LoanDate { get; set; }

        public int UserId { get; set; }

        public int StatusId { get; set; }

        public string? StatusName { get; set; }

        public List<string>? BookTitles { get; set; }
    }
}
