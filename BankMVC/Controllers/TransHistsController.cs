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

namespace BankMVC.Controllers
{
    public class TransHistsController : Controller
    {
        private BankingContext db = new BankingContext();

        // GET: TransHists
        public ActionResult Index()
        {
            var transactionHistory = db.TransactionHistory.Include(t => t.customer);
            return View(transactionHistory.ToList());
        }

        public ActionResult Sorted(DateTime date1, DateTime date2)
        {
            var transactionHistory = from a in db.TransactionHistory.Include(t => t.customer)
                                     where a.date >= date1 && a.date <= date2
                                     select a; ;
            return View(transactionHistory.ToList());
        }

        // GET: TransHists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransHist transHist = db.TransactionHistory.Find(id);
            if (transHist == null)
            {
                return HttpNotFound();
            }
            return View(transHist);
        }

        // GET: TransHists/Create
        public ActionResult Create()
        {
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "fName");
            return View();
        }

        // POST: TransHists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransID,date,customerID,type,amount,employee")] TransHist transHist)
        {
            if (ModelState.IsValid)
            {
                db.TransactionHistory.Add(transHist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customerID = new SelectList(db.Customers, "customerID", "fName", transHist.customerID);
            return View(transHist);
        }

        // GET: TransHists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransHist transHist = db.TransactionHistory.Find(id);
            if (transHist == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "fName", transHist.customerID);
            return View(transHist);
        }

        // POST: TransHists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransID,date,customerID,type,amount,employee")] TransHist transHist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transHist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerID = new SelectList(db.Customers, "customerID", "fName", transHist.customerID);
            return View(transHist);
        }

        // GET: TransHists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransHist transHist = db.TransactionHistory.Find(id);
            if (transHist == null)
            {
                return HttpNotFound();
            }
            return View(transHist);
        }

        // POST: TransHists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TransHist transHist = db.TransactionHistory.Find(id);
            db.TransactionHistory.Remove(transHist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult userTrans(int? id)
        {
            ViewBag.customerID = id;
            var accounts = (from a in db.TransactionHistory.Include(a => a.customer)
                           where a.customerID == id
                           select a).OrderByDescending(a => a.date).Take(10);
            return View(accounts.ToList());
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
