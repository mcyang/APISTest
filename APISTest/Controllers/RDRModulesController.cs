using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APISTest.Models;
using APISTest.ViewModels;


namespace APISTest.Controllers
{
    public class RDRModulesController : Controller
    {
        private JohnTestEntities db = new JohnTestEntities();
        List<RDRModuleViewModel> AddModuleList = new List<RDRModuleViewModel>();

        #region 列表頁
        // GET: RDRModules
        public ActionResult Index()
        {
            return View(db.RDRModules.ToList());
        }

        /// <summary>
        /// 顯示新增的機種資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowModuleList(int id)
        {
            return View(db.RDRModules.Where(m => m.ParentID == id).ToList());
        } 
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
        // GET: RDRModules/Create
        /// <summary>
        /// 新增機種
        /// </summary>
        /// <param name="id">RDRMain.ID (RDR主表ID)</param>
        /// <returns></returns>
        public ActionResult Create(int id)
        {
            //Search By RDRMain.ID
            RDRMain main = db.RDRMains.Find(id);

            RDRModuleViewModel viewModel = new RDRModuleViewModel();
            viewModel.ParentID = main.ID;
            viewModel.MainCode = main.RDRNumber;
            viewModel.StartYear = main.SOPDate.Year;
            viewModel.EndYear = main.EOLDate.Year;
            viewModel.CustomerID = main.CustomerTeamID;

            return View(viewModel);
        }

        // POST: RDRModules/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RDRModuleViewModel viewModel, FormCollection fc)
        {
            //現在做法是將Module新增到 db.RDRModuleTemp 裡
            //User確認清單內容無誤後，按儲存才全部打包存進 db.RDRModule
            RDRModuleTemp rdrModuleTemp = new RDRModuleTemp();      //RDRModule的暫存表
            
            rdrModuleTemp.ParentID = viewModel.ParentID;
            rdrModuleTemp.RDRNumber = viewModel.MainCode;
            rdrModuleTemp.ModuleName = viewModel.ModuleName;
            rdrModuleTemp.CustomerBOM = viewModel.CustomerBOM;  //客戶料號

            int pid = 0;
            int.TryParse(fc["ProductGroupsList"], out pid);
            rdrModuleTemp.ProductGroupID = pid;                     //產品別ID
            string productCode = db.ProductGroups.Find(pid).Code;   //產品別代碼(全碼)
            string productName = db.ProductGroups.Find(pid).Name;   //產品別名稱
            rdrModuleTemp.ProductGroupCode = productCode;
            rdrModuleTemp.ProductGroupName = productName;

            int cid = 0;
            int.TryParse(fc["CustomersList"], out cid);
            rdrModuleTemp.CustomerID = cid;                         //交貨地ID
            string customerName = db.Customers.Find(cid).Name;      //交貨地名稱
            rdrModuleTemp.CustomerName = customerName;

            rdrModuleTemp.EstimateProduct = fc["EstimateProduct"];
            rdrModuleTemp.Attachment = fc["Attachment"];  //附件
            rdrModuleTemp.Remark = viewModel.Remark;  //Remark
            rdrModuleTemp.ModuleVersion = 0;    //版本號 預設值0
            rdrModuleTemp.CreateTime = System.DateTime.Now;
           

            db.RDRModuleTemps.Add(rdrModuleTemp);
            db.SaveChanges();

            return RedirectToAction("Create", new { id = viewModel.ParentID });
        }
        #endregion


        #region 編輯頁
        /// <summary>
        /// 
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

            RDRModuleEditViewModel viewModel = (from module in db.RDRModules
                                                join product in db.ProductGroups on module.ProductGroupID equals product.ID
                                                join customer in db.Customers on module.CustomerID equals customer.ID
                                                where module.ID == id
                                                select new RDRModuleEditViewModel {
                                                    ID = module.ID,
                                                    ParentID = module.ParentID,
                                                    RDRNumber = module.RDRNumber,
                                                    ModuleName = module.ModuleName,
                                                    ProductGroupID = module.ProductGroupID,
                                                    ProductGroupName = product.Name,
                                                    CustomerID = module.CustomerID,
                                                    CustomerName = customer.Name,
                                                    Attachment = module.Attachment,
                                                    Remark = module.Remark,
                                                    EstimateProduct = module.EstimateProduct

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

        // POST: RDRModules/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RDRModuleEditViewModel viewModel, FormCollection fc)
        {
            RDRModule rdrModule = db.RDRModules.Find(viewModel.ID);
            
            rdrModule.ModuleName = viewModel.ModuleName;
            rdrModule.Remark = viewModel.Remark;
            rdrModule.EstimateProduct = viewModel.EstimateProduct;

            if (ModelState.IsValid)
            {
                db.Entry(rdrModule).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //儲存成功，導向RDRManage/Details
                return RedirectToAction("Details","RDRManage", new { id = rdrModule.ParentID });
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
        /// 
        /// </summary>
        /// <param name="id">RDRModules.ID</param>
        /// <returns></returns>
        public ActionResult DeleteFromRDRManageDetails(int? id)
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

            return RedirectToAction("Details", "RDRManage", new { id = rdrModule.ParentID });
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

        public ActionResult Main()
        {
            return View();
        }

        /// <summary>
        /// 機種資料實際新增至RDRModule
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(int selectParentID)
        {
            //Insert Into RDRModules
            List<RDRModuleTemp> tempList = db.RDRModuleTemps.ToList();

            if (tempList.Count > 0)
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

                return RedirectToAction("Details", "RDRManage", new { id = selectParentID });
            }
            else
            {
                //日後有時間應該改成宗瑋的 ShowMsgThenRedirect(string Msg, string Url) 這樣共用比較好用
                return Content("<script language='javascript' type='text/javascript'>alert('尚未新增機種資料!'); location.replace('" + Url.Action("Create", "RDRModules", new { id = selectParentID }) + "');</script>");
            }
        }

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
                    rdrNumber = MainCode + "-" + ProductShortCode + "." + "1" + "." + moduleCount + "." + "0";
                }

            }
            return rdrNumber;
        }
    }
}
