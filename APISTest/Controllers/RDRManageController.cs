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

namespace APISTest.Controllers
{
    public class RDRManageController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        #region RDR表單-列表頁
        // GET: RDRMains
        // Join RDRMain、RDRModule、RDRInfo三張表
        public ActionResult Index()
        {
            var aa = from rdrMain in db.RDRMains.Where(m=>m.IsDelete == false)
                     join car in db.CarMakers
                     on rdrMain.CarMakerID equals car.ID
                     select new { rdrMain, car.Name };

            List<RDRMainIndexViewModel> list = new List<RDRMainIndexViewModel>();
            foreach (var item in aa)
            {
                RDRMainIndexViewModel viewModel = new RDRMainIndexViewModel();
                viewModel.rdrMain = item.rdrMain;
                viewModel.CarMaker = item.Name;
                list.Add(viewModel);
            }

            #region 搜尋條件

            #endregion
            return View(list);
        }
        #endregion

        public ActionResult Details(int? id)
        {
            //找不到!! 拋出例外
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RDRMainDetailsViewModel mainData = (from main in db.RDRMains.Where(m => m.ID == id)
                       join car in db.CarMakers on main.CarMakerID equals car.ID
                       select new RDRMainDetailsViewModel
                       {
                           rdrMain = main ,
                           CarMaker = car.Name
                       }).SingleOrDefault();

            RDRViewModel viewModel = new RDRViewModel();
            viewModel.rdrMainDetail = mainData;
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

            RDRInformation rdrInfo = db.RDRInformations.Find(id);
            viewModel.rdrInfoDetail = rdrInfo;

            return View(viewModel);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id">RDRMain.ID</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            //1.找出該筆資料
            RDRMain item = db.RDRMains.Find(id);
            if (item != null)
            {
                //2.設定假刪(IsDelete=true)
                item.IsDelete = true;

                //3.資料表Save
                db.SaveChanges();
            }

            //4.重新導向回列表頁
            return RedirectToAction("Index");
        }
    }
}