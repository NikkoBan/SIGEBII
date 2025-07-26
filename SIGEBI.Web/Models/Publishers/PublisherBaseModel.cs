namespace SIGEBI.Web.Models.Publishers
{
    public class PublisherBaseModel
    {
        public required string publisherName { get; set; }
        public required string address { get; set; }
        public required string phoneNumber { get; set; }
        public required string email { get; set; }
        public required string website { get; set; }
        public required string notes { get; set; }

    }
}
