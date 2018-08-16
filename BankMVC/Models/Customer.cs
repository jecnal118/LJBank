using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject
{
    public class Customer
    {
        [Key]
        [Display(Name = "ID")]
        public int customerID { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Display(Name="First Name")]
        public string fName { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Display(Name = "Last Name")]
        public string lName { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Display(Name = "City")]
        public string city { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Display(Name = "State")]
        public string state { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Display(Name = "Zip Code")]
        public string zip { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        //[Column(TypeName = "date")]
        public DateTime DOB { get; set; }
        [Display(Name = "Phone")]
        public string tele { get; set; }
        [Display(Name = "E-Mail")]
        public string email { get; set; }
        [Display(Name = "Active")]
        public bool locked { get; set; }

        public IList<bankAccount> bankAccounts;
        public IList<Loan> Loans;
        public IList<TransHist> TransHists;
        public IList<TermDepo> TermDepos;

        public Customer()
        {

        }

        public void Close()
        {
            locked = true;
        }
    }
}
