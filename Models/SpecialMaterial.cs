namespace Models {
    public class SpecialMaterial
    {
        public int MaterialId { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime RequestDate { get; set; }
        public int RequestedByUserId { get; set; }
        public string ApprovalStatus { get; set; } = null!;
        public int? ApprovedByUserId { get; set; }
        public DateTime? AcquisitionDate { get; set; }
    }
}