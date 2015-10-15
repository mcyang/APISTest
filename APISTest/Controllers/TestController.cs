using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace APISTest.Controllers
{
    public class TestController : Controller
    {
        //宣告 logger
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Test
        public ActionResult Index()
        {
            ViewBag.Content = "歡迎來到前台!";

            logger.Trace("我是Trace");
            logger.Debug("我是Debug");
            logger.Info("我是Info");
            logger.Warn("我是Warn");
            logger.Error("我是Error");
            logger.Fatal("我是Fatal");

            try
            {
                int a = 6;
                int b = 0;
                int result = a / b;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
            }

            return View();
        }
    }
}