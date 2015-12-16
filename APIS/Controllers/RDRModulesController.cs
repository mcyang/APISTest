using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APIS.Models;
using APIS.ViewModels;


namespace APIS.Controllers
{
    public class RDRModulesController : BaseController
    {
        private JohnTestEntities db = new JohnTestEntities();
        List<RDRModuleViewModel> AddModuleList = new List<RDRModuleViewModel>();

        #region 列表頁
        //目前沒用到
        //public ActionResult Index()
        //{
        //    return View(db.RDRModules.ToList());
        //}
        #endregion


        #region 明細頁
        // GET: RDRModules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDRModule rDRModule = db.RDRModules.Find(id);
            if (rDRModule == null)
            {
                return HttpNotFound();
            }
            return View(rDRModule);
        }
        #endregion


        #region 新增頁
        /// <summary>
        /// GET: 新增機種
        /// </summary>
        /// <param name="id">RDRMain.ID (RDR主表ID)</param>
        /// <returns></returns>
        public ActionResult Create(int id)
        {
            //Search By RDRMain.ID
            RDRMain main = db.RDRMains.Where(m => m.ID == id).FirstOrDefault(); // .Find(id);
            if (main != null)
            {
                RDRModuleCreateViewModel viewModel = new RDRModuleCreateViewModel();
                viewModel.ParentID = id;
                viewModel.MainCode = main.RDRNumber;
                viewModel.StartYear = main.SOPDate.Year;
                viewModel.EndYear = main.EOLDate.Year;
                viewModel.CustomerTeamID = main.CustomerTeamID;

                viewModel.rdrModuleList = (from m in db.RDRModules.Where(m => m.ParentID == id)
                                           join p in db.ProductGroups on m.ProductGroupID equals p.ID
                                           join c in db.Customers on m.CustomerID equals c.ID
                                           select new RDRModuleDetailsViewModel
                                           {
                                               ID = m.ID,
                                               ParentID = m.ParentID,
                                               RDRNumber = m.RDRNumber,
                                               ModuleName = m.ModuleName,
                                               CustomerBOM = m.CustomerBOM,
                                               ProductGroupName = p.Name,
                                               CustomerName = c.Name,
                                               EstimateProduct = m.EstimateProduct,
                                               Attachment = m.Attachment,
                                               Remark = m.Remark,
                                               CreateTime = m.CreateTime
                                           }).ToList();

                return View(viewModel);
            }
            else
            {
                //拋出例外
                return ShowMsgThenRedirect("找不到RDR表單", Url.Action("Details","RDRManage", new { id = id }));
            }


            //RDRModuleViewModel viewModel = new RDRModuleViewModel();
            //viewModel.ParentID = main.ID;
            //viewModel.MainCode = main.RDRNumber;
            //viewModel.StartYear = main.SOPDate.Year;
            //viewModel.EndYear = main.EOLDate.Year;
            //viewModel.CustomerID = main.CustomerTeamID;

            //ViewData["ParentID"] = viewModel.ParentID;
            //viewModel.RDRModuleTempList = db.RDRModuleTemps.Where(m => m.ParentID == id).ToList();

            //return View(viewModel);
        }
        

        /// <summary>
        /// POST: 新增機種
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="fc"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(RDRModuleViewModel viewModel, FormCollection fc, HttpPostedFileBase file)
        //{
        //    //現在做法是將Module新增到 db.RDRModuleTemp 裡
        //    //User確認清單內容無誤後，按儲存才全部打包存進 db.RDRModule
        //    RDRModuleTemp rdrModuleTemp = new RDRModuleTemp();      //RDRModule的暫存表
            
        //    rdrModuleTemp.ParentID = viewModel.ParentID;
        //    rdrModuleTemp.RDRNumber = viewModel.MainCode;
        //    rdrModuleTemp.ModuleName = viewModel.ModuleName;
        //    if (string.IsNullOrWhiteSpace(viewModel.CustomerBOM))
        //    {
        //        rdrModuleTemp.CustomerBOM = "N/A";  //客戶料號
        //    }

        //    int pid = 0;
        //    int.TryParse(fc["ProductGroupsList"], out pid);
        //    rdrModuleTemp.ProductGroupID = pid;                     //產品別ID
        //    string productCode = db.ProductGroups.Find(pid).Code;   //產品別代碼(全碼)
        //    string productName = db.ProductGroups.Find(pid).Name;   //產品別名稱
        //    rdrModuleTemp.ProductGroupCode = productCode;
        //    rdrModuleTemp.ProductGroupName = productName;

