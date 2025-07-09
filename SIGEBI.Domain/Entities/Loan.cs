using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class Loan : BaseEntity<int>
    {
        [Column("LoanID")]
        [Key]
        public override int ID { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string LoanStatus { get; set; } = string.Empty;
        public Book? Book { get; set; }
        public User? User { get; set; }
        public bool Borrado { get; set; } = false;
    }
}