using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using System.Web.Routing;
using APIS.Models;
using APIS.Tools;
using APIS.Unity;

namespace APIS.Helpers
{
    public static class SelectionHelper
    {
        /// <summary>
        /// 從 Enum Type 產生下拉選單
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">第1個參數一定要填「this HtmlHelper helper」</param>
        /// <param name="Name">下拉選單名稱</param>
        /// <param name="selected">選取項目</param>
        /// <param name="hasAllSelection">是否加入「不拘」選項</param>
        /// <param name="htmlAttribute">CSS屬性</param>
        /// <returns></returns>
        public static IHtmlString DDL_FromEnum<T>(this HtmlHelper helper, string Name, string selected, bool hasAllSelection = true, object htmlAttribute = null)
        {
            //建立下拉選單
            List<SelectListItem> list = new List<SelectListItem>();

            if (hasAllSelection)
            {
                list.Add(new SelectListItem { Text = "請選擇", Value = "" });
            }

            if (string.IsNullOrEmpty(selected))
            {
                selected = "";
            }

            foreach (Enum item in Enum.GetValues(typeof(T)))
            {
                string desc = EnumMapTool.GetDescription(item);
                if (!string.IsNullOrEmpty(desc))
                {
                    list.Add(new SelectListItem
                    {
                        Text = desc,
                        Value = Convert.ToInt32(item).ToString(),
                        Selected = selected == desc //.Split(',').Contains(Convert.ToInt32(item).ToString())
                    });
                }
            }

            return helper.DropDownList(Name, list, htmlAttribute);
        }

        /// <summary>
        /// 依客戶群下拉選單-從資料庫產生
        /// </summary>
        /// <param name="helper">第1個參數一定要填「this HtmlHelper helper」</param>
        /// <param name="Name">下拉選單名稱</param>
        /// <param name="selected">選取項目</param>
        /// <param name="hasAllSelection">是否加入「不拘」選項</param>
        /// <param name="htmlAttribute">CSS屬性</param>
        /// <returns></returns>
        public static IHtmlString DDL_CustomerTeam(this HtmlHelper helper, string Name, int selected, bool hasAllSelection = false, object htmlAttribute = null)
        {
            #region 錯誤訊息
            if (String.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("必須給這個下拉選單 DDL_CustomerTeam 一個 Tag Name", "Name");
            }
            #endregion

            List<SelectListItem> list = new List<SelectListItem>(); //建立下拉選單

            if (hasAllSelection)
            {
                list.Add(new SelectListItem { Text = "不拘", Value = "" });
            }


            using (JohnTestEntities ddl_db = new JohnTestEntities())
            {
                foreach (var item in ddl_db.CustomerTeams.Where(p => p.IsDelete == false))
                {
                    list.Add(new SelectListItem()
                    {
                        Text = item.Code,
                        Value = item.ID.ToString(),
                        Selected = selected == item.ID ? true : false
                    });
                }

            }

            return helper.DropDownList(Name, list, htmlAttribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="Name"></param>
        /// <param name="selected"></param>
        /// <param name="LOBID">由LOB參數來決定撈對應的客戶群</param>
        /// <param name="htmlAttribute"></param>
        /// <returns></returns>
        public static IHtmlString DDL_CustomerTeam(this HtmlHelper helper, string Name, int selected, int? LobID, object htmlAttribute = null)
        {
            List<SelectListItem> list = new List<SelectListItem>(); //建立下拉選單
            list.Add(new SelectListItem { Text = "請選擇", Value = "" });

            if (LobID != null && LobID > 0)
            {
                switch (LobID)
                {
                    case 1:
                        //VLS
                        using (JohnTestEntities db = new JohnTestEntities())
                        {
                            foreach (var item in db.CustomerTeams.Where(p => p.IsDelete == false))
                            {
                                list.Add(new SelectListItem()
                                {
                                    Text = item.Code,
                                    Value = item.ID.ToString(),
                                    Selected = selected == item.ID ? true : false
                                });
                            }
                        }
                        break;
                    case 2:
                        //VSA
                        //暫無資料
                        break;
                    case 3:
                        //MCM
                        //暫無資料
                        break;
                }
            }


            return helper.DropDownList(Name, list, htmlAttribute);
        }

        /// <summary>
        /// 依車廠下拉選單-從資料庫產生
        /// </summary>
        /// <param name="helper">第1個參數一定要填「this HtmlHelper helper」</param>
        /// <param name="Name">下拉選單名稱</param>
        /// <param name="selected">選取項目</param>
        /// <param name="hasAllSelection">是否加入「不拘」選項</param>
        /// <param name="htmlAttribute">CSS屬性</param>
        /// <returns></returns>
        public static IHtmlString DDL_CarMaker(this HtmlHelper helper, string Name, int? selected, bool hasAllSelection = false, object htmlAttribute = null)
        {
            #region 錯誤訊息
            if (String.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("必須給這個下拉選單 DDL_CarMaker 一個Tag Name", "Name");
            }
            #endregion

            //建立下拉選單
            List<SelectListItem> list = new List<SelectListItem>();

            if (hasAllSelection)
            {
                list.Add(new SelectListItem { Text = "不拘", Value = "" });
            }

            using (JohnTestEntities ddl_db = new JohnTestEntities())
            {
                foreach (var item in ddl_db.CarMakers.Where(p => p.IsDelete == false).OrderBy(p => p.Name))
                {
                    list.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.ID.ToString(),
                        Selected = selected == item.ID ? true : false
                    });
                }
            }
            return helper.DropDownList(Name, list, htmlAttribute);
        }

