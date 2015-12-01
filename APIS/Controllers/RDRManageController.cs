using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using APIS.Models;
using APIS.ViewModels;
using PagedList;


namespace APIS.Controllers
{
    public class RDRManageController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        private const int pageSize = 5; // 設定分頁一頁5筆資料

        #region RDR表單管理 - 列表頁
        // Join RDRMain、RDRModule、RDRInfo三張表
        /// <summary>
        /// RDR表單管理 - 列表頁
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public ActionResult Index(FormCollection fc, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page; // 當前頁

            var data = (from main in db.RDRMains.Where(m => m.IsDelete == false)
                                                join car in db.CarMakers
                                                on main.CarMakerID equals car.ID
                                                select new RDRMainIndexViewModel
                                                {
                                                    rdrMain = main,
                                                    CarMaker = car.Name
                                                }).OrderBy(m=>m.rdrMain.ID); // 一定要先 OrderBy，否則分頁處理會報錯

            #region 搜尋條件
            // Search By RDRNumber
            string byRDRNo = fc["serach_RDRNo"] == null ? "" : fc["serach_RDRNo"].Trim();
            if (!string.IsNullOrEmpty(byRDRNo))
            {
                data = data.Where(m => m.rdrMain.RDRNumber.Contains(byRDRNo)).OrderBy(m=>m.rdrMain.ID);
            }

            // Search By CustomerTeam
            string byCustomerTeam = fc["search_CustomerTeam"] == null ? "" : fc["search_CustomerTeam"].Trim();
            if (!string.IsNullOrEmpty(byCustomerTeam))
            {
                int cid = 0;
                int.TryParse(fc["search_CustomerTeam"], out cid);
                data = data.Where(m => m.rdrMain.CustomerTeamID == cid).OrderBy(m=>m.rdrMain.ID);
            }

            // Search By Site
            string bySite = fc["search_Site"] == null ? "" : fc["search_Site"].Trim();
            if (!string.IsNullOrEmpty(bySite))
            {
                int sid = 0;
                int.TryParse(bySite, out sid);
                string site = APIS.Tools.EnumMapTool.GetDescription((EnumSite)sid);
                data = data.Where(m => m.rdrMain.Site == site).OrderBy(m=>m.rdrMain.ID);
            }

            // Search By CarMaker
            string byCarMaker = fc["search_CarMaker"] == null ? "" : fc["search_CarMaker"].Trim();
            if (!string.IsNullOrEmpty(fc["search_CarMaker"]))
            {
                int carid = 0;
                int.TryParse(byCarMaker, out carid);
                data = data.Where(m => m.rdrMain.CarMakerID == carid).OrderBy(m=>m.rdrMain.ID);
            }

            // Search By Product
            string byProduct = fc["search_Product"] == null ? "" : fc["search_Product"].Trim();
            if (!string.IsNullOrEmpty(byProduct))
            {
                int pid = 0;
                int.TryParse(byProduct, out pid);
                var idList = db.RDRModules.Where(m => m.ProductGroupID == pid).Select(p=>p.ParentID).Distinct();

                data = data.Where(m=> idList.Contains(m.rdrMain.ID)).OrderBy(m => m.rdrMain.ID);
            }

            // Search By RFQDate
            string byRFQDate = fc["search_RFQDate"] == null ? "" : fc["search_RFQDate"].Trim();
            if (!string.IsNullOrEmpty(byRFQDate))
            {
                DateTime dtRFQ = Convert.ToDateTime(byRFQDate);
                data = data.Where(m => m.rdrMain.RFQDate == dtRFQ).OrderBy(m => m.rdrMain.ID);
            }

            // Search By SOPDate EOLDate
            bool HasSOP = string.IsNullOrEmpty(fc["search_SOPDate"]);
            if (!HasSOP)
            {
                DateTime dtSOP = Convert.ToDateTime(fc["search_SOPDate"]);
                data = data.Where(m => m.rdrMain.SOPDate <= dtSOP).OrderBy(m => m.rdrMain.ID);
            }
            bool HasEOL = string.IsNullOrEmpty(fc["search_EOLDate"]);
            if (!HasEOL)
            {
                DateTime dtEOL = Convert.ToDateTime(fc["search_EOLDate"]);
                data = data.Where(m => m.rdrMain.EOLDate >= dtEOL).OrderBy(m => m.rdrMain.ID); ;
            }
            #endregion

            var result = data.ToPagedList(currentPage, pageSize);

            return View(result);
        }
        #endregion

        #region RDR表單管理 - 明細頁
        /// <summary>
        /// RDR表單管理 - 明細頁
        /// </summary>
        /// <param name="id">RDR表單ID</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            //找不到!! 拋出例外
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //建立 RDR表單ViewModel
            RDRViewModel viewModel = new RDRViewModel();

            //撈RDRMain & 丟到RDR主表明細ViewModel
            viewModel.rdrMainDetail = (from main in db.RDRMains.Where(m => m.ID == id)
                                       join car in db.CarMakers on main.CarMakerID equals car.ID
                                       select new RDRMainDetailsViewModel
                                       {
                                           rdrMain = main,
                                           CarMaker = car.Name
                                       }).SingleOrDefault();

