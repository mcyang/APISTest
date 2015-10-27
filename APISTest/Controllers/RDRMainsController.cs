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
using APISTest.Helpers;

namespace APISTest.Controllers
{
    public class RDRMainsController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        #region 列表頁
        // GET: RDRMains
        public ActionResult Index()
        {
            var aa = from rdrMain in db.RDRMains
                     join car in db.CarMakers
                     on rdrMain.CarMakerID equals car.ID
                     select new { rdrMain , car.Name};

            List<RDRMainIndexViewModel> list = new List<RDRMainIndexViewModel>();
            foreach (var item in aa)
            {
                RDRMainIndexViewModel viewModel = new RDRMainIndexViewModel();
                viewModel.rdrMain = item.rdrMain;
                viewModel.CarMaker = item.Name;
                list.Add(viewModel);
            }

            return View(list);
        }
        #endregion


        #region 明細頁
        // GET: RDRMains/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDRMain rDRMain = db.RDRMains.Find(id);
            if (rDRMain == null)
            {
                return HttpNotFound();
            }
            return View(rDRMain);
        } 
        #endregion


        #region 新增頁
        // GET: RDRMains/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RDRMains/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection fc)
        {
            #region 前端data >> FormCollection >> RDRMain
            RDRMain rdrMain = new RDRMain();
            
            int teamID = 0;
            int.TryParse(fc["CustomerTeamList"], out teamID);
            rdrMain.CustomerTeamID = teamID; //客戶群ID
            string teamCode = fc["ddl_CustomerTeamText"]; //客戶群代碼
            rdrMain.CustomerTeamCode = teamCode.Trim();
            
            //自動編號原則: 客戶群2碼.系統年2碼.流水號4碼
            rdrMain.RDRNumber = CommonHelps.CreateRDRNumber(System.DateTime.Now.Year, teamCode.Trim()); //RDRNumber(系統自動編號)
            rdrMain.ProjectName = fc["rdrMain.ProjectName"]; //專案名稱
            rdrMain.LOBID = 1; //LOB
            rdrMain.Site = fc["ddl_SiteText"]; //量產地

            int carMakerID = 0;
            int.TryParse(fc["CarMakerList"], out carMakerID);
            rdrMain.CarMakerID = carMakerID; //車廠
            rdrMain.CarModel = fc["rdrMain.CarModel"]; //車型

            int sales = 0;
            int.TryParse(fc["rdrMain.EstimateSales"], out sales);
            rdrMain.EstimateSales = sales; //預估年銷售量

            rdrMain.RFQDate = Convert.ToDateTime(fc["RFQDate"]);   //RFQDate
            rdrMain.AcquisitionDate = Convert.ToDateTime(fc["AcquisitionDate"]);   //報價需求日
            rdrMain.SOPDate = Convert.ToDateTime(fc["SOPDate"]);    //SOPDate
            rdrMain.EOLDate = Convert.ToDateTime(fc["EOLDate"]);    //EOLDate
            rdrMain.Certainty = fc["ddl_CertaintyText"];    //把握度
            rdrMain.RequestClass = fc["ddl_QuotationText"]; //報價類型
            rdrMain.RequestType = "DDP"; //報價方式
            rdrMain.Currency = fc["ddl_CurrencyText"]; //報價貨幣

            int revenue = 0;
            int.TryParse(fc["EstimateRevenueValue"], out revenue);
            rdrMain.EstimateRevenue = revenue;  //預估年營業額
            rdrMain.CreateTime = System.DateTime.Now;

            #endregion

            #region 新增
            if (ModelState.IsValid)
            {
                db.RDRMains.Add(rdrMain);
                db.SaveChanges();
                return RedirectToAction("Index");
            } 
            #endregion

            return View();
        }

        #endregion


        #region 編輯頁
        // GET: RDRMains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDRMain rDRMain = db.RDRMains.Find(id);
            if (rDRMain == null)
            {
                return HttpNotFound();
            }

            //轉換為 ViewModel 資料型態傳入
            RDRMainViewModel viewModel = new RDRMainViewModel();
            viewModel.rdrMain = rDRMain;

            return View(viewModel);
        }

        // POST: RDRMains/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection fc)
        {
            #region 前端data >> FormCollection >> RDRMain

            RDRMain rdrMain = db.RDRMains.Find(id);
            
            int teamID = 0;
            int.TryParse(fc["CustomerTeamList"], out teamID);
            rdrMain.CustomerTeamID = teamID; //客戶群ID
            rdrMain.CustomerTeamCode = fc["ddl_CustomerTeamText"]; //客戶群代碼

            rdrMain.ProjectName = fc["rdrMain.ProjectName"]; //專案名稱
            rdrMain.Site = fc["ddl_SiteText"]; //量產地

            int carMakerID = 0;
            int.TryParse(fc["CarMakerList"], out carMakerID);
            rdrMain.CarMakerID = carMakerID; //車廠
            rdrMain.CarModel = fc["rdrMain.CarModel"]; //車型

            int sales = 0;
            int.TryParse(fc["rdrMain.EstimateSales"], out sales);
            rdrMain.EstimateSales = sales; //預估年銷售量 

            rdrMain.RFQDate = Convert.ToDateTime(fc["RFQDate"]);   //RFQDate
            rdrMain.AcquisitionDate = Convert.ToDateTime(fc["AcquisitionDate"]);   //報價需求日
            rdrMain.SOPDate = Convert.ToDateTime(fc["SOPDate"]);    //SOPDate
            rdrMain.EOLDate = Convert.ToDateTime(fc["EOLDate"]);    //EOLDate
            rdrMain.Certainty = fc["ddl_CertaintyText"];    //把握度
            rdrMain.RequestClass = fc["ddl_QuotationText"]; //報價類型
            rdrMain.Currency = fc["ddl_CurrencyText"]; //報價貨幣

            int revenue = 0;
            int.TryParse(fc["EstimateRevenueValue"], out revenue);
            rdrMain.EstimateRevenue = revenue;  //預估年營業額
            #endregion

            #region 更新
            if (ModelState.IsValid)
            {
                db.Entry(rdrMain).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            } 
            #endregion

            RDRMain rDRMain = db.RDRMains.Find(id);
            if (rDRMain == null)
            {
                return HttpNotFound();
            }

            //轉換為 ViewModel 資料型態傳入
            RDRMainViewModel viewModel = new RDRMainViewModel();
            viewModel.rdrMain = rDRMain;

            return View(viewModel);
        }

        #endregion


        #region 刪除頁
        // GET: RDRMains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDRMain rDRMain = db.RDRMains.Find(id);
            if (rDRMain == null)
            {
                return HttpNotFound();
            }
            return View(rDRMain);
        }

        // POST: RDRMains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RDRMain rDRMain = db.RDRMains.Find(id);
            db.RDRMains.Remove(rDRMain);
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
