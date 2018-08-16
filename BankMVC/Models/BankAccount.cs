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
    public class bankAccount
    {

        [Key]
        [Display(Name = "Account ID")]
        public int accountID { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Column(TypeName = "decimal")]
        [Display(Name = "Balance")]
        public decimal balance { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Display(Name = "Business Account")]
        public bool type { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [Column(TypeName = "decimal")]
        [Display(Name = "Amount of Interest")]
        public decimal interest { get; set; }
        [Display(Name = "ID")]
        public int customerID { get; set; }
        [Display(Name = "Active")]
        public bool locked { get; set; }

        public IEnumerable<SelectListItem> percents { get; set; }


        public virtual Customer customer { get; set; }

        public bankAccount()
        {

        }

        //Subtract funds
        public void withdraw(decimal amount)
        {
            //allowed to overdraft
            if (amount > balance)
            {
                //if it's a business account
                if(type){
                    balance -= amount;
                    //OVERDRAFT
                    if(balance < 0)
                    {
                        balance += (balance * (interest + (decimal)0.1));
                    }
                }
            }
            else
            {
                balance -= amount;
            }
        }

        //Add funds
        public void deposit(decimal amount)
        {
            balance += amount;
        }

        //Close Account
        public void close()
        {
            locked = true;
        }

        }
}
