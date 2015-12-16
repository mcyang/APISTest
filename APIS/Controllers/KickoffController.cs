using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APIS.Models;
using APIS.ViewModels;
using Microsoft.Reporting.WebForms;

namespace APIS.Controllers
{
    /// <summary>
    /// 開案單
    /// </summary>
    public class KickoffController : BaseController
    {
        JohnTestEntities db = new JohnTestEntities();

        /// <summary>
        /// 開案單列表搜尋
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public ActionResult Index(FormCollection fc)
        {
            KickoffIndexViewModel viewModel = new KickoffIndexViewModel();

            // Search By RDRNumber
            string byRDRNo = fc["search_RDRNo"] == null ? "" : fc["search_RDRNo"].Trim();
            if (!string.IsNullOrEmpty(byRDRNo))
            {
                //查詢條件:
                //(1)RDRMain 必須 match User輸入的RDR Number記錄
                //(2)RDRMain,RDRModule,RDRInformation 三張表都要有資料
                //(3)RDRMain.Status=1 , RDRMain.IsDelete=false
                var query = (from main in db.RDRMains.Where(m => m.Status == 1 && m.IsDelete == false)
                             join module in db.RDRModules on main.ID equals module.ParentID
                             join info in db.RDRInformations on main.ID equals info.ParentID
                             where main.RDRNumber == byRDRNo
                             select new KickoffIndexViewModel {
                                 ID = main.ID,
                                 RDRNumber = main.RDRNumber,
                                 ProjectName = main.ProjectName,
                                 CustomerTeam = main.CustomerTeamCode,
                                 Status = main.Status,
                             }).Distinct().FirstOrDefault();

                if (query != null)
                {
                    return View(query);
                }
                else
                {
                    return ShowMsgThenRedirect("Error! 找不到此單號/RDR單據內容不完整，無法開案", Url.Action("Index"));
                }
            }
            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">開案單ID</param>
        /// <returns></returns>
        public ActionResult List(int id)
        {
            Kickoff kick = db.Kickoffs.FirstOrDefault(m => m.ID == id);

            KickoffListViewModel viewModel = (from k in db.Kickoffs.Where(m => m.ID == id)
                                              join main in db.RDRMains on kick.ParentID equals main.ID
                                              select new KickoffListViewModel
                                              {
                                                  ID = k.ID,
                                                  RDRNumber = k.KickoffNumber,
                                                  ProjectName = main.ProjectName,
                                              }).FirstOrDefault();
            
            return View(viewModel);
        }

        #region 新增開案單
        [HttpGet]
        public ActionResult Create(int id)
        {
            KickoffCreateViewModel viewModel = new KickoffCreateViewModel();

            //判斷是否已開案
            Kickoff ki = db.Kickoffs.FirstOrDefault(m => m.ParentID == id);
            if (ki != null)
            {
                return ShowMsgThenRedirect("此單已開案", Url.Action("Index"));
            }

            if (ki == null)
            {
                //撈RDR主表資料、RDR其他資訊(db.RDRMain join CarMaker join RDRInfomation)
                viewModel = (from main in db.RDRMains
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
                viewModel.rdrModuleList = db.RDRModules.Where(p => p.ParentID == id).ToList();
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(KickoffCreateViewModel viewModel, FormCollection fc)
        {
            bool IsModuleOK = false;
            bool IsInfoOK = false;
            bool IsKickoffOK = false;
            bool IsKickoffDetailsOK = false;
            int kickoffID=0;

            IsInfoOK = SaveToRDRInformation(viewModel);
            IsModuleOK = SaveToRDRModule(viewModel,fc);

            if (IsInfoOK && IsModuleOK)
            {
                try
                {
                    //儲存 Kickoff
                    Kickoff kick = new Kickoff();

                    kick.KickoffNumber = viewModel.rdrMain.RDRNumber;
                    kick.ParentID = viewModel.rdrMain.ID;
                    kick.KickoffDate = viewModel.kickoff.KickoffDate;
                    kick.IsReliabilityTest = viewModel.kickoff.IsReliabilityTest;
                    kick.IsPPAP = viewModel.kickoff.IsPPAP;
                    kick.IsCustomerDocForm = viewModel.kickoff.IsCustomerDocForm;
                    kick.IsLiteonDocForm = viewModel.kickoff.IsLiteonDocForm;
                    kick.IsLiteonConFirm = viewModel.kickoff.IsLiteonConFirm;
                    kick.IsCustomerConFirm = viewModel.kickoff.IsCustomerConFirm;
                    kick.CreateTime = DateTime.Now;
                    kick.ModifyTime = DateTime.Now;
                    kick.Level = viewModel.kickoff.Level;

                    db.Kickoffs.Add(kick);
                    db.SaveChanges();

                    IsKickoffOK = true;
                    kickoffID = kick.ID;
                }
                catch (Exception ex)
                {
                    IsKickoffOK = false;
                }
            }

            if (IsKickoffOK)
            {
                try
                {
                    //儲存 KickoffDetails
                    string[] AllDesignVerify = fc["DDL_DesignVerify"].Split(',');
                    string[] AllRequireDate = fc["RequireDate"].Split(',');
                    string[] AllQuantity = fc["Quantity"].Split(',');
                    string[] AllDescription = fc["Description"].Split(',');

                    for (int i = 0; i < AllDesignVerify.Length; i++)
                    {
                        KickoffDetail kickDetails = new KickoffDetail();
                        kickDetails.ParentID = kickoffID;
                        kickDetails.CreateTime = DateTime.Now;
                        kickDetails.ModifyTime = DateTime.Now;

                        int designValue = 0;
                        int.TryParse(AllDesignVerify[i], out designValue);
                        kickDetails.Stage = APIS.Tools.EnumMapTool.GetDescription((APIS.Models.EnumDesignVerify)designValue);

                        
                        kickDetails.RequireDate = Convert.ToDateTime(AllRequireDate[i]);

                        int Quantity = 0;
                        int.TryParse(AllQuantity[i], out Quantity);
                        kickDetails.Quantity = Quantity;
                        kickDetails.Description = AllDescription[i];

                        db.KickoffDetails.Add(kickDetails);
                        db.SaveChanges();
                        IsKickoffDetailsOK = true;
                    }
                    
                }
                catch (Exception ex)
                {
                    //Delete Kickoff , 保持資訊一致
                    IsKickoffDetailsOK = false;
                }
            }


            //最後如果全都完成,記得回RDRMain修改 Status=2 (Kickoff)
            if (IsModuleOK && IsInfoOK && IsKickoffOK && IsKickoffDetailsOK)
            {
                return ShowMsgThenRedirect("新增成功", Url.Action("List", "Kickoff", new { id = kickoffID }));
            }

            return View(viewModel);
        }

        #endregion

        #region 檢視開案單
        public ActionResult Details()
        {

            return View();
        }
        #endregion

        #region 修改開案單

        #endregion

        #region 刪除開案單

        #endregion

        #region 列印開案單
        /// <summary>
        /// 列印開案單
        /// </summary>
        /// <returns></returns>
        public ActionResult Print(int id, string format = "pdf")
        {
            Kickoff ki = db.Kickoffs.FirstOrDefault(m => m.ID == id);

            #region 報表資料來源
            //1.RDRMain
            List<RDRMainReportViewModel> mainReport = (from main in db.RDRMains.Where(m => m.ID == ki.ParentID)
                                                join car in db.CarMakers on main.CarMakerID equals car.ID
                                                select new RDRMainReportViewModel { rdrMain = main, CarMakerName = car.Name }).ToList();

            //2.RDRModule
            List<RDRModuleReportViewModel> modulesList = (from module in db.RDRModules.Where(m => m.ParentID == ki.ParentID)
                                                          join customer in db.Customers on module.CustomerID equals customer.ID
                                                          join productGroup in db.ProductGroups on module.ProductGroupID equals productGroup.ID
                                                          join main in db.RDRMains on module.ParentID equals main.ID
                                                          select new RDRModuleReportViewModel { rdrModule = module, CustomerName = customer.Name, ProductGroupName = productGroup.Name }).ToList();

            //3.RDRInformation
            List<RDRInfoReportViewModel> infoReport = (from info in db.RDRInformations.Where(x => x.ParentID == ki.ParentID)
                                                     select new RDRInfoReportViewModel { rdrInfo = info }).ToList();


            //4.Kickoff
            List<KickoffReportViewModel> kickofReport = (from kick in db.Kickoffs.Where(m => m.ID == ki.ID)
                                                   select new KickoffReportViewModel { kickoff = kick }).ToList();

            //5.KickoffDetails
            List<KickoffDetailsReportViewModel> kickoffDetailsList = (from detail in db.KickoffDetails.Where(m => m.ParentID == ki.ID)
                                                                      select new KickoffDetailsReportViewModel { kickDetail = detail }).ToList();
            #endregion

            try
            {
                // Set report info
                LocalReport localReport = new LocalReport();
                localReport.ReportPath = Server.MapPath("~/Reports/AwardedReport/AwardedRpt.rdlc");
                localReport.DataSources.Add(new ReportDataSource("DS_RDRMain", mainReport));
                localReport.DataSources.Add(new ReportDataSource("DS_RDRModule", modulesList));
                localReport.DataSources.Add(new ReportDataSource("DS_RDRInfo", infoReport));
                localReport.DataSources.Add(new ReportDataSource("DS_Kickoff", kickofReport));
                localReport.DataSources.Add(new ReportDataSource("DS_KickoffDetails", kickoffDetailsList));
                //rw.ReportParameters.Add(new ReportParameter("param", list[0].EstimateProduct));

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
                return File(renderedBytes, mimeType, string.Format("{0}.{1}", Server.UrlPathEncode("Kickoff"), fileNameExtension));
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                return Content("下載失敗!");
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


        private bool SaveToRDRModule(KickoffCreateViewModel viewModel, FormCollection fc)
        {
            bool IsSaveOK = false; 

            //儲存 RDRModule
            foreach (var item in viewModel.rdrModuleList)
            {
                int idx = viewModel.rdrModuleList.IndexOf(item);

                RDRModule module = db.RDRModules.Where(m => m.ID == item.ID).FirstOrDefault();
                if (module != null)
                {
                    try
                    {
                        module.IsAwarded = Convert.ToBoolean(fc["AwardedOrFail" + idx]); ;
                        module.Price = item.Price;
                        module.Reason = item.Reason;

                        db.Entry(module).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        IsSaveOK = true;
                    }
                    catch (Exception ex)
                    {
                        IsSaveOK = false;
                    }
                }
                else
                {
                    IsSaveOK = false;
                }
                
            }
            return IsSaveOK;
        }

        private bool SaveToRDRInformation(KickoffCreateViewModel viewModel)
        {
            bool IsSaveOK = false;

            //儲存 RDRInformation
            RDRInformation info = db.RDRInformations.Where(m => m.ID == viewModel.rdrInfo.ID).FirstOrDefault();
            if (info != null)
            {
                try
                {
                    info.IsSpec = viewModel.rdrInfo.IsSpec;
                    info.IsGERBER = viewModel.rdrInfo.IsGERBER;
                    info.IsWireDwg = viewModel.rdrInfo.IsWireDwg;
                    info.IsSample = viewModel.rdrInfo.IsSample;
                    info.IsBOM = viewModel.rdrInfo.IsBOM;
                    info.IsAssemblyDwg = viewModel.rdrInfo.IsAssemblyDwg;
                    info.IsDChart = viewModel.rdrInfo.IsDChart;
                    info.IsCKT = viewModel.rdrInfo.IsCKT;
                    info.Is2D = viewModel.rdrInfo.Is2D;
                    info.IsROHS = viewModel.rdrInfo.IsROHS;
                    info.IsELV = viewModel.rdrInfo.IsELV;
                    info.IsVLS = viewModel.rdrInfo.IsVLS;
                    info.Is3D = viewModel.rdrInfo.Is3D;

                    db.Entry(info).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    IsSaveOK = true;
                }
                catch (Exception ex)
                {
                    IsSaveOK = false;
                }
            }
            return IsSaveOK;
        }
    }
}