        //    int cid = 0;
        //    int.TryParse(fc["CustomersList"], out cid);
        //    rdrModuleTemp.CustomerID = cid;                         //交貨地ID
        //    string customerName = db.Customers.Find(cid).Name;      //交貨地名稱
        //    rdrModuleTemp.CustomerName = customerName;

        //    rdrModuleTemp.EstimateProduct = fc["EstimateProduct"];
        //    //rdrModuleTemp.Attachment = fc["Attachment"];  //附件
        //    rdrModuleTemp.Remark = viewModel.Remark;  //Remark
        //    rdrModuleTemp.ModuleVersion = 0;    //版本號 預設值0
        //    rdrModuleTemp.CreateTime = System.DateTime.Now;

        //    //上傳
        //    //從RDRNumber 取出資料夾結構: RFQFiles > 客戶群> ProjectName > 交貨地 > ModuleName > Version
        //    string UploadFilesType = "RFQFiles";
        //    string rdrNum = viewModel.MainCode; //ex. AL.15.0001
        //    string team = rdrNum.Substring(0, rdrNum.IndexOf('.')); //取出客戶群
        //    string projName = db.RDRMains.Find(viewModel.ParentID).ProjectName;      //專案名稱
        //    string version = ".0"; //新增RDR表單的機種內容,預設版本號為0

        //    //UP是IIS的虛擬目錄，於IIS設定指向Web Server上的實體資料夾
        //    var target = "/UP/" + UploadFilesType + "/" + team + "/" + projName + "/" + customerName + "/" + viewModel.ModuleName + "/" + version;
        //    string path = Server.MapPath(target);
        //    bool IsUpload = GOUpload(file); //驗證是否真的有檔案上傳 & 驗證檔案大小、格式是否符合規範

        //    if (IsUpload)
        //    {
        //        var fileName = System.IO.Path.GetFileName(file.FileName); //完整檔名

        //        try
        //        {
        //            if (!System.IO.Directory.Exists(path))
        //            {
        //                System.IO.Directory.CreateDirectory(path); //檢查資料夾路徑是否存在, 不存在就自動建立
        //            }

        //            path = System.IO.Path.Combine(path, fileName);
        //            file.SaveAs(path); //檔案存放到儲存路徑上

        //            //寫入資料庫
        //            using (APIS.Models.JohnTestEntities db = new Models.JohnTestEntities())
        //            {
        //                APIS.Models.UploadFile uploadfile = new Models.UploadFile();
        //                uploadfile.RefID = viewModel.ParentID;
        //                uploadfile.Name = fileName;
        //                uploadfile.FileSize = file.ContentLength;
        //                uploadfile.UploadType = 1;
        //                uploadfile.ContentType = file.ContentType;
        //                uploadfile.Location = path;
        //                uploadfile.CreateDateTime = DateTime.Now;
        //                uploadfile.CreateUserID = 1;
        //                uploadfile.ModifyDateTime = DateTime.Now;
        //                uploadfile.ModifyUserID = 1;

        //                db.UploadFiles.Add(uploadfile);
        //                db.SaveChanges();
        //            }
        //            ModelState.Clear();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }

        //        rdrModuleTemp.Attachment = path; //附件位置
        //    }
        //    else
        //    {
        //        rdrModuleTemp.Attachment = ""; //無附件
        //    }

        //    db.RDRModuleTemps.Add(rdrModuleTemp);
        //    db.SaveChanges();
            
