
using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBI.Domain.Entities.Configuration
{
    public  class Books : Base.BaseEntity<int>
    {
        [Column("BookId")]
        [Key]
        public override int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public DateOnly? PublicationDate { get; set; }
        public string Language { get; set; }
        public string Summary { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string GeneralStatus { get; set; }

     
        public int CategoryId { get; set; }
        public Categories Categories { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
    }


    

     
}
