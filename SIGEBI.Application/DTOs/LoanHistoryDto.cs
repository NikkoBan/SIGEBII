namespace SIGEBI.Application.DTOs
{
    public class LoanHistoryDto
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
