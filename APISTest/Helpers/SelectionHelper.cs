using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using APISTest.Models;
using APISTest.Tools;

namespace APISTest.Helpers
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
                        Selected = selected.Split(',').Contains(Convert.ToInt32(item).ToString())
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
            //建立下拉選單
            List<SelectListItem> list = new List<SelectListItem>();

            if (hasAllSelection)
            {
                list.Add(new SelectListItem { Text = "不拘", Value = "" });
            }

            using (JohnTestEntities ddl_db = new JohnTestEntities())
            {
                foreach (var item in ddl_db.CarMakers.Where(p => p.IsDelete == false))
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
    }
}