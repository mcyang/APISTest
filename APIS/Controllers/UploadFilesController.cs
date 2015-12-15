using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APIS.Models;

namespace APIS.Controllers
{
    public class UploadFilesController : BaseController
    {
        JohnTestEntities db = new JohnTestEntities();

        // GET: UploadFiles
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 列出特定機種資料的所有附件資訊
        /// </summary>
        /// <param name="id">機種資料ID(RDRModule.ID)</param>
        /// <returns></returns>
        public ActionResult List(int id)
        {
            var query = db.UploadFiles.Where(m => m.ModuleID == id).OrderBy(m => m.ID);

            return View(query.ToList());
        }

        #region 新增頁
        /// <summary>
        /// Get: 新增附件資訊
        /// </summary>
        /// <param name="id">機種ID</param>
        /// <returns></returns>
        public ActionResult Create(int id)
        {
            UploadFile upload = new UploadFile();

            var query = (from module in db.RDRModules.Where(m => m.ID == id)
                        join main in db.RDRMains on module.ParentID equals main.ID
                        select new
                        {
                            MainID = main.ID,
                            ModuleID = module.ID,
                        }).FirstOrDefault();

            if (query != null)
            {
                upload.MainID = query.MainID;
                upload.ModuleID = query.ModuleID;
                upload.UploadType = 1; //RFQ
                
                return View(upload);
            }

            return View(upload);
        }

        // POST: RDRMains/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UploadFile upload, HttpPostedFileBase file)
        {
            string fileName = "";
            string path = "";
            bool IsUploadOK = false;

            //上傳檔案驗證
            if (file != null)
            {
                #region 上傳
                //從RDRNumber 取出資料夾結構: RFQFiles > 客戶群> ProjectName > 交貨地 > ModuleName > Version
                string UploadFilesType = "RFQFiles";
                string MainCode = db.RDRMains.Where(m => m.ID == upload.MainID).Select(m => m.RDRNumber).FirstOrDefault();
                var query = (from m in db.RDRModules.Where(m => m.ID == upload.ModuleID)
                                       join c in db.Customers on m.CustomerID equals c.ID
                                       select new { ModuleName = m.ModuleName, customerName = c.Name }).FirstOrDefault();

                string team = MainCode.Substring(0, MainCode.IndexOf('.')); //取出客戶群,ex. AL.15.0001
                string projName = db.RDRMains.Where(m => m.ID == upload.MainID).Select(m => m.ProjectName).FirstOrDefault();  //專案名稱
                string version = ".0"; //新增RDR表單的機種內容,預設版本號為0

                //UP是IIS的虛擬目錄，於IIS設定指向Web Server上的實體資料夾
                var target = "/UP/" + UploadFilesType + "/" + team + "/" + query.customerName + "/" + projName + "/" + query.ModuleName + "/" + version;
                path = Server.MapPath(target);

                bool IsUpload = GOUpload(file); //驗證是否真的有檔案上傳 & 驗證檔案大小、格式是否符合規範
                if (IsUpload)
                {
                    fileName = System.IO.Path.GetFileName(file.FileName); //完整檔名

                    try
                    {
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(path); //檢查資料夾路徑是否存在, 不存在就自動建立
                        }

                        path = System.IO.Path.Combine(path, fileName);
                        file.SaveAs(path); //檔案存放到儲存路徑上

                        IsUploadOK = true;
                    }
                    catch (Exception ex)
                    {
                        path = "";
                        return ShowMsgThenRedirect(ex.ToString(), Url.Action("List", "UploadFiles", new { id = upload.ModuleID }));
                    }
                }
                else
                {
                    path = "";
                }
                #endregion
            }


            if (IsUploadOK)
            {
                //新增附件資訊到 db.UploadFile
                upload.Name = fileName;
                upload.Location = path;
                upload.ContentType = file.ContentType;
                upload.FileSize = file.ContentLength;
                upload.CreateDateTime = DateTime.Now;
                upload.ModifyDateTime = DateTime.Now;
                upload.CreateUserID = 1;
                upload.ModifyUserID = 1;

                try
                {
                    db.UploadFiles.Add(upload);
                    db.SaveChanges();

                    return ShowMsgThenRedirect("新增成功", Url.Action("List", "UploadFiles", new { id = upload.ModuleID }));
                }
                catch (Exception ex)
                {
                    //新增記錄失敗時,要刪除附件,讓資訊一致
                    //刪除已上傳的的檔案
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    return ShowMsgThenRedirect(ex.ToString(), Url.Action("List", "UploadFiles", new { id = upload.ModuleID }));
                }
            }
            return ShowMsgThenRedirect("新增失敗", Url.Action("List", "UploadFiles", new { id = upload.ModuleID }));
        }

        #endregion

        /// <summary>
        /// 檔案管理-明細頁
        /// </summary>
        /// <param name="id">檔案ID</param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            APIS.ViewModels.UploadFileDetailsViewModel viewModel =
                (from u in db.UploadFiles.Where(m => m.ID == id)
                 join m in db.RDRModules on u.ModuleID equals m.ID
                 select new APIS.ViewModels.UploadFileDetailsViewModel
                 {
                     uploadfile = u,
                     RDRNumber = m.RDRNumber,
                     ModuleName = m.ModuleName
                 }).FirstOrDefault();

            if (viewModel != null)
            {
                return View(viewModel);
            }

            return ShowMsgThenRedirect("檢視明細失敗", Url.Action("List","UploadFiles", new { id = viewModel.uploadfile.ModuleID}));
        }

        #region 刪除頁
        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //找出要刪除的機種資料
            UploadFile uploadfile = db.UploadFiles.Where(m => m.ID == id).FirstOrDefault();
            int moduleID = uploadfile.ModuleID ?? 0;

            if (uploadfile != null) //先刪實體檔案,再刪除記錄
            {
                try
                {
                    //刪除已上傳的的檔案
                    if (System.IO.File.Exists(uploadfile.Location))
                    {
                        System.IO.File.Delete(uploadfile.Location);
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }

                //刪除UploadFile資料表的資料
                db.UploadFiles.Remove(uploadfile);
                db.SaveChanges();

                return RedirectToAction("List", "UploadFiles", new { id = moduleID });
                //return ShowMsgThenRedirect("刪除成功", Url.Action("List","UploadFiles",new { id = moduleID }));
            }

            return View(uploadfile);
        }
        #endregion

        /// <summary>
        /// 驗證是否真的有檔案上傳 & 驗證檔案大小、格式是否符合規範
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected bool GOUpload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
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
                    return true;
                }
            }
            return false;
        }
    }
}