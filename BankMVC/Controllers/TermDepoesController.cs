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
    public class TermDeposController : Controller
    {
        private BankingContext db = new BankingContext();

        // GET: TermDepoes
        public ActionResult Index()
        {
            return View(db.TermDeposit.ToList());
        }

        // GET: TermDepoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermDepo termDepo = db.TermDeposit.Find(id);
            if (termDepo == null)
            {
                return HttpNotFound();
            }
            return View(termDepo);
        }

        // GET: TermDepoes/Create
        public ActionResult Create(int? id)
        {
            var term = new TermDepo();
            term.customerID = (int)id;
            term.percents = new[]
       {
            new SelectListItem { Value = "0.15", Text = "3 Months - 5%" },
            new SelectListItem { Value = "0.20", Text = "6 Months - 10%" },
            new SelectListItem { Value = "0.25", Text = "12 Months - 15%" }
        };
            return View(term);
        }

        // POST: TermDepoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "termID,amount,interest,duration,output,customerID,message")] TermDepo termDepo)
        {
            termDepo.amount += (termDepo.amount * termDepo.interest);
            if (ModelState.IsValid)
            {
                TransHist transaction = new TransHist()
                {
                    date = DateTime.Now,
                    customerID = termDepo.customerID,
                    type = $"Term Deposit - {termDepo.termID}",
                    amount = termDepo.amount,
                    employee = User.Identity.GetUserName()
                };
                db.TransactionHistory.Add(transaction);
                db.TermDeposit.Add(termDepo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(termDepo);
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
