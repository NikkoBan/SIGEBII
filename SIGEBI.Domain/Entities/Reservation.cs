using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities
{
    public class Reservation : BaseEntity<int>
    {
        [Column("ReservationID")]
        [Key]
        public override int ID { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ReservationStatus { get; set; } = string.Empty;
        public Book? Book { get; set; }
        public User? User { get; set; }
    }
}
