using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using APISTest.Models;
using APISTest.ViewModels;

namespace APISTest.Controllers
{
    public class CustomersController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();
        private const int pageSize = 10; // 設定分頁一頁10筆資料

        #region 列表頁
        // GET: Customers
        public ActionResult Index()
        {
            //int currentPage = page ?? 1; //當前頁

            //作分頁前一定要先 OrderBy
            var data = from cs in db.Customers
                       join csteam in db.CustomerTeams
                       on cs.ParentID equals csteam.ID
                       orderby cs.ID
                       select new { cs, csteam.Code };

            List<CustomerViewModel> list = new List<CustomerViewModel>();
            foreach (var item in data)
            {
                CustomerViewModel viewModel = new CustomerViewModel();
                viewModel.customer = item.cs;
                viewModel.CustomerTeamCode = item.Code;
                list.Add(viewModel);
            }

            //var result = data.ToPagedList(currentPage, pageSize);
            return View(list);
        }

        /// <summary>
        /// Index中資料會變動的部分抽出來放在PartialView
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult PagedPartial(int? page)
        {
            int currentPage = page ?? 1; //當前頁

            //作分頁前一定要先 OrderBy
            var data = from cs in db.Customers
                       join csteam in db.CustomerTeams
                       on cs.ParentID equals csteam.ID
                       orderby cs.ID
                       select new
                       {
                           cs,
                           csteam.Code
                       };

            List<CustomerViewModel> list = new List<CustomerViewModel>();
            foreach (var item in data)
            {
                CustomerViewModel viewModel = new CustomerViewModel();
                viewModel.customer = item.cs;
                viewModel.CustomerTeamCode = item.Code;
                list.Add(viewModel);
            }
            var result = list.ToPagedList(currentPage, pageSize);
            ViewData.Model = result;

            return PartialView("_PagedAjax");
        } 
        #endregion


        #region 明細頁
        // GET: Customers/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var data = from cs in db.Customers
                       join csteam in db.CustomerTeams
                       on cs.ParentID equals csteam.ID
                       where cs.ID == id
                       select new
                       {
                           cs,
                           csteam.Code
                       };

            CustomerViewModel viewModel = new CustomerViewModel();
            viewModel.customer = data.First().cs;
            viewModel.CustomerTeamCode = data.First().Code;

            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }
        #endregion

        #region 新增頁
        // GET: Customers/Create
        public ActionResult Create()
        {
            List<SelectListItem> list = new List<SelectListItem>(); //建立下拉選單

            foreach (var item in db.CustomerTeams.Where(p => p.IsDelete == false))
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Code,
                    Value = item.ID.ToString()
                });
            }

            ViewData["TeamList"] = list;
            return View();
        }

        // POST: Customers/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer();
                short teamID = 0;
                short.TryParse(fc["TeamList"], out teamID);

                bool isDel = false;
                bool.TryParse(fc["IsDelete"], out isDel);

                customer.Code = fc["Code"];
                customer.Name = fc["Name"];
                customer.ParentID = teamID;
                customer.IsDelete = isDel;
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        } 
        #endregion

        #region 編輯頁
        // GET: Customers/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            //建立下拉選單
            List<SelectListItem> list = new List<SelectListItem>(); 

            foreach (var item in db.CustomerTeams.Where(p => p.IsDelete == false))
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Code,
                    Value = item.ID.ToString(),
                    Selected = item.ID.Equals(customer.ParentID)
                });
            }

            ViewBag.TeamList = list;

            return View(customer);
        }

        // POST: Customers/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection fc)
        {
            Customer customer = db.Customers.Find(id);
            if (ModelState.IsValid)
            {
                short teamID = 0;
                short.TryParse(fc["TeamList"], out teamID);

                bool isDel = false;
                string str = fc["IsDelete"];
                bool.TryParse(fc["IsDelete"], out isDel);

                customer.Code = fc["Code"];
                customer.Name = fc["Name"];
                customer.ParentID = teamID;
                customer.IsDelete = isDel;

                db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", customer);
        } 
        #endregion

        #region 刪除頁
        // GET: Customers/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
