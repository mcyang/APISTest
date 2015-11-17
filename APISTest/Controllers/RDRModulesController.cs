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
        List<RDRModuleViewModel> AddModuleList = new List<RDRModuleViewModel>();

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
        /// <param name="id">RDRMain.ID (RDR主表ID)</param>
        /// <returns></returns>
        public ActionResult Create(int id)
        {
            //Search By RDRMain.ID
            RDRMain main = db.RDRMains.Find(id);

            RDRModuleViewModel viewModel = new RDRModuleViewModel();
            viewModel.ParentID = main.ID;
            viewModel.MainCode = main.RDRNumber;
            viewModel.StartYear = main.SOPDate.Year;
            viewModel.EndYear = main.EOLDate.Year;

            return View(viewModel);
        }

        // POST: RDRModules/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RDRModuleViewModel viewModel, FormCollection fc)
        {
            //現在做法是將Module新增到 db.RDRModuleTemp 裡
            //User確認清單內容無誤後，按儲存才全部打包存進 db.RDRModule
            RDRModuleTemp rdrModuleTemp = new RDRModuleTemp();      //RDRModule的暫存表
            rdrModuleTemp.ParentID = viewModel.ParentID;
            rdrModuleTemp.RDRNumber = viewModel.MainCode;
            rdrModuleTemp.ModuleName = viewModel.ModuleName;

            int pid = 0;
            int.TryParse(fc["ProductGroupsList"], out pid);
            rdrModuleTemp.ProductGroupID = pid;                     //產品別ID
            string productCode = db.ProductGroups.Find(pid).Code;   //產品別代碼(全碼)
            string productShortCode = productCode.Substring(8, 2);  //產品別代碼(後2碼)
            string productName = db.ProductGroups.Find(pid).Name;   //產品別名稱
            rdrModuleTemp.ProductGroupCode = productCode;
            rdrModuleTemp.ProductGroupName = productName;

            int cid = 0;
            int.TryParse(fc["CustomersList"], out cid);
            rdrModuleTemp.CustomerID = cid;                         //交貨地ID
            string customerName = db.Customers.Find(cid).Name;      //交貨地名稱
            rdrModuleTemp.CustomerName = customerName;

            rdrModuleTemp.EstimateProduct = fc["EstimateProduct"];
            rdrModuleTemp.Attachment = fc["Attachment"];  //附件
            rdrModuleTemp.Remark = viewModel.Remark;  //Remark
            rdrModuleTemp.ModuleVersion = 0;    //版本號 預設值0
            rdrModuleTemp.CreateTime = System.DateTime.Now;

            db.RDRModuleTemps.Add(rdrModuleTemp);
            db.SaveChanges();

            //自動編碼原則: 繼承RDR單號+產品別2碼+交貨地數目1碼
            //問題: 交貨地數目需要所有機種蒐集完才有辦法算
            //string str_rdr = viewModel.MainCode + "-" + productCode;
            //AddModuleList.Add(viewModel);
            
            return RedirectToAction("Create", 16); //new { id = viewModel.ParentID }
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

        public ActionResult Main()
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult _MyPartialView()
        //{
        //    return PartialView();
        //    //return PartialView(db.RDRModuleTemps.ToList());
        //}
    }
}
