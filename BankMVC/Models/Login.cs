using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject
{
    public class Login
    {
        [Key]
        public string username { get; set; }
        public string password { get; set; }

        public Login()
        {

        }
    }
}