            //撈RDRModule & 丟到RDR機種資料明細ViewModel
            viewModel.rdrModuleList = (from m in db.RDRModules
                                       join n in db.ProductGroups on m.ProductGroupID equals n.ID
                                       join p in db.Customers on m.CustomerID equals p.ID
                                       where m.ParentID == id
                                       select new RDRModuleDetailsViewModel
                                       {
                                           ID = m.ID,
                                           ParentID = m.ParentID,
                                           RDRNumber = m.RDRNumber,
                                           ModuleName = m.ModuleName,
                                           ProductGroupName = n.Name,
                                           CustomerName = p.Name,
                                           Attachment = m.Attachment,
                                           Remark = m.Remark,
                                           EstimateProduct = m.EstimateProduct,
                                           CreateTime = m.CreateTime
                                       }).ToList();

            //撈RDRInfo & 丟到RDR其他資訊明細ViewModel
            viewModel.rdrInfoDetail = (from info in db.RDRInformations
                                       join main in db.RDRMains on info.ParentID equals main.ID
                                       where info.ParentID == id
                                       select new RDRInfoViewModel
                                       {
                                           rdrInfo = info,
                                       }).FirstOrDefault();

            return View(viewModel);
        }
        #endregion

        #region RDR表單管理 - 刪除頁
        /// <summary>
        /// RDR表單管理 - 刪除頁
        /// </summary>
        /// <param name="id">RDR表單ID</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            //1.找出該筆資料
            RDRMain item = db.RDRMains.Find(id);
            if (item != null)
            {
                //找出對應的RDRModules & 刪除
                List<RDRModule> moduleList = db.RDRModules.Where(m => m.ParentID == item.ID).ToList();
                if (moduleList.Count > 0)
                {
                    foreach (var module in moduleList)
                    {
                        module.IsDelete = true; //假刪
                        //db.RDRModules.Remove(module); //真刪
                        db.SaveChanges();
                    }
                }

                //找出對應的RDRInformation & 刪除
                RDRInformation rdrInfo = db.RDRInformations.Where(m => m.ParentID == item.ID).FirstOrDefault();
                if (rdrInfo != null)
                {
                    rdrInfo.IsDelete = true; //假刪
                    //db.RDRInformations.Remove(rdrInfo); //真刪
                    db.SaveChanges();
                }

                //2.設定假刪(IsDelete=true)
                item.IsDelete = true;

                //3.資料表Save
                db.SaveChanges();
            }

            //4.重新導向回列表頁
            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult BrowseReport(int id, string format = "pdf")
        {
            //SET DATA TO RDLC
            List<RDRMainReportViewModel> mainList = (from main in db.RDRMains.Where(m => m.ID == id)
                                                     join car in db.CarMakers on main.CarMakerID equals car.ID
                                                     select new RDRMainReportViewModel { rdrMain = main, CarMakerName = car.Name }).ToList();


            List<RDRModuleReportViewModel> modulesList = (from module in db.RDRModules
                                                          join customer in db.Customers on module.CustomerID equals customer.ID
                                                          join productGroup in db.ProductGroups on module.ProductGroupID equals productGroup.ID
                                                          join main in db.RDRMains.Where(m => m.ID == id) on module.ParentID equals main.ID
                                                          select new RDRModuleReportViewModel { rdrModule = module, CustomerName = customer.Name, ProductGroupName = productGroup.Name }).ToList();


            List<RDRInfoReportViewModel> infoList = (from info in db.RDRInformations.Where(x => x.ParentID == id)
                                                     select new RDRInfoReportViewModel { rdrInfo = info }).ToList();

            try
            {
                LocalReport localReport = new LocalReport();
                localReport.ReportPath = Server.MapPath("~/Reports/RdrReport/RDR.rdlc");

                // 資料來源加進去
                // ReportDataSource("對應rdlc的資料來源名稱", 資料集合)
                localReport.DataSources.Add(new ReportDataSource("DS_RDRMain", mainList));
                localReport.DataSources.Add(new ReportDataSource("DS_RDRModule", modulesList));
                localReport.DataSources.Add(new ReportDataSource("DS_RDRInfo", infoList));

                // 定義報表格式, ex.檔案類型、輸出格式(A4、邊界...)
                string reportType = ReportType.PDF.ToString();
                string mimeType;
                string encoding;
                string fileNameExtension;

                //The DeviceInfo settings should be changed based on the reportType
                //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
                string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>" +
                "  <PageWidth>8.27in</PageWidth>" +
                "  <PageHeight>11.69in</PageHeight>" +
                "  <MarginTop>0.3937in</MarginTop>" +
                "  <MarginLeft>0.3937in</MarginLeft>" +
                "  <MarginRight>0.3937in</MarginRight>" +
                "  <MarginBottom>0.3937in</MarginBottom>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                //Render the report
                renderedBytes = localReport.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                // 以匯出檔案串流方式輸出
                return File(renderedBytes, mimeType, string.Format("{0}.{1}", Server.UrlPathEncode("RDR"), fileNameExtension));
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                return Content("下載失敗!");
            }
        }

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