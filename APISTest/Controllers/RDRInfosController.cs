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
    /// <summary>
    /// RDR表單其他資訊Controller
    /// </summary>
    public class RDRInfosController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        #region 列表頁
        // GET: RDROthersInfoes
        public ActionResult Index()
        {
            return View(db.RDRInformations.ToList());
        }
        #endregion


        #region 明細頁
        /// <summary>
        /// RDR其他資訊明細
        /// </summary>
        /// <param name="id">RDRInformation.ID</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDRInformation rdrInfo = db.RDRInformations.Find(id);
            if (rdrInfo == null)
            {
                return HttpNotFound();
            }
            return View(rdrInfo);
        }
        #endregion


        #region 新增頁
        /// <summary>
        /// RDR其他資訊新增
        /// </summary>
        /// <param name="id">RDRMain.ID</param>
        /// <returns></returns>
        public ActionResult Create(int id)
        {
            RDRInfoViewModel viewModel = new RDRInfoViewModel();
            viewModel._ParentID = id;

            return View(viewModel);
        }

        // POST: RDROthersInfoes/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RDRInfoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                switch (viewModel.SelectedDevelopSiteId)
                {
                    case 1:
                        viewModel.rdrInfo.IsLAK = true;
                        break;
                    case 2:
                        viewModel.rdrInfo.IsLAW = true;
                        break;
                    case 3:
                        viewModel.rdrInfo.IsLGA = true;
                        break;
                    case 4:
                        viewModel.rdrInfo.IsLAT = true;
                        break;
                    case 5:
                        viewModel.rdrInfo.IsTCH = true;
                        break;
                }

                db.RDRInformations.Add(viewModel.rdrInfo);
                db.SaveChanges();
                return RedirectToAction("Index", "RDRManage");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            return View(viewModel);
        }
        #endregion

        /// <summary>
        /// GET: RDR其他資訊編輯
        /// </summary>
        /// <param name="id">RDRInformation.ID</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RDRInfoViewModel viewModel = (from info in db.RDRInformations
                                          where info.ID == id
                                          select new RDRInfoViewModel
                                          {
                                              rdrInfo = info
                                          }).FirstOrDefault();

            if (viewModel.rdrInfo.IsLAK) { viewModel.SelectedDevelopSiteId = 1; }
            if (viewModel.rdrInfo.IsLAW) { viewModel.SelectedDevelopSiteId = 2; }
            if (viewModel.rdrInfo.IsLGA) { viewModel.SelectedDevelopSiteId = 3; }
            if (viewModel.rdrInfo.IsLAT) { viewModel.SelectedDevelopSiteId = 4; }
            if (viewModel.rdrInfo.IsTCH) { viewModel.SelectedDevelopSiteId = 5; }

            return View(viewModel);
        }

        // POST: RDRInfos/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RDRInfoViewModel viewModel)
        {
            //RDRInformation rdrInfo = db.RDRInformations.Where(m => m.ID == viewModel.rdrInfo.ID).FirstOrDefault();

            switch (viewModel.SelectedDevelopSiteId)
            {
                case 1:
                    viewModel.rdrInfo.IsLAK = true;
                    break;
                case 2:
                    viewModel.rdrInfo.IsLAW = true;
                    break;
                case 3:
                    viewModel.rdrInfo.IsLGA = true;
                    break;
                case 4:
                    viewModel.rdrInfo.IsLAT = true;
                    break;
                case 5:
                    viewModel.rdrInfo.IsTCH = true;
                    break;
            }

            if (ModelState.IsValid)
            {
                switch (viewModel.SelectedDevelopSiteId)
                {
                    case 1:
                        viewModel.rdrInfo.IsLAK = true;
                        break;
                    case 2:
                        viewModel.rdrInfo.IsLAW = true;
                        break;
                    case 3:
                        viewModel.rdrInfo.IsLGA = true;
                        break;
                    case 4:
                        viewModel.rdrInfo.IsLAT = true;
                        break;
                    case 5:
                        viewModel.rdrInfo.IsTCH = true;
                        break;
                }

                db.Entry(viewModel.rdrInfo).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "RDRManage", new { id = viewModel.rdrInfo.ParentID });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return View(viewModel);
        }

        // GET: RDROthersInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDRInformation rDROthersInfo = db.RDRInformations.Find(id);
            if (rDROthersInfo == null)
            {
                return HttpNotFound();
            }
            return View(rDROthersInfo);
        }

        // POST: RDROthersInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RDRInformation rDROthersInfo = db.RDRInformations.Find(id);
            db.RDRInformations.Remove(rDROthersInfo);
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