        //    return RedirectToAction("Create", new { id = viewModel.ParentID });
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RDRModuleCreateViewModel viewModel, HttpPostedFileBase file, FormCollection fc)
        {
            string fileName = "";
            string path = "";
            bool IsUploadOK = false;
            bool IsWriteToRDRModule = false;
            int moduleID = 0;

            int pid = 0;
            int.TryParse(fc["ProductGroupsList"], out pid); //表單產品別ID

            int cid = 0;
            int.TryParse(fc["CustomersList"], out cid); //表單客戶ID



            //1.若有上傳附件則執行上傳驗證,若無上傳略過此步驟
            if (file != null)
            {
                #region 上傳
                //從RDRNumber 取出資料夾結構: RFQFiles > 客戶群> ProjectName > 交貨地 > ModuleName > Version
                string UploadFilesType = "RFQFiles";
                string team = viewModel.MainCode.Substring(0, viewModel.MainCode.IndexOf('.')); //取出客戶群,ex. AL.15.0001
                string projName = db.RDRMains.Find(viewModel.ParentID).ProjectName;      //專案名稱
                string customerName = db.Customers.Find(cid).Name;
                string version = ".0"; //新增RDR表單的機種內容,預設版本號為0

                //UP是IIS的虛擬目錄，於IIS設定指向Web Server上的實體資料夾
                var target = "/UP/" + UploadFilesType + "/" + team + "/" + customerName + "/" + projName + "/" + viewModel.ModuleName + "/" + version;
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
                        return ShowMsgThenRedirect(ex.ToString(), Url.Action("Create", "RDRModule", new { id = viewModel.ParentID }));
                    }
                }
                else
                {
                    path = "";
                }
                #endregion
            }

            //2.上傳驗證OK>>寫入db.RDRModule,若寫入db.RDRModule失敗則刪除附件&拋出例外
            try
            {
                RDRModule rdrModule = new RDRModule();
                rdrModule.ParentID = viewModel.ParentID;
                rdrModule.RDRNumber = CreateRDRNumber(viewModel.MainCode, pid, cid);
                rdrModule.ModuleName = viewModel.ModuleName;

                if (string.IsNullOrWhiteSpace(viewModel.CustomerBOM))
                {
                    rdrModule.CustomerBOM = "N/A";  //客戶料號
                }
                else {
                    rdrModule.CustomerBOM = viewModel.CustomerBOM;
                }
                
                rdrModule.ProductGroupID = pid;                     //產品別ID
                rdrModule.CustomerID = cid;                         //交貨地ID
                rdrModule.EstimateProduct = viewModel.EstimateProduct;
                rdrModule.Attachment = path;
                rdrModule.Remark = viewModel.Remark;
                rdrModule.ModuleVersion = 0;    //版本號 預設值0
                rdrModule.IsDelete = false;
                rdrModule.CreateTime = DateTime.Now;

                db.RDRModules.Add(rdrModule);
                db.SaveChanges();

                IsWriteToRDRModule = true;
                moduleID = rdrModule.ID;
            }
            catch(Exception ex)
            {
                //刪除已上傳的的檔案
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return ShowMsgThenRedirect(ex.ToString(), Url.Action("Create", "RDRModule", new { id = viewModel.ParentID }));
            }


            //3.寫入db.RDRModule OK>>將附件資訊寫入db.UploadFile,若寫入db.UploadFile失敗則 delete db.RDRModule 與附件, 保持 RDRModule 與 UploadFile 資料一致
            if (IsUploadOK && IsWriteToRDRModule)
            {
                try
                {
                    UploadFile uploadfile = new UploadFile();
                    uploadfile.ModuleID = moduleID;  // UploadFile.RefID 參考 RDRModule.ID
                    uploadfile.Name = fileName;
                    uploadfile.FileSize = file.ContentLength;
                    uploadfile.UploadType = 1;
                    uploadfile.ContentType = file.ContentType;
                    uploadfile.Location = path;
                    uploadfile.IsProductSpec = viewModel.IsProductSpec;
                    uploadfile.IsTestInstruction = viewModel.IsTestInstruction;
                    uploadfile.IsCustomerBOM = viewModel.IsCustomerBOM;
                    uploadfile.IsBinResistorTable = viewModel.IsBinResistorTable;
                    uploadfile.IsPCBA = viewModel.IsPCBA;
                    uploadfile.IsPCB = viewModel.IsPCB;
                    uploadfile.IsHarness = viewModel.IsHarness;
                    uploadfile.IsGerber = viewModel.IsGerber;
                    uploadfile.IsCoordinate = viewModel.IsCoordinate;
                    uploadfile.IsSchematics = viewModel.IsSchematics;
                    uploadfile.IsComp = viewModel.IsComp;
                    uploadfile.IsPVTestPlan = viewModel.IsPVTestPlan;
                    uploadfile.IsSVRF = viewModel.IsSVRF;
                    uploadfile.CreateDateTime = DateTime.Now;
                    uploadfile.CreateUserID = 1;
                    uploadfile.ModifyDateTime = DateTime.Now;
                    uploadfile.ModifyUserID = 1;


                    db.UploadFiles.Add(uploadfile);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    //刪除附件
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    //刪除RDRModule記錄
                    RDRModule mo = db.RDRModules.Where(m => m.ID == moduleID).FirstOrDefault();
                    if (mo != null)
                    {
                        db.RDRModules.Remove(mo);
                        db.SaveChanges();
                    }

                    return ShowMsgThenRedirect(ex.ToString(), Url.Action("Index", "RDRManage"));
                }
            }


            return RedirectToAction("Create", new { id = viewModel.ParentID });
        }

        /// <summary>
        /// 機種資料實際新增至RDRModule
        /// </summary>
        /// <param name="parentID">RDRMain.ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(int parentID) //奇怪!! 為什麼不能Submit整個List ???
        {
            //Insert Into RDRModules
            List<RDRModuleTemp> tempList = db.RDRModuleTemps.Where(m => m.ParentID == parentID).ToList();

            if (tempList != null & tempList.Count > 0)
            {
                foreach (var item in tempList)
                {
                    RDRModule module = new RDRModule();
                    module.ParentID = item.ParentID;
                    module.RDRNumber = item.RDRNumber;
                    module.ModuleName = item.ModuleName;
                    module.ProductGroupID = item.ProductGroupID;
                    module.CustomerID = item.CustomerID;
                    module.EstimateProduct = item.EstimateProduct;
                    module.Attachment = item.Attachment;
                    module.Remark = item.Remark;
                    module.ModuleVersion = item.ModuleVersion;
                    module.CreateTime = item.CreateTime;
                    module.RDRNumber = CreateRDRNumber(item.RDRNumber, item.ProductGroupID, item.CustomerID);
                    module.CustomerBOM = item.CustomerBOM;

                    if (ModelState.IsValid)
                    {
                        db.RDRModules.Add(module);
                        db.SaveChanges();

                        db.RDRModuleTemps.Remove(item);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Details", "RDRManage", new { id = parentID });
            }
            else
            {
                int pid = int.Parse(ViewData["ParentID"].ToString());
                //日後有時間應該改成宗瑋的 ShowMsgThenRedirect(string Msg, string Url) 這樣共用比較好用
                //return Content("<script language='javascript' type='text/javascript'>alert('尚未新增機種資料!'); location.replace('" + Url.Action("Create", "RDRModules", new { id = parentID }) + "');</script>");
                return ShowMsgThenRedirect("尚未新增機種資料", Url.Action("Create", "RDRModules", new { id = parentID }));
            }
        }

        #endregion


        #region 編輯頁
        /// <summary>
        /// GET:編輯機種
        /// </summary>
        /// <param name="id">傳入RDRModule.ID</param>
        /// <returns></returns>
        // GET: RDRModules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RDRModuleEditViewModel viewModel = (from module in db.RDRModules.Where(m=>m.ID == id)
                                                join product in db.ProductGroups on module.ProductGroupID equals product.ID
                                                join customer in db.Customers on module.CustomerID equals customer.ID
                                                select new RDRModuleEditViewModel {
                                                    ID = module.ID,
                                                    ParentID = module.ParentID,
                                                    RDRNumber = module.RDRNumber,
                                                    ModuleName = module.ModuleName,
                                                    CustomerBOM = module.CustomerBOM,
                                                    ProductGroupID = module.ProductGroupID,
                                                    ProductGroupName = product.Name,
                                                    CustomerID = module.CustomerID,
                                                    CustomerName = customer.Name,
                                                    Attachment = module.Attachment,
                                                    Remark = module.Remark,
                                                    EstimateProduct = module.EstimateProduct,
                                                }).FirstOrDefault();

            RDRMain main = db.RDRMains.Find(viewModel.ParentID);
            string StartYear, EndYear;
            if (main != null)
            {
                StartYear = main.SOPDate.Year.ToString();
                EndYear = main.EOLDate.Year.ToString();
                ViewData["StartYear"] = StartYear.ToString();
                ViewData["EndYear"] = EndYear.ToString();
            }
            
            return View(viewModel);
        }

        /// <summary>
        /// POST:編輯機種
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="file"></param>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RDRModuleEditViewModel viewModel)
        {
            //更新db.RDRModule,若更新db.RDRModule失敗則刪除附件&拋出例外
            try
            {
                RDRModule rdrModule = db.RDRModules.Where(m => m.ID == viewModel.ID).FirstOrDefault();
                if (rdrModule != null)
                {
                    rdrModule.ModuleName = viewModel.ModuleName;
                    rdrModule.CustomerBOM = viewModel.CustomerBOM;
                    rdrModule.EstimateProduct = viewModel.EstimateProduct;
                    rdrModule.Remark = viewModel.Remark;

                    if (ModelState.IsValid)
                    {
                        db.Entry(rdrModule).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        return ShowMsgThenRedirect("更新成功!", Url.Action("Details", "RDRManage", new { id = viewModel.ParentID }));
                    }
                }
            }
            catch (Exception ex)
            {
                return ShowMsgThenRedirect(ex.ToString(), Url.Action("Edit", "RDRModule", new { id = viewModel.ID }));
            }
            return View(viewModel);
        }
        

        #endregion


        #region 刪除頁
        // GET: RDRModules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RDRModule rdrModule = db.RDRModules.Find(id);
            if (rdrModule == null)
            {
                return HttpNotFound();
            }

            //rdrModule.IsDelete = true; //假刪
            db.RDRModules.Remove(rdrModule); //真刪
            db.SaveChanges();

            return RedirectToAction("ShowModuleList","RDRModules", new { id = rdrModule.ParentID });
        }

        /// <summary>
        /// RDRManage/Details 的機種刪除
        /// </summary>
        /// <param name="id">RDRModules.ID</param>
        /// <returns></returns>
        public ActionResult DeleteFromRDRManageDetails(int? id, int? parentID)
        {
            if (id == null || parentID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //找出要刪除的機種資料
            RDRModule rdrModule = db.RDRModules.Where(m => m.ID == id).FirstOrDefault();
            UploadFile uploadfile = db.UploadFiles.Where(m => m.ModuleID == id).FirstOrDefault();

            if (rdrModule != null && uploadfile != null) //有module 也有 uploadfile, 刪檔案>刪up>刪module
            {
                //1. 刪除已上傳的的檔案
                List<UploadFile> fileList = db.UploadFiles.Where(m => m.ModuleID == id).ToList();
                foreach (var item in fileList)
                {
                    if (!string.IsNullOrEmpty(item.Location))
                    {
                        if (System.IO.File.Exists(item.Location))
                        {
                            System.IO.File.Delete(item.Location);
                        }
                    }

                    //2. 刪除UploadFile資料表的資料
                    db.UploadFiles.Remove(item);
                    db.SaveChanges();
                }

                //3. 刪除RDRModule的資料
                db.RDRModules.Remove(rdrModule); //真刪
                db.SaveChanges();

                return ShowMsgThenRedirect("刪除成功!", Url.Action("Details","RDRManage",new { id = parentID }));
            }
            else if (rdrModule != null)
            {
                //刪除RDRModule的資料
                db.RDRModules.Remove(rdrModule); //真刪
                db.SaveChanges();

                return ShowMsgThenRedirect("刪除成功!", Url.Action("Details", "RDRManage", new { id = parentID }));
            }
            else
            {
                return ShowMsgThenRedirect("刪除失敗!", Url.Action("Details", "RDRManage", new { id = parentID }));
            }
        }
        

        /// <summary>
        /// RDRModules/Create的機種清單刪除
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteFromCreateList(int Id, int parentID)
        {
            RDRModule rdrModule = db.RDRModules.Where(m => m.ID == Id).FirstOrDefault();
            UploadFile uploadfile = db.UploadFiles.Where(m => m.ModuleID == Id).FirstOrDefault();

            if (rdrModule != null && uploadfile != null) //有module 也有 uploadfile, 刪檔案>刪up>刪module
            {
                //1. 刪除已上傳的的檔案
                List<UploadFile> fileList = db.UploadFiles.Where(m => m.ModuleID == Id).ToList();
                foreach (var item in fileList)
                {
                    if (!string.IsNullOrEmpty(item.Location))
                    {
                        if (System.IO.File.Exists(item.Location))
                        {
                            System.IO.File.Delete(item.Location);
                        }
                    }

                    //2. 刪除UploadFile資料表的資料
                    db.UploadFiles.Remove(item);
                    db.SaveChanges();
                }

                //3. 刪除RDRModule的資料
                db.RDRModules.Remove(rdrModule); //真刪
                db.SaveChanges();

                return Json(new { result = "Success", url = Url.Action("Create", "RDRModules", new { id = parentID }) });
            }
            else if (rdrModule != null)
            {
                //刪除RDRModule的資料
                db.RDRModules.Remove(rdrModule); //真刪
                db.SaveChanges();

                return Json(new { result = "Success", url = Url.Action("Create", "RDRModules", new { id = parentID }) });
            }
            else {
                return Json(new { result = "Fail", url = Url.Action("Create", "RDRModules", new { id = parentID }) });
            }
        }

        #endregion


        #region 進版

        /// <summary>
        /// 進版
        /// </summary>
        /// <param name="id">RDRModule.ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upgrade(int id)
        {
            RDRModule rdrmodule = db.RDRModules.FirstOrDefault(m => m.ID == id);

            //搜尋DB相似的RDRNumber & 取出目前尾碼最大碼
            var lastChar = rdrmodule.RDRNumber.Substring(rdrmodule.RDRNumber.Length - 1, 1); //取RDRNumber最後一碼
            var partString = rdrmodule.RDRNumber.Substring(0, rdrmodule.RDRNumber.Length - 2); //移除RDRNumber最後一碼

            var allChar = (from c in db.RDRModules
                           where c.RDRNumber.Contains(partString)
                           orderby c.RDRNumber
                           select new
                           {
                               c.RDRNumber,
                           }).ToList();
            
            List<int> LastCodeList = new List<int>();
            foreach (var item in allChar)
            {
                string str_last = item.RDRNumber.Substring(rdrmodule.RDRNumber.Length - 1, 1);
                int i_last = int.Parse(str_last);
                LastCodeList.Add(i_last);
            }
            int currentMax = LastCodeList.Max();
            int futureMax = currentMax + 1;

            //複製&新增
            int newModuleID = 0;
            try
            {
                RDRModule newModule = new RDRModule();
                newModule.ParentID = rdrmodule.ParentID;
                newModule.RDRNumber = partString + "." + futureMax.ToString();
                newModule.ModuleName = rdrmodule.ModuleName;
                newModule.CustomerBOM = rdrmodule.CustomerBOM;
                newModule.ProductGroupID = rdrmodule.ProductGroupID;
                newModule.CustomerID = rdrmodule.CustomerID;
                newModule.EstimateProduct = rdrmodule.EstimateProduct;
                newModule.Remark = rdrmodule.Remark;
                newModule.CreateTime = DateTime.Now;

                db.RDRModules.Add(newModule);
                db.SaveChanges();

                newModuleID = newModule.ID;
                return RedirectToAction("Edit", new { id = newModuleID });

            }
            catch (Exception ex)
            {
                
            }
           
            return View();
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

        /// <summary>
        /// RDRNumber 編碼方法
        /// 自動編碼原則: 繼承RDR單號+產品別2碼+交貨地數目1碼+Module數2碼+版本號1碼
        /// </summary>
        /// <param name="MainCode">RDRMain的RDRNumber</param>
        /// <param name="pid">產品別ID</param>
        /// <param name="cid">交貨地ID</param>
        /// <returns></returns>
        protected string CreateRDRNumber(string MainCode, int pid, int cid)
        {
            string rdrNumber = "";

            string ProductCode = db.ProductGroups.Find(pid).Code;   //產品別代碼(全碼)
            string ProductShortCode = ProductCode.Substring(8, 2);  //產品別代碼(後2碼)

            //搜尋機種資料 By RDR單號
            List<RDRModule> list = db.RDRModules.Where(m=>m.RDRNumber.Contains(MainCode)).ToList();

            if (list.Count == 0)
            {
                rdrNumber = MainCode + "-" + ProductShortCode + "." + "1" + "." + "00" + "." + "0";
            }
            else
            {
                //產生Module數:2碼
                string moduleCount = list.Count.ToString().PadLeft(2, '0');

                //判斷規則: 只有 "產品別同 & 交貨地不同" 情況下，需要計算交貨地數。其他情況都固定。
                //ex. AL.16.0001-01.1.00.0
                //    AL.16.0001-01.2.01.0
                bool IsRule = list.Where(m => m.ProductGroupID == pid && m.CustomerID != cid).Any();
                if (IsRule)
                {
                    var compare = list.Where(m => m.ProductGroupID == pid && m.CustomerID != cid).ToList();

                    //找出資料庫中交貨地數目最大值
                    int max = compare.Select(p => Convert.ToInt32(p.RDRNumber.ToString().Substring(14, 1))).Max();
                    string customerCount = (max + 1).ToString();

                    rdrNumber = MainCode + "-" + ProductShortCode + "." + customerCount + "." + moduleCount + "." + "0";
                }
                else
                {
                    string customerCount = "1";
                    rdrNumber = MainCode + "-" + ProductShortCode + "." + customerCount + "." + moduleCount + "." + "0";
                }

            }
            return rdrNumber;
        }

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
                string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf", ".zip", ".rar" }; //限定檔案格式
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
