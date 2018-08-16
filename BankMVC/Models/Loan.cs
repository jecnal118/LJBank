using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BankProject
{
    public class Loan
    {
        [Key]
        [Display(Name = "ID")]
        public int loanID { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Column(TypeName = "decimal")]
        [Display(Name = "Amount")]
        public decimal amount { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Column(TypeName = "decimal")]
        [Display(Name = "Interest")]
        public decimal interest { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Display(Name = "Duration In Months")]
        public int duration { get; set; }
        public int customerID { get; set; }
        public bool locked { get; set; }

        public IEnumerable<SelectListItem> percents { get; set; }

        public virtual Customer customer { get; set; }



        public Loan()
        {

        }

        public void total()
        {
            decimal calc = amount + (amount * interest);
            amount = calc;
        }

        public void installment(decimal money)
        {
            amount -= money;
        }
    }
}
