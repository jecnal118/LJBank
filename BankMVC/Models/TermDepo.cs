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
    public class TermDepo
    {
        [Key]
        public int termID { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Column(TypeName = "decimal")]
        [Display(Name = "Amount")]
        public decimal amount { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Column(TypeName = "decimal")]
        [Display(Name = "Interest")]
        public decimal interest { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Display(Name = "Months Until Maturity")]
        public int duration { get; set; }
        [Column(TypeName = "decimal")]
        public decimal output { get; set; }
        [Display(Name = "Customer ID")]
        public int customerID { get; set; }
        [Display(Name = "Message")]
        public string message { get; set; }


        public IEnumerable<SelectListItem> percents { get; set; }


        public TermDepo()
        {

        }

        public void total()
        {
            decimal calc = amount + (amount * interest);
            output = calc;
        }

        public void payout()
        {
            if (duration <= 0)
            {
                amount = 0;
            }
            else
            {
                message = "Your deposit is still in the hold period";
            }
        }

        
    }
}