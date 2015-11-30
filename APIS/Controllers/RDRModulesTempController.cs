using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using APIS.Models;

namespace APIS.Controllers
{
    /// <summary>
    /// 機種暫存資料
    /// </summary>
    public class RDRModulesTempController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        //[HttpPost]
        //public ActionResult Delete(int Id, int parentID)
        //{
        //    RDRModuleTemp temp = db.RDRModuleTemps.Find(Id);
        //    db.RDRModuleTemps.Remove(temp);
        //    db.SaveChanges();

        //    return RedirectToAction("Create", "RDRModules", new { id = parentID });
        //}

        [HttpPost]
        public JsonResult Delete(int Id, int parentID)
        {
            RDRModuleTemp temp = db.RDRModuleTemps.Find(Id);
            db.RDRModuleTemps.Remove(temp);
            db.SaveChanges();

            return Json(new { result = "Redirect", url = Url.Action("Create", "RDRModules", new { id = parentID }) });
        }

        [HttpGet]  // 奇怪! 若改成HttpPost就不能用
        public ActionResult DeleteAll(int parentID)
        {
            using (JohnTestEntities tempdb = new JohnTestEntities())
            {
                List<RDRModuleTemp> list = tempdb.RDRModuleTemps.Where(m => m.ParentID == parentID).ToList();
                foreach (var item in list)
                {
                    //db.RDRModuleTemps.Remove(item);  //幹! 這個作法竟然不能用
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                return RedirectToAction("Details", "RDRManage", new { id = parentID });
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