using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APIS.Models;

namespace APIS.Controllers
{
    public class ProductGroupsController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        #region 列表頁
        // GET: ProductGroups
        public ActionResult Index()
        {
            return View(db.ProductGroups.ToList());
        }
        #endregion


        #region 明細頁
        // GET: ProductGroups/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = db.ProductGroups.Find(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }
        #endregion


        #region 新增頁
        // GET: ProductGroups/Create
        public ActionResult Create()
        {
            return View();
        } 

        // POST: ProductGroups/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Name,IsDelete")] ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {
                db.ProductGroups.Add(productGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productGroup);
        }
        #endregion


        #region 編輯頁
        // GET: ProductGroups/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = db.ProductGroups.Find(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }

        // POST: ProductGroups/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Name,IsDelete")] ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productGroup).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productGroup);
        }
        #endregion


        #region 刪除頁
        // GET: ProductGroups/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = db.ProductGroups.Find(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }

        // POST: ProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            ProductGroup productGroup = db.ProductGroups.Find(id);
            db.ProductGroups.Remove(productGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
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
