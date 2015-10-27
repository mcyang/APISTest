using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APISTest.Models;
using APISTest.ViewModels;

namespace APISTest.Controllers
{
    public class RDRModulesController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        // GET: RDRModules
        public ActionResult Index()
        {
            return View(db.RDRModules.ToList());
        }

        // GET: RDRModules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDRModule rDRModule = db.RDRModules.Find(id);
            if (rDRModule == null)
            {
                return HttpNotFound();
            }
            return View(rDRModule);
        }

        // GET: RDRModules/Create
        /// <summary>
        /// 新增機種
        /// </summary>
        /// <param name="id">RDRMain.ID</param>
        /// <param name="code">RDRMain.RDRNumber</param>
        /// <param name="startYear">SOPDate年</param>
        /// <param name="EndYear">EOLDate年</param>
        /// <returns></returns>
        public ActionResult Create(int id = 1, string code= "AL.15.0001", int startYear=2016, int endYear=2020)
        {
            RDRModuleViewModel viewModel = new RDRModuleViewModel();
            viewModel.ParentID = id;
            viewModel.MainCode = code;
            viewModel.StartYear = startYear;
            viewModel.EndYear = endYear;

            return View(viewModel);
        }

        // POST: RDRModules/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ParentID,RDRNumber,ModuleName,ProductGroupID,CustomerID,EstimateProduct,Attachment,Remark,ModuleVersion,CreateTime")] RDRModule rDRModule)
        {
            if (ModelState.IsValid)
            {
                db.RDRModules.Add(rDRModule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rDRModule);
        }

        // GET: RDRModules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDRModule rDRModule = db.RDRModules.Find(id);
            if (rDRModule == null)
            {
                return HttpNotFound();
            }
            return View(rDRModule);
        }

        // POST: RDRModules/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ParentID,RDRNumber,ModuleName,ProductGroupID,CustomerID,EstimateProduct,Attachment,Remark,ModuleVersion,CreateTime")] RDRModule rDRModule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rDRModule).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rDRModule);
        }

        // GET: RDRModules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDRModule rDRModule = db.RDRModules.Find(id);
            if (rDRModule == null)
            {
                return HttpNotFound();
            }
            return View(rDRModule);
        }

        // POST: RDRModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RDRModule rDRModule = db.RDRModules.Find(id);
            db.RDRModules.Remove(rDRModule);
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