        public static IHtmlString DDL_ProductGroup(this HtmlHelper helper, string Name, int selected, bool hasAllSelection = false, object htmlAttribute = null)
        {
            #region 錯誤訊息
            if (String.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("必須給這個下拉選單 DDL_CarMaker 一個Tag Name", "Name");
            }
            #endregion

            //建立下拉選單
            List<SelectListItem> list = new List<SelectListItem>();

            if (hasAllSelection)
            {
                list.Add(new SelectListItem { Text = "不拘", Value = "" });
            }

            using (JohnTestEntities ddl_db = new JohnTestEntities())
            {
                foreach (var item in ddl_db.ProductGroups.Where(p => p.IsDelete == false))
                {
                    list.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.ID.ToString(),
                        Selected = selected == item.ID ? true : false
                    });
                }
            }

            return helper.DropDownList(Name, list, htmlAttribute);
        }

        public static IHtmlString DDL_Customers(this HtmlHelper helper, string Name, int TeamID, int selected, bool hasAllSelection = false, object htmlAttribute = null)
        {
            #region 錯誤訊息
            if (String.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("必須給這個下拉選單 DDL_CarMaker 一個Tag Name", "Name");
            }
            #endregion

            //建立下拉選單
            List<SelectListItem> list = new List<SelectListItem>();

            if (hasAllSelection)
            {
                list.Add(new SelectListItem { Text = "不拘", Value = "" });
            }

            using (JohnTestEntities ddl_db = new JohnTestEntities())
            {
                foreach (var item in ddl_db.Customers.Where(p => p.IsDelete == false && p.ParentID == TeamID))
                {
                    list.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.ID.ToString(),
                        Selected = selected == item.ID ? true : false
                    });
                }
            }

            return helper.DropDownList(Name, list, htmlAttribute);
        }

        private static IHtmlString CheckBoxList(HtmlHelper helper, List<SelectListItem> list, string Name, string selected, object htmlAttribute = null)
        {
            string attributeList = htmlAttribute.ToAttributeList();

            StringBuilder sb = new StringBuilder();

            foreach (var i in list)
            {
                sb.AppendFormat("<input name=\"{0}\" value=\"{1}\" {2} type=\"checkbox\" {4}/>{3} &nbsp",
                Name,
                i.Value,
                selected.Contains(i.Value) ? "checked=\"checked\"" : "",
                string.Format("<span>{0}</span>", i.Text),
                attributeList);

                sb.AppendLine();
            }

            return new HtmlString(sb.ToString());
        }
    }


}