using BankProject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BankMVC.Models
{
    public class BankingContext : DbContext
    {
        public BankingContext() : base("name=BankingProject")
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<bankAccount> Accounts { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<TransHist> TransactionHistory { get; set; }
        public DbSet<TermDepo> TermDeposit { get; set; }
        public DbSet<Login> Emp { get; set; }


}
}