using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankMVC.Models;
using BankProject;
using Microsoft.AspNet.Identity;

namespace BankMVC.Controllers
{
    public class LoansController : Controller
    {
        private BankingContext db = new BankingContext();

        // GET: Loans
        public ActionResult Index()
        {
            var loans = db.Loans.Include(l => l.customer);
            return View(loans.ToList());
        }

        // GET: Loans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // GET: Loans/Create
        public ActionResult Create(int id)
        {
            var loan = new Loan();
            loan.customerID = (int)id;
            loan.percents = new[]
       {
            new SelectListItem { Value = "0.10", Text = "1 Month - 10%" },
            new SelectListItem { Value = "0.15", Text = "3 Months - 15%" },
            new SelectListItem { Value = "0.20", Text = "6 Months - 20%" },
            new SelectListItem { Value = "0.25", Text = "> 6 Months - 25%" }
        };
            //ViewBag.customerID = new SelectList(db.Customers, "customerID", "fName");
            return View(loan);
        }

        // POST: Loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "loanID,amount,interest,duration,customerID,locked")] Loan loan)
        {
            loan.amount += (loan.amount * loan.interest);
            if (ModelState.IsValid)
            {
                TransHist transaction = new TransHist()
                {
                    date = DateTime.Now,
                    customerID = loan.customerID,
                    type = $"Loan - {loan.loanID}",
                    amount = loan.amount,
                    employee = User.Identity.GetUserName()
                };
                db.TransactionHistory.Add(transaction);
                db.Loans.Add(loan);
                db.SaveChanges();
                return RedirectToAction("userLoans", new { id = loan.customerID });
            }

            ViewBag.customerID = new SelectList(db.Customers, "customerID", "fName", loan.customerID);
            return View(loan);
        }

        // GET: Loans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "fName", loan.customerID);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "loanID,amount,interest,duration,customerID,locked")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "fName", loan.customerID);
            return View(loan);
        }

        public ActionResult userLoans(int? id)
        {
            ViewBag.customerID = id;
            //var accounts = db.Accounts.Include(b => b.customer);
            var accounts = from a in db.Loans.Include(a => a.customer)
                           where a.customerID == id && !a.locked
                           select a;
            return View(accounts.ToList());
        }

        public ActionResult Payment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(int loanID, decimal? amount)
        {
            Loan loan = db.Loans.Find(loanID);
            
            if (loan == null)
            {
                return HttpNotFound();
            }
            else if (!amount.HasValue)
            {
                return View(loan);
            }
            else if (amount > loan.amount)
            {
                TempData["msg"] = "<script>alert('Can not overpay on loan.');</script>";
                return View(loan);
            }
            else
            {
                loan.installment((decimal)amount);
            }
            if (ModelState.IsValid)
            {
                TransHist transaction = new TransHist()
                {
                    date = DateTime.Now,
                    customerID = loan.customerID,
                    type = $"Loan Payment - {loan.loanID}",
                    amount = (decimal)amount,
                    employee = User.Identity.GetUserName()
                };
                db.TransactionHistory.Add(transaction);
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("userLoans", new { id = loan.customerID });

            }
            return View(loan);
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
