using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject
{
    public class TransHist
    {
        [Key]
        public int TransID { get; set; }
        //[Column(TypeName = "date")]
        public DateTime date { get; set; }
        public int customerID { get; set; }
        public string type { get; set; }
        [Column(TypeName = "decimal")]
        public decimal amount { get; set; }
        public string employee { get; set; }
        

        public virtual Customer customer { get; set; }

        public TransHist()
        {

        }
    }
}
