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
    public class bankAccountsController : Controller
    {
        private BankingContext db = new BankingContext();


        // GET: bankAccounts
        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(b => b.customer);
            return View(accounts.ToList());
        }

        // GET: bankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bankAccount bankAccount = db.Accounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }


        // GET: bankAccounts/Create
        public ActionResult Create(int id)
        {
            var bankAccount = new bankAccount();
            bankAccount.customerID = (int)id;
            bankAccount.percents = new[]
       {
            new SelectListItem { Value = "0.00", Text = "Business - 0%" },
            new SelectListItem { Value = "0.15", Text = "Checking - 0.15%" }
        };
            return View(bankAccount);
        }
       
        // POST: bankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "accountID,balance,type,interest,customerID,locked")] bankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                TransHist transaction = new TransHist()
                {
                    date = DateTime.Now,
                    customerID = bankAccount.customerID,
                    type = "Account Creation",
                    amount = bankAccount.balance,
                    employee = User.Identity.GetUserName()
                };
                db.TransactionHistory.Add(transaction);
                db.Accounts.Add(bankAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.customerID = new SelectList(db.Customers, "customerID", "fName", bankAccount.customerID);
            return RedirectToAction("userAccounts", new { id = bankAccount.customerID });
            
        }

        // GET: bankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bankAccount bankAccount = db.Accounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "fName", bankAccount.customerID);
            return View(bankAccount);
        }

        // POST: bankAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "accountID,balance,type,interest,customerID,locked")] bankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "fName", bankAccount.customerID);
            return View(bankAccount);
        }


        public ActionResult userAccounts(int? id)
        {
            ViewBag.customerID = id;
            var accounts = from a in db.Accounts.Include(a => a.customer)
                           where a.customerID == id && !a.locked
                           select a;
            return View(accounts.ToList());
        }

        public ActionResult Withdraw(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bankAccount bankAccount = db.Accounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw(int accountID, decimal? balance)
        {
            bankAccount bankAccount = db.Accounts.Find(accountID);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            if (!balance.HasValue)
            {
                return View(bankAccount);
            }
            if (bankAccount.balance >= 0)
            {
                if (bankAccount.type)
                {
                    bankAccount.withdraw((decimal)balance);
                }
                else
                {
                    if (balance > bankAccount.balance)
                    {
                        TempData["msg"] = "<script>alert('Checking accounts can't be overdrawn.');</script>";
                        return View(bankAccount);
                    }
                    else
                    {
                        bankAccount.withdraw((decimal)balance);
                    }
                }
            }
            else
            {
                TempData["msg"] = "<script>alert('Current debts must be paid before withdrawing more money.');</script>";
                return View(bankAccount);
            }
            if (ModelState.IsValid)
            {
                TransHist transaction = new TransHist()
                {
                    date = DateTime.Now,
                    customerID = bankAccount.customerID,
                    type = $"Withdraw - {bankAccount.accountID}",
                    amount = (decimal)balance,
                    employee = User.Identity.GetUserName()
                };
                db.TransactionHistory.Add(transaction);
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("userAccounts", new { id = bankAccount.customerID });
            }
            return View(bankAccount);
        }


        public ActionResult Deposit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bankAccount bankAccount = db.Accounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit(int accountID, decimal? balance)
        {
            bankAccount bankAccount = db.Accounts.Find(accountID);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            else if (!balance.HasValue) {
                return View(bankAccount);
            }

            else
            {
                bankAccount.deposit((decimal)balance);
            }

            if (ModelState.IsValid)
            {
                TransHist transaction = new TransHist()
                {
                    date = DateTime.Now,
                    customerID = bankAccount.customerID,
                    type = $"Deposit - {bankAccount.accountID}",
                    amount = (decimal)balance,
                    employee = User.Identity.GetUserName()
                };
                db.TransactionHistory.Add(transaction);
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("userAccounts", new { id = bankAccount.customerID });
            }
            return View(bankAccount);
        }

        public ActionResult Close(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bankAccount bankAccount = db.Accounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Close(int id)
        {
            bankAccount bankAccount = db.Accounts.Find(id);
            bankAccount.close();
            if (ModelState.IsValid)
            {
                TransHist transaction = new TransHist()
                {
                    date = DateTime.Now,
                    customerID = bankAccount.customerID,
                    type = $"Closure -{bankAccount.accountID}",
                    amount = (decimal)0,
                    employee = User.Identity.GetUserName()
                };
                db.TransactionHistory.Add(transaction);
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("userAccounts", new { id = bankAccount.customerID });
            }
            return View(bankAccount);

        }

        public ActionResult Transfer(int? id)
        {
            var accounts = from a in db.Accounts.Include(a => a.customer)
                           where a.customerID == id && !a.locked
                           select a;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bankAccount bankAccount = db.Accounts.Find(id);
            ViewBag.customerID = id;
            return View(bankAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer(int id, int id2, decimal amount)
        {
            bankAccount bankAccount = db.Accounts.Find(id);
            bankAccount bankAccount2 = db.Accounts.Find(id2);
            bankAccount.balance -= amount;
            bankAccount2.balance += amount;
            if (ModelState.IsValid)
            {
                TransHist transaction = new TransHist()
                {
                    date = DateTime.Now,
                    customerID = bankAccount.customerID,
                    type = $"Transfer -{bankAccount.accountID} > {bankAccount2.accountID}",
                    amount = (decimal)amount,
                    employee = User.Identity.GetUserName()
                };
                db.TransactionHistory.Add(transaction);
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("userAccounts", new { id = bankAccount.customerID });
            }
            return View(bankAccount);

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
