using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APIS.Controllers
{
    public class UploadFilesController : BaseController
    {
        // GET: UploadFiles
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewData["Message"] = "This is UploadFiles/Add in FancyBox";
            return View();
        }
    }
}