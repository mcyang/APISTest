using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using APISTest.Models;

namespace APISTest.Controllers
{
    /// <summary>
    /// 機種暫存資料
    /// </summary>
    public class RDRModulesTempController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();

        [HttpPost]
        public JsonResult Delete(int? id, int? parentID)
        {
            RDRModuleTemp temp = db.RDRModuleTemps.Find(id);
            db.RDRModuleTemps.Remove(temp);
            db.SaveChanges();

            return Json(new { result = "Redirect", url = Url.Action("Create", "RDRModules",new { id = parentID}) });
        }

        [HttpPost]
        public ActionResult DeleteAll(int parentID)
        {
            RDRModuleTemp temp = db.RDRModuleTemps.Find(parentID);
            db.RDRModuleTemps.Remove(temp);
            db.SaveChanges();

            return RedirectToAction("Details", "RDRManage", new { id = parentID });
        }
    }
}