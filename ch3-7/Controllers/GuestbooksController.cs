using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using ch3_7.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace ch3_7.Controllers
{
    [Authorize]
    public class GuestbooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Guestbooks
        public ActionResult Index(String Id)
        {
            int counts = db.GuestbookModels.ToList().Count();
            //ViewData["counts"] = counts;
            ViewBag.Counts = counts;
            TempData["counts"] = counts;
            if (Id == null)
            {
                return PartialView(db.GuestbookModels.OrderByDescending(p => p.Created).ToList());
            }
            else
            { 
                var username = User.Identity.Name;
                var user = db.Users.Where(p => p.UserName == username).FirstOrDefault();
                return PartialView(db.GuestbookModels.Where(p => p.User.Id == user.Id).ToList());
            }
            
           
        }

        public ActionResult Info()
        {
            var user = db.Users.Where(p => p.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.GuestBookCount = user.Guestbooks.Count();
            return View();
        }

        // GET: Guestbooks/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestbookModels guestbookModels = db.GuestbookModels.Find(id);
            if (guestbookModels == null)
            {
                return HttpNotFound();
            }
            return View(guestbookModels);
        }

        // GET: Guestbooks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Guestbooks/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content,Created")] GuestbookModels guestbookModels)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.Name;
                var userid = User.Identity.GetUserId();
                guestbookModels.Id = Guid.NewGuid();
                guestbookModels.Created = DateTime.Now;
                guestbookModels.User = db.Users.Where(p => p.Id == userid).FirstOrDefault();
                db.GuestbookModels.Add(guestbookModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guestbookModels);
        }

        // GET: Guestbooks/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestbookModels guestbookModels = db.GuestbookModels.Find(id);
            if (guestbookModels == null)
            {
                return HttpNotFound();
            }
            return View(guestbookModels);
        }

        // POST: Guestbooks/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content,Created")] GuestbookModels guestbookModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guestbookModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            return View(guestbookModels);
        }

        // GET: Guestbooks/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestbookModels guestbookModels = db.GuestbookModels.Find(id);
            if (guestbookModels == null)
            {
                return HttpNotFound();
            }
            return View(guestbookModels);
        }

        // POST: Guestbooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            GuestbookModels guestbookModels = db.GuestbookModels.Find(id);
            db.GuestbookModels.Remove(guestbookModels);
            db.SaveChanges();
            return RedirectToAction("Create");
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
