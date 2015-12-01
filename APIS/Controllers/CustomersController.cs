using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using APIS.Models;
using APIS.ViewModels;

namespace APIS.Controllers
{
    public class CustomersController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();
        private const int pageSize = 10; // 設定分頁一頁10筆資料

        #region 列表頁
        // GET: Customers
        public ActionResult Index(FormCollection fc, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page; // 當前頁

            //作分頁前一定要先 OrderBy
            var data = (from customer in db.Customers
                       join team in db.CustomerTeams
                       on customer.ParentID equals team.ID
                       orderby customer.ID
                       select new CustomerViewModel {
                           ID = customer.ID,
                           Code = customer.Code,
                           Name = customer.Name,
                           IsDelete = customer.IsDelete,
                           CustomerTeamID = customer.ParentID,
                           CustomerTeamCode = team.Code,
                       });


            #region 搜尋條件
            //依客戶名稱
            string byMakers = fc["serach_CustomerName"] == null ? "" : fc["serach_CustomerName"].Trim();
            if (!String.IsNullOrWhiteSpace(byMakers))
            {
                data = data.Where(m => m.Name.Contains(byMakers)).OrderBy(m => m.ID);
            }

            //依客戶群
            string byTeam = fc["serach_CustomerTeamName"] == null ? "" : fc["serach_CustomerTeamName"];
            if (!String.IsNullOrWhiteSpace(byTeam))
            {
                int byTeamID = 0;
                int.TryParse(byTeam, out byTeamID);
                data = data.Where(m => m.CustomerTeamID == byTeamID).OrderBy(m => m.ID);
            } 
            #endregion

            var result = data.ToPagedList(currentPage, pageSize);
            return View(result);
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
            var data = (from cs in db.Customers
                       join csteam in db.CustomerTeams
                       on cs.ParentID equals csteam.ID
                       orderby cs.ID
                       select new CustomerViewModel
                       {
                           ID = cs.ID,
                           Code = cs.Code,
                           Name = cs.Name,
                           IsDelete = cs.IsDelete,
                           CustomerTeamID = csteam.ID,
                           CustomerTeamCode = csteam.Code
                       }).OrderBy(m=>m.ID);

           
            var result = data.ToPagedList(currentPage, pageSize);
            ViewData.Model = result;

            return PartialView("_PagedAjax");
        } 
        #endregion


        #region 新增頁
        // GET: Customers/Create
        public ActionResult Create()
        {
            List<SelectListItem> list = new List<SelectListItem>(); //建立客戶群下拉選單

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
        public ActionResult Create([Bind(Include = "ID,ParentID,Code,Name,City,IsDelete")] Customer customer)
        {
            if(ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
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

            Customer customer = db.Customers.Where(m => m.ID == id).FirstOrDefault();
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

            ViewData["TeamList"] = list;

            return View(customer);
        }

        // POST: Customers/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ParentID,Code,Name,City,IsDelete")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        #endregion


        #region 刪除頁
        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Where(m => m.ID == id).FirstOrDefault();
            if (customer != null)
            {
                customer.IsDelete = true; //假刪
                if (ModelState.IsValid)
                {
                    db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(customer);
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
