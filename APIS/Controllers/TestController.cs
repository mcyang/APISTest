using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace APIS.Controllers
{
    /// <summary>
    /// 所有的測試功能都在這邊試作
    /// </summary>
    public class TestController : BaseController
    {
        //宣告 logger
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 測試NLog
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 測試檔案管理清單
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            using (APIS.Models.JohnTestEntities db = new Models.JohnTestEntities())
            {
                return View(db.UploadFiles.ToList());
            }

        }

        /// <summary>
        /// 測試上傳檔案、自動建立資料夾、放到正確的資料夾上
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Uploads()
        {
            return View();
        }

        /// <summary>
        /// 測試上傳檔案、自動建立資料夾、放到正確的資料夾上
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Uploads(HttpPostedFileBase file, FormCollection fc)
        {
            if (!string.IsNullOrEmpty(fc["RDRNumber"]))
            {
                //從RDRNumber 取出資料夾結構: RFQFiles > 客戶群> ProjectName > 交貨地 > ModuleName > Version
                string rdrNum = fc["RDRNumber"]; //ex. AL.15.0001-01.1.00.0
                string team = rdrNum.Substring(0, rdrNum.IndexOf('.')); //取出客戶群
                string projName = fc["ProjectName"];
                string customer = fc["Customer"];
                string module = fc["ModuleName"];
                string version = rdrNum.Substring(rdrNum.LastIndexOf('.'));

                //UP是IIS的虛擬目錄，於IIS設定指向Web Server上的實體資料夾
                var target = "/UP/" + team + "/" + projName + "/" + customer + "/" + module + "/" + version;
                string path = Server.MapPath(target);
                if (ModelState.IsValid)
                {
                    if (file == null)
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                    else if (file.ContentLength > 0) //檔案大小
                    {
                        int MaxContentLength = 1024 * 1024 * 3; //上傳上限: 3 MB
                        string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf" }; //限定檔案格式
                        if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                        {
                            ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                        }
                        else if (file.ContentLength > MaxContentLength) //檔案大小超過上傳上限
                        {
                            ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                        }
                        else
                        {
                            //TO:DO
                            var fileName = System.IO.Path.GetFileName(file.FileName); //完整檔名

                            if (!System.IO.Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path);
                            }
                            path = System.IO.Path.Combine(path, fileName);
                            file.SaveAs(path); //檔案存放到儲存路徑上

                            //寫入資料庫
                            using (APIS.Models.JohnTestEntities db = new Models.JohnTestEntities())
                            {
                                APIS.Models.UploadFile uploadfile = new Models.UploadFile();
                                uploadfile.RefID = 1;
                                uploadfile.Name = fileName;
                                uploadfile.FileSize = file.ContentLength;
                                uploadfile.UploadType = 1;
                                uploadfile.ContentType = file.ContentType;
                                uploadfile.Location = path;
                                uploadfile.CreateDateTime = DateTime.Now;
                                uploadfile.CreateUserID = 1;
                                uploadfile.ModifyDateTime = DateTime.Now;
                                uploadfile.ModifyUserID = 1;

                                db.UploadFiles.Add(uploadfile);
                                db.SaveChanges();
                            }
                            ModelState.Clear();
                            return ShowMsgThenRedirect("File uploaded successfully", Url.Action("List"));
                        }
                    }
                }
            }
            else
            {
                return ShowMsgThenRedirect("Please Indert RDR Number", Url.Action("Upload"));
            }

            return View();
        }
    }
}