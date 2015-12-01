﻿using System.Linq;
using System.Net;
using System.Web.Mvc;
using APIS.Models;

namespace APIS.Controllers
{
    public class CustomerTeamsController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        // GET: CustomerTeams
        public ActionResult Index()
        {
            return View(db.CustomerTeams.ToList());
        }

        // GET: CustomerTeams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerTeams/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Name,IsDelete")] CustomerTeam customerTeam)
        {
            if (ModelState.IsValid)
            {
                db.CustomerTeams.Add(customerTeam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerTeam);
        }

        // GET: CustomerTeams/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTeam customerTeam = db.CustomerTeams.Find(id);
            if (customerTeam == null)
            {
                return HttpNotFound();
            }
            return View(customerTeam);
        }

        // POST: CustomerTeams/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Name,IsDelete")] CustomerTeam customerTeam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerTeam).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerTeam);
        }


        #region 刪除頁
        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTeam customerTeam = db.CustomerTeams.Where(m => m.ID == id).FirstOrDefault();
            if (customerTeam != null)
            {
                customerTeam.IsDelete = true; //假刪
                if (ModelState.IsValid)
                {
                    db.Entry(customerTeam).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(customerTeam);
        }

        #endregion

        #region 回收連線資源
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        } 
        #endregion
    }
}
