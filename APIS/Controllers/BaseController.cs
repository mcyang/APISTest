using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using APIS.Tools;
using APIS.ViewModels;

namespace APIS.Controllers
{
    //[InitializeSimpleMembershipAttribute]


    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class BaseController : Controller
    {
        public const int PageSize = 10;

        /// <summary>資料表工具</summary>
        protected readonly EntityTool EntityTool = new EntityTool();

        /// <summary>
        /// 錯誤頁面
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public ActionResult ErrorPage(Exception ex = null)
        {
            return Redirect(Url.Action("NotFind", "Error", new { area = "" }));
        }

        /// <summary>
        /// 傳回 javascript (base)
        /// </summary>
        protected internal virtual ContentResult JavaScriptContent(string script)
        {
            return Content("<script type=\"text/javascript\">" + script + "</script>", null);
        }

        /// <summary>
        /// 提示訊息後執行 javascript (base)
        /// </summary>
        protected virtual ContentResult ShowMsgThenRunScript(string Msg, string script = "")
        {
            return JavaScriptContent("alert('" + Msg + "'); " + script);
        }

        /// <summary>
        /// 提示訊息後重新導向 (base)
        /// </summary>
        /// <param name="Msg">提示訊息</param>
        /// <param name="Url">重新導向路徑</param>
        protected ContentResult ShowMsgThenRedirect(string Msg, string Url)
        {
            return JavaScriptContent("alert('" + Msg + "'); location.replace('" + Url + "');");
        }

        /// <summary>
        /// 導向指的URL
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        protected ContentResult RedirectToUrl(string Url)
        {
            return JavaScriptContent("location.replace('" + Url + "')");
        }

        public MemoryStream ReadFully(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            //byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms;
            }
        }

        #region 列表 or 搜尋條件 Helper

        /// <summary>搜尋條件快取 Key 值(For Manage Page)</summary>
        //protected const string RouterCatcheKey_Manage = "Manage";

        /// <summary>搜尋條件快取 Key 值(For ListSelector Page)</summary>
        protected const string RouterCatcheKey_ListSelector = "ListSelector";

        /// <summary>搜尋條件快取 Key 值 Header</summary>
        private const string PreviousRouteKeyHeader = "_s_c_";

        /// <summary>
        /// 自 Session or cookie 取得上一次排序、搜尋條件
        /// </summary>
        /// <typeparam name="T1">請確保「需要」記錄的欄位為 public get; public set</typeparam>
        /// <param name="key"></param>
        protected T1 GetCatcheRoutes<T1>(string key = "") where T1 : SortModel, new()
        {
            // 修正 key 值確保不會衝突
            //key = HttpContext.Session.SessionID + PreviousRouteKeyHeader + this.GetType().Name + key;
            key = string.Format("{0}_{1}_{2}_{3}_{4}", HttpContext.Session.SessionID, PreviousRouteKeyHeader, this.GetType().Name, ControllerContext.RouteData.Values["action"], key);
            if (HttpContext.Cache[key] != null)
            {
                return (T1)HttpContext.Cache[key];
            }

            return Activator.CreateInstance<T1>();
        }

        /// <summary>
        /// 儲存現有排序、搜尋條件至 Session or cookie
        /// </summary>
        /// <param name="key">主要的 Session or Cookie Key</param>
        /// <param name="values"></param>
        protected void SetCatcheRoutes<T1>(T1 values, string key = "") where T1 : SortModel
        {
            // 修正 key 值確保不會衝突
            //key = HttpContext.Session.SessionID + PreviousRouteKeyHeader + this.GetType().Name + key;
            key = string.Format("{0}_{1}_{2}_{3}_{4}", HttpContext.Session.SessionID, PreviousRouteKeyHeader, this.GetType().Name, ControllerContext.RouteData.Values["action"], key);
            HttpContext.Cache[key] = values;
        }

        private readonly string[] clearSkipFields = new string[] { "SortField", "SortAction", "Field" };

