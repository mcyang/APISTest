using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APISTest.Models;
using APISTest.ViewModels;
using APISTest.Helpers;
using PagedList;
namespace APISTest.Controllers
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
            if (!string.IsNullOrEmpty(fc["serach_RDRNo"]))
            {
                data = data.Where(m => m.rdrMain.RDRNumber.Contains(fc["serach_RDRNo"])).OrderBy(m=>m.rdrMain.ID);
            }

            // Search By CustomerTeam
            if (!string.IsNullOrEmpty(fc["search_CustomerTeam"]))
            {
                int cid = 0;
                int.TryParse(fc["search_CustomerTeam"], out cid);

                data = data.Where(m => m.rdrMain.CustomerTeamID == cid).OrderBy(m=>m.rdrMain.ID);
            }

            // Search By Site
            if (!string.IsNullOrEmpty(fc["search_Site"]))
            {
                int sid = 0;
                int.TryParse(fc["search_Site"], out sid);
                string site = APISTest.Tools.EnumMapTool.GetDescription((EnumSite)sid);
                data = data.Where(m => m.rdrMain.Site == site).OrderBy(m=>m.rdrMain.ID);
            }

            // Search By CarMaker
            if (!string.IsNullOrEmpty(fc["search_CarMaker"]))
            {
                int carid = 0;
                int.TryParse(fc["search_CarMaker"], out carid);
                data = data.Where(m => m.rdrMain.CarMakerID == carid).OrderBy(m=>m.rdrMain.ID);
            }

            // Search By Product
            if (!string.IsNullOrEmpty(fc["search_Product"]))
            {
                int pid = 0;
                int.TryParse(fc["search_Product"], out pid);
                var idList = db.RDRModules.Where(m => m.ProductGroupID == pid).Select(p=>p.ParentID).Distinct();

                data = data.Where(m=> idList.Contains(m.rdrMain.ID)).OrderBy(m => m.rdrMain.ID);
            }

            // Search By RFQDate
            if (!string.IsNullOrEmpty(fc["search_RFQDate"]))
            {
                DateTime dtRFQ = Convert.ToDateTime(fc["search_RFQDate"]);
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