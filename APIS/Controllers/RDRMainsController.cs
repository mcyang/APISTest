using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using APIS.Helpers;
using APIS.Models;
using APIS.ViewModels;


namespace APIS.Controllers
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
            RDRMain rdrmain = db.RDRMains.Find(id);
            if (rdrmain == null)
            {
                return HttpNotFound();
            }

            #region 轉為ViewModel
            RDRMainDetailsViewModel viewModel = new RDRMainDetailsViewModel();
            viewModel.rdrMain = rdrmain;
            viewModel.CarMaker = db.CarMakers.SingleOrDefault(p => p.ID == rdrmain.CarMakerID).Name;
            #endregion

            return View(viewModel);
        } 
        #endregion


        #region 新增頁
        // GET: RDRMains/Create
        public ActionResult Create()
        {
            //待處理
            //RFQ Date 預設系統日期
            //AcquisitionDate 預設系統日期+7天
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
            rdrMain.ProjectName = fc["ProjectName"]; //專案名稱
            rdrMain.LOB = fc["ddl_LOBText"]; //LOB
            rdrMain.Site = fc["ddl_SiteText"]; //量產地

            int carMakerID = 0;
            int.TryParse(fc["CarMakerList"], out carMakerID);
            rdrMain.CarMakerID = carMakerID; //車廠
            rdrMain.CarModel = fc["CarModel"]; //車型

            int sales = 0;
            int.TryParse(fc["EstimateSales"], out sales);
            rdrMain.EstimateSales = sales; //預估年銷售量

            rdrMain.RFQDate = Convert.ToDateTime(fc["RFQDate"]);   //RFQDate
            rdrMain.AcquisitionDate = Convert.ToDateTime(fc["AcquisitionDate"]);   //報價需求日
            rdrMain.SOPDate = Convert.ToDateTime(fc["SOPDate"]);    //SOPDate
            rdrMain.EOLDate = Convert.ToDateTime(fc["EOLDate"]);    //EOLDate
            rdrMain.Certainty = fc["ddl_CertaintyText"];    //把握度
            rdrMain.RequestClass = fc["ddl_QuotationText"]; //報價類型
            rdrMain.RequestType = fc["ddl_RequestTypeText"]; //報價方式
            rdrMain.Currency = fc["ddl_CurrencyText"]; //報價貨幣

            int revenue = 0;
            int.TryParse(fc["EstimateRevenueValue"], out revenue);
            rdrMain.EstimateRevenue = revenue;  //預估年營業額
            rdrMain.Status = 1; // 狀態:RFQ階段
            rdrMain.CreateTime = System.DateTime.Now;
            rdrMain.ModifyTime = System.DateTime.Now;
            rdrMain.CreateUserID = fc["CreateUserID"];
            rdrMain.IsDelete = false;

            #endregion

            #region 新增
            if (ModelState.IsValid)
            {
                db.RDRMains.Add(rdrMain);
                db.SaveChanges();

                //導向RDRMains/Deatils，再從Details中選擇要新增Module還是要修改RDRMain
                
                return RedirectToAction("Details","RDRManage" , new { id = rdrMain.ID });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
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

            //轉換為 ViewModel 資料型態傳入
            RDRMainViewModel viewModel = (from main in db.RDRMains.Where(m => m.ID == id)
                                          select new RDRMainViewModel
                                          {
                                              rdrMain = main,
                                              ID = main.ID,
                                              RDRNumber = main.RDRNumber,
                                              CustomerTeamID = main.CustomerTeamID,
                                              CustomerTeamCode = main.CustomerTeamCode,
                                              CreateUserID = main.CreateUserID,
                                              ProjectName = main.ProjectName,
                                              LOB = main.LOB,
                                              Site = main.Site,
                                              CarMakerID = main.CarMakerID,
                                              CarModel = main.CarModel,
                                              EstimateSales = main.EstimateSales,
                                              Certainty = main.Certainty,
                                              RequestClass = main.RequestClass,
                                              RequestType = main.RequestType,
                                              Currency = main.Currency,
                                              EstimateRevenue = main.EstimateRevenue,
                                              Status = main.Status,
                                              CreateTime = main.CreateTime,
                                              ModifyTime = main.ModifyTime,
                                          }).FirstOrDefault();

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
            rdrMain.ProjectName = fc["ProjectName"]; //專案名稱
            //rdrMain.LOB = fc["ddl_LOBList"]; //LOB
            rdrMain.Site = fc["ddl_SiteText"]; //量產地

            int carMakerID = 0;
            int.TryParse(fc["CarMakerList"], out carMakerID);
            rdrMain.CarMakerID = carMakerID; //車廠
            rdrMain.CarModel = fc["CarModel"]; //車型

            int sales = 0;
            int.TryParse(fc["EstimateSales"], out sales);
            rdrMain.EstimateSales = sales; //預估年銷售量 

            rdrMain.RFQDate = Convert.ToDateTime(fc["RFQDate"]);   //RFQDate
            rdrMain.AcquisitionDate = Convert.ToDateTime(fc["AcquisitionDate"]);   //報價需求日
            rdrMain.SOPDate = Convert.ToDateTime(fc["SOPDate"]);    //SOPDate
            rdrMain.EOLDate = Convert.ToDateTime(fc["EOLDate"]);    //EOLDate
            rdrMain.Certainty = fc["ddl_CertaintyText"];    //把握度
            rdrMain.RequestClass = fc["ddl_QuotationText"]; //報價類型
            rdrMain.RequestType = fc["ddl_RequestTypeText"];   //報價方式
            rdrMain.Currency = fc["ddl_CurrencyText"]; //報價貨幣

            int revenue = 0;
            int.TryParse(fc["EstimateRevenueValue"], out revenue);
            rdrMain.EstimateRevenue = revenue;  //預估年營業額
            rdrMain.ModifyTime = System.DateTime.Now;
            #endregion

            #region 更新
            if (ModelState.IsValid)
            {
                db.Entry(rdrMain).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "RDRManage", new { id = rdrMain.ID });
                //若目前狀態只有RDRMain有資料 , 則導向RDRMain/Details
                //若三部分都有資料,導向RDRManage/Details
                //RDRInformation rdrinfo = db.RDRInformations.Where(m=>m.ParentID == rdrMain.ID).FirstOrDefault();

                //if (rdrinfo == null)
                //{
                //    return RedirectToAction("Details", "RDRMains", new { id = rdrMain.ID });
                //}
                //else //導向RDRMain/Details
                //{
                //    return RedirectToAction("Details", "RDRManage", new { id = rdrMain.ID });
                //}
            }

            #endregion

            //更新失敗時，停留在原頁面
            RDRMainViewModel viewModel = (from main in db.RDRMains.Where(m => m.ID == id)
                                          select new RDRMainViewModel
                                          {
                                              rdrMain = main,
                                              ID = main.ID,
                                              RDRNumber = main.RDRNumber,
                                              CustomerTeamID = main.CustomerTeamID,
                                              CustomerTeamCode = main.CustomerTeamCode,
                                              CreateUserID = main.CreateUserID,
                                              ProjectName = main.ProjectName,
                                              LOB = main.LOB,
                                              Site = main.Site,
                                              CarMakerID = main.CarMakerID,
                                              CarModel = main.CarModel,
                                              EstimateSales = main.EstimateSales,
                                              Certainty = main.Certainty,
                                              RequestClass = main.RequestClass,
                                              RequestType = main.RequestType,
                                              Currency = main.Currency,
                                              EstimateRevenue = main.EstimateRevenue,
                                              Status = main.Status,
                                              CreateTime = main.CreateTime,
                                              ModifyTime = main.ModifyTime,
                                          }).FirstOrDefault();

            return View(viewModel);
        }
        #endregion


        #region 刪除頁
        // GET: RDRMains/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDRMain rdrMain = db.RDRMains.Find(id);
            if (rdrMain != null)
            {
                db.RDRMains.Remove(rdrMain);    //真刪
                db.SaveChanges();

                return RedirectToAction("Index","RDRManage");
            }
            
            return View(rdrMain);
        }

        #endregion


        #region 瀏覽報表
        //瀏覽報表
        //透過ReportViewer.aspx 放 ReportViewer控制項,並將內容顯示於ReportViewer的方法,在遠端Server行不通
        public ActionResult BrowseReport(int id)
        {
            #region 報表資料來源

            //撈資料
            List<RDRMainReportViewModel> mainList = (from main in db.RDRMains.Where(m=>m.ID == id)
                           join car in db.CarMakers on main.CarMakerID equals car.ID
                           select new RDRMainReportViewModel { rdrMain = main, CarMakerName = car.Name }).ToList();

           
            List<RDRModuleReportViewModel> modulesList = (from module in db.RDRModules
                              join customer in db.Customers on module.CustomerID equals customer.ID
                              join productGroup in db.ProductGroups on module.ProductGroupID equals productGroup.ID
                              join main in db.RDRMains.Where(m => m.ID == id) on module.ParentID equals main.ID
                              select new RDRModuleReportViewModel { rdrModule = module, CustomerName = customer.Name, ProductGroupName = productGroup.Name }).ToList();


            List<RDRInfoReportViewModel> infoList = (from info in db.RDRInformations.Where(x => x.ParentID == id)
                           select new RDRInfoReportViewModel { rdrInfo = info }).ToList();

            #endregion

            try
            {
                // Set report info
                APIS.Reports.ReportWrapper rw = new APIS.Reports.ReportWrapper();
                rw.ReportPath = Server.MapPath("~/Reports/RdrReport/RDR.rdlc");
                rw.ReportDataSources.Add(new ReportDataSource("DS_RDRMain", mainList));
                rw.ReportDataSources.Add(new ReportDataSource("DS_RDRModule", modulesList));
                rw.ReportDataSources.Add(new ReportDataSource("DS_RDRInfo", infoList));
                //rw.ReportParameters.Add(new ReportParameter("param", list[0].EstimateProduct));
                rw.IsDownloadDirectly = false;

                // Pass report info via session
                Session["ReportWrapper"] = rw;

                // Go report viewer page
                return Redirect("/Reports/ReportViewer.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