        /// <summary>
        /// 初始化 FormViewModel 的 SortModel
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="router"></param>
        /// <param name="page"></param>
        /// <param name="DefaultSortField">預設的排序欄位</param>
        /// <param name="fc"></param>
        public T1 initSortingRouter<T1>(T1 router, int? page, string DefaultSortField, FormCollection fc) where T1 : SortModel
        {
            return initSortingRouter<T1>(router, page, DefaultSortField, true, fc);
        }
        /// <summary>
        /// 初始化 FormViewModel 的 SortModel
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="router"></param>
        /// <param name="page"></param>
        /// <param name="DefaultSortField">預設的排序欄位</param>
        /// <param name="DefaultDecsending">預設的排序方法</param>
        /// <param name="fc"></param>
        public T1 initSortingRouter<T1>(T1 router, int? page, string DefaultSortField, bool DefaultDecsending, FormCollection fc) where T1 : SortModel
        {
            FormCollection param = new FormCollection(Request.Unvalidated().QueryString);
            if (fc.AllKeys.Contains("btnSearchAll"))
            {
                // 列出全部則回到第一頁並清空搜尋條件
                page = 0;
                this.ClearSearchRouter(router);
                this.UpdateSearchRouter<SortModel>(router, page, new FormCollection(), param);
            }
            else
            {
                // 重新搜尋則頁碼回到第一頁並更新搜尋參數
                if (fc.AllKeys.Contains("btnSearch"))
                {
                    page = 0;
                    this.UpdateSearchRouter(router, page, fc, param, true);
                }
                else
                {
                    page = page ?? router.NowPage;
                    this.UpdateSearchRouter(router, page, fc, param);
                }

            }

            string sortField, sortAction;
            if (DefaultDecsending)
            {
                _SetSortField(DefaultSortField, "Desc", fc["Field"], fc["SortField"] ?? router.SortField, fc["SortAction"] ?? router.SortAction, out sortField, out sortAction);
            }
            else
            {
                _SetSortField(DefaultSortField, "Asc", fc["Field"], fc["SortField"] ?? router.SortField, fc["SortAction"] ?? router.SortAction, out sortField, out sortAction);
            }

            router.SetSortInfo(sortField, sortAction);
            if (page.HasValue) { router.NowPage = page; }

            return router;
        }

        /// <summary>
        /// 設定排序資訊 (base)
        /// </summary>
        /// <param name="DefaultField">預設排序欄位</param>
        /// <param name="DefaultAction">預設排序欄位</param>
        /// <param name="Field">目前指定之排序欄位</param>
        /// <param name="SortField">目前的排序欄位</param>
        /// <param name="SortType">目前的排序方法("Asc" or "Desc")</param>
        /// <param name="ResultSortField">輸出之排序欄位</param>
        /// <param name="ResultSortAction">輸出之排序方法</param>
        private void _SetSortField(string DefaultField, string DefaultAction, string Field, string SortField, string SortType, out string ResultSortField, out string ResultSortAction)
        {
            // 修正參數
            if (SortField == null) SortField = "";

            if (SortType == null) SortType = "";

            if (Field != null)
            {
                // 變更排序欄位或方法排序
                if (SortField.Equals(Field))
                {
                    if (SortType.Equals("Asc"))
                        ResultSortAction = "Desc";
                    else
                        ResultSortAction = "Asc";
                }
                else
                {
                    ResultSortAction = SortType;
                }

                ResultSortField = Field;
            }
            else if (SortField.Length > 0)
            {
                // 指定排序欄位
                ResultSortField = SortField;

                if (SortType.Length > 0)
                    ResultSortAction = SortType;
                else
                    ResultSortAction = "Desc";
            }
            else
            {
                // 指定預設值
                ResultSortField = DefaultField;
                ResultSortAction = DefaultAction;
            }
        }

        /// <summary>
        /// 重設所有搜尋條件
        /// </summary>
        private T1 UpdateSearchRouter<T1>(T1 router, int? page, NameValueCollection postForm, NameValueCollection getForm, bool IsSearch = false) where T1 : SortModel
        {
            // 取得所有屬性
            PropertyInfo[] i = typeof(T1).GetProperties();
            foreach (PropertyInfo property in i)
            {
                if (!clearSkipFields.Contains(property.Name))
                {
                    if (IsSearch)
                    {
                        //carl 20131106 
                        //重新搜尋時不要將Search開頭的欄位補上
                        object value = postForm[property.Name]; //?? getForm[property.Name];
                        if (!property.Name.ToLower().StartsWith("search"))
                        {
                            value = postForm[property.Name] ?? getForm[property.Name];
                        }
                        else
                        {
                            if (value == null)
                            {
                                value = "";
                            }
                        }

                        if (value != null)
                        {
                            EntityTool.SetProperty(property, router, value);
                        }
                    }
                    else
                    {
                        object value = postForm[property.Name] ?? getForm[property.Name];
                        if (value != null)
                        {
                            EntityTool.SetProperty(property, router, value);
                        }
                    }
                }
            }

            return router;
        }

        /// <summary>
        /// 清空所有搜尋條件
        /// </summary>
        private T1 ClearSearchRouter<T1>(T1 router) where T1 : SortModel
        {
            // 取得所有屬性
            PropertyInfo[] i = typeof(T1).GetProperties();
            foreach (PropertyInfo property in i)
            {
                if (!clearSkipFields.Contains(property.Name))
                {
                    if (property.PropertyType.IsValueType)
                    {
                        EntityTool.SetProperty(property, router, Activator.CreateInstance(property.PropertyType));
                    }
                    else
                    {
                        EntityTool.SetProperty(property, router, null);
                    }
                }
            }

            return router;
        }

        #endregion
    }
}