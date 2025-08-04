using System;

namespace SIGEBI.Application.DTOsAplication.LoanHistoryDTOs
{
    public class LoanHistoryUpdateDTO
    {
        public int HistoryId { get; set; }
        public int LoanId { get; set; }
        public int StatusId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
