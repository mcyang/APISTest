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

        #region 列表頁
        // GET: RDRModules
        public ActionResult Index()
        {
            return View(db.RDRModules.ToList());
        } 
        #endregion


        #region 明細頁
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
        #endregion


        #region 新增頁
        // GET: RDRModules/Create
        /// <summary>
        /// 新增機種
        /// </summary>
        /// <param name="id">RDRMain.ID</param>
        /// <param name="code">RDRMain.RDRNumber</param>
        /// <param name="startYear">SOPDate年</param>
        /// <param name="EndYear">EOLDate年</param>
        /// <returns></returns>
        public ActionResult Create(int id = 1, string code = "AL.15.0001", int startYear = 2016, int endYear = 2018)
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
        public ActionResult Create(RDRModuleViewModel viewModel, FormCollection fc)
        {
            RDRModule rdrModule = new RDRModule();

            //int fid = 0;
            //int.TryParse(fc["ParentID"], out fid);
            //rdrModule.ParentID = fid;
            rdrModule.ParentID = viewModel.ParentID;
            
            //rdrModule.RDRNumber = "xxyy123"; //假資料

            //rdrModule.ModuleName = fc["ModuleName"];
            rdrModule.ModuleName = viewModel.ModuleName;

            int pid = 0;
            int.TryParse(fc["ProductGroupsList"], out pid);
            rdrModule.ProductGroupID = pid; //產品別ID
            string productCode = db.ProductGroups.Find(pid).Code.Substring(8, 2);   //產品別代碼2碼
            
            int cid = 0;
            int.TryParse(fc["CustomersList"], out cid);
            rdrModule.CustomerID = cid; //交貨地

            rdrModule.EstimateProduct = fc["EstimateProduct"];
            rdrModule.Attachment = fc["Attachment"];  //附件
            //rdrModule.Remark = fc["Remark"];  //Remark
            rdrModule.Remark = viewModel.Remark;  //Remark
            rdrModule.ModuleVersion = 0;    //版本號 預設值0
            rdrModule.CreateTime = System.DateTime.Now;

            //自動編碼原則: 繼承RDR單號+產品別2碼+交貨地數目1碼
            //問題: 交貨地數目需要所有機種蒐集完才有辦法算
            string str_rdr = viewModel.MainCode + "-" + productCode;

            if (ModelState.IsValid)
            {
                db.RDRModules.Add(rdrModule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
        #endregion


        #region 編輯頁
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

        #endregion


        #region 刪除頁
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
