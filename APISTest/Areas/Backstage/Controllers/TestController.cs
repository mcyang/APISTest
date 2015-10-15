using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APISTest.Areas.Backstage.Controllers
{
    public class TestController : Controller
    {
        // GET: Backstage/Test
        public ActionResult Index()
        {
            ViewBag.Content = "歡迎來到後台!";
            return View();
        }
    }
}