using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APISTest.Models;

namespace APISTest.Controllers
{
    public class RDRsController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        // GET: RDRs
        public ActionResult Index()
        {
            return View(db.RDRs.ToList());
        }

        // GET: RDRs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDR rDR = db.RDRs.Find(id);
            if (rDR == null)
            {
                return HttpNotFound();
            }
            return View(rDR);
        }

        // GET: RDRs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RDRs/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RDRNumber,CustomerTeamID,CustomerTeamCode,Site,IsDelete")] RDR rDR)
        {
            if (ModelState.IsValid)
            {
                db.RDRs.Add(rDR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rDR);
        }

        // GET: RDRs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDR rDR = db.RDRs.Find(id);
            if (rDR == null)
            {
                return HttpNotFound();
            }
            return View(rDR);
        }

        // POST: RDRs/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RDRNumber,CustomerTeamID,CustomerTeamCode,Site,IsDelete")] RDR rDR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rDR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rDR);
        }

        // GET: RDRs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDR rDR = db.RDRs.Find(id);
            if (rDR == null)
            {
                return HttpNotFound();
            }
            return View(rDR);
        }

        // POST: RDRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RDR rDR = db.RDRs.Find(id);
            db.RDRs.Remove(rDR);
            db.SaveChanges();
            return RedirectToAction("Index");
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
