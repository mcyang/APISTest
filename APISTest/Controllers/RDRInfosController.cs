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
    public class RDRInfosController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        // GET: RDROthersInfoes
        public ActionResult Index()
        {
            return View(db.RDRInformations.ToList());
        }

        // GET: RDROthersInfoes/Details/5
        public ActionResult Details(int? id)
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

        // GET: RDROthersInfoes/Create
        public ActionResult Create(int id)
        {
            RDRInfoViewModel viewModel = new RDRInfoViewModel();
            viewModel.ParentID = id;
            //viewModel.SelectedDevelopSiteId = 1;
            //viewModel.DevelopSites = new List<DevelopSite> {
            //    new DevelopSite {
            //        Id = 1,
            //        Name = "LAK",
            //    },
            //    new DevelopSite
            //    {
            //        Id = 2,
            //        Name = "LAW",
            //    },
            //     new DevelopSite {
            //        Id = 3,
            //        Name = "LGA",
            //    },
            //    new DevelopSite
            //    {
            //        Id = 4,
            //        Name = "LAT",
            //    },
            //     new DevelopSite {
            //        Id = 5,
            //        Name = "TCH",
            //    },
            //};
            //viewModel.DevelopSites = Enumerable.Range(1, 5).Select(x => new DevelopSite
            //{
            //    Id = x,
            //    Name = "DevelopSite" + x,
            //}).ToList();

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
                return RedirectToAction("Index","RDRManage");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            return View(viewModel);
        }

        // GET: RDROthersInfoes/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: RDROthersInfoes/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ParentID,ProjectType,IsElectronic,IsSoftware,IsMechanism,IsOptic,IsRF,IsLAK,IsLAW,IsLGA,IsLAT,IsTCH,StartDate,EndDate,TxtOther_1,IsSpec,IsGERBER,IsWireDwg,IsSample,IsBOM,IsAssemblyDwg,IsDChart,IsCKT,Is2D,Is3D,IsROHS,IsELV,IsVLS,TxtOthers_2,IsMaterialCost,IsPackageCost,IsProcessCost,IsProductHourCost,IsTestHourCost,IsSamplePrice,IsModuleFee,IsSampleFee,IsHandMoldingFee,IsTravelFee,IsTestFee,IsDevelopFee,IsVerifyToolFee,TxtOthers_3,IsAssignForm,IsProposal,IsDevPlan,IsStepPrice,TxtOthers_4")] RDRInformation rDROthersInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rDROthersInfo).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rDROthersInfo);
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
