using APISTest.Models;
using APISTest.ViewModels;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace APISTest.Controllers
{
    public class AwardedCheckController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        #region Award-列表頁
        // GET: AwardedCheck
        public ActionResult Index()
        {
            //從RDRMain表撈出所有主表資訊(重點是Status:RFQ、Awarded、Fail)
            //列表後方僅出現 Details ,讓user從 Details Link 進入新機種開案單
            List<RDRAwardIndexViewModel> list = (from rdrMain in db.RDRMains
                                                 join car in db.CarMakers on rdrMain.CarMakerID equals car.ID
                                                 select new RDRAwardIndexViewModel
                                                 {
                                                     ID = rdrMain.ID,
                                                     RDRNumber = rdrMain.RDRNumber,
                                                     ProjectName = rdrMain.ProjectName,
                                                     CustomerTeamName = rdrMain.CustomerTeamCode,
                                                     CarMakerName = car.Name,
                                                     SOPDate = rdrMain.SOPDate,
                                                     EOLDate = rdrMain.EOLDate,
                                                     Status = rdrMain.Status
                                                 }).ToList();


            return View(list);
        }
        #endregion

        #region 填寫開案單
        [HttpGet]
        public ActionResult Kickoff(int id)
        {
            //撈RDR主表資料、RDR其他資訊(db.RDRMain join CarMaker join RDRInfomation)
            KickoffCreateViewModel viewModel = (from main in db.RDRMains
                                               join car in db.CarMakers on main.CarMakerID equals car.ID
                                               join infos in db.RDRInformations on main.ID equals infos.ParentID
                                               where main.ID == id
                                               select new KickoffCreateViewModel
                                               {
                                                   rdrMain = main,
                                                   rdrInfo = infos,
                                                   CarMakerName = car.Name
                                               }).SingleOrDefault();

            //撈RDRModule
            viewModel.rdrModuleList = db.RDRModules.Where(p => p.ParentID == id).AsEnumerable().ToList();
             
            //List<RDRModule> mo = tt.ToList<RDRModule>();

            return View(viewModel);
        }
        #endregion

        #region 瀏覽報表
        //瀏覽報表
        public ActionResult BrowseReport()
        {
            #region 報表資料來源
            //1.RDRMain
            List<RDRAwardedReportViewModel> mainList = new List<RDRAwardedReportViewModel>();

            //撈資料
            var mainData = from main in db.RDRMains.Where(m => m.ID == 1)
                           join car in db.CarMakers on main.CarMakerID equals car.ID
                           select new { main, car.Name };

            foreach (var m in mainData)
            {
                RDRAwardedReportViewModel viewModel = new RDRAwardedReportViewModel();
                viewModel.rdrMain = m.main;
                viewModel.CarMakerName = m.Name;
                mainList.Add(viewModel);
            }

            //List<RDRModuleReportViewModel> modulesList = new List<RDRModuleReportViewModel>();
            //var modulesData = from module in db.RDRModules
            //                  join customer in db.Customers on module.CustomerID equals customer.ID
            //                  join productGroup in db.ProductGroups on module.ProductGroupID equals productGroup.ID
            //                  join main in db.RDRMains.Where(m => m.ID == 1) on module.ParentID equals main.ID
            //                  select new { rdrModule = module, CustomerName = customer.Name, ProductGroupName = productGroup.Name };

            //foreach (var m in modulesData)
            //{
            //    RDRModuleReportViewModel moduleRptModel = new RDRModuleReportViewModel();
            //    moduleRptModel.rdrModule = m.rdrModule;
            //    moduleRptModel.ProductGroupName = m.ProductGroupName;
            //    moduleRptModel.CustomerName = m.CustomerName;

            //    modulesList.Add(moduleRptModel);
            //}

            List<RDRInfoReportViewModel> infoList = new List<RDRInfoReportViewModel>();
            var infoData = from info in db.RDRInformations.Where(x => x.ParentID == 1)
                           select new { info };
            foreach (var i in infoData)
            {
                RDRInfoReportViewModel infoModel = new RDRInfoReportViewModel();
                infoModel.rdrInfo = i.info;

                infoList.Add(infoModel);
            }
            #endregion

            try
            {
                // Set report info
                APISTest.Reports.ReportWrapper rw = new APISTest.Reports.ReportWrapper();
                rw.ReportPath = Server.MapPath("~/Reports/AwardedReport/AwardedRpt.rdlc");
                rw.ReportDataSources.Add(new ReportDataSource("DS_RDRMain", mainList));
                //rw.ReportDataSources.Add(new ReportDataSource("DS_RDRModule", modulesList));
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
    }
}