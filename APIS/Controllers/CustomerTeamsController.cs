using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using APIS.Models;

namespace APIS.Controllers
{
    public class CustomerTeamsController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        // GET: CustomerTeams
        public ActionResult Index()
        {
            return View(db.CustomerTeams.ToList());
        }

        // GET: CustomerTeams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerTeams/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerTeam customerTeam)
        {
            if (ModelState.IsValid)
            {
                db.CustomerTeams.Add(customerTeam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerTeam);
        }

        // GET: CustomerTeams/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTeam customerTeam = db.CustomerTeams.Find(id);
            if (customerTeam == null)
            {
                return HttpNotFound();
            }
            return View(customerTeam);
        }

        // POST: CustomerTeams/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Name,LobID,IsDelete")] CustomerTeam customerTeam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerTeam).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerTeam);
        }


        #region 刪除頁
        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTeam customerTeam = db.CustomerTeams.Where(m => m.ID == id).FirstOrDefault();
            if (customerTeam != null)
            {
                customerTeam.IsDelete = true; //假刪
                if (ModelState.IsValid)
                {
                    db.Entry(customerTeam).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(customerTeam);
        }

        #endregion

        /// <summary>
        /// LOB下拉選單&客戶群下拉選單連動用
        /// </summary>
        /// <param name="LobID">EnumLOB索引值</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetTeamsByLOB(string LobID)
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrWhiteSpace(LobID))
            {
                var teams = this.GetTeams(LobID);
                if (teams.Count() > 0)
                {
                    foreach (var team in teams)
                    {
                        items.Add(new KeyValuePair<string, string>(
                            team.ID.ToString(),
                            team.Code));
                    }
                }
            }

            return this.Json(items);
        }

        /// <summary>
        /// 依LOB索引值取得對應的客戶群
        /// </summary>
        /// <param name="LobID">EnumLOB索引值</param>
        /// <returns></returns>
        private IEnumerable<CustomerTeam> GetTeams(string LobID)
        {
            using (JohnTestEntities db = new JohnTestEntities())
            {
                // 到時VSA、MCM 所對應的客戶群資料若有了, 再到這邊作變更
                var query = db.CustomerTeams.Where(m=>m.LobID == LobID).OrderBy(m => m.ID);
                return query.ToList();
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
