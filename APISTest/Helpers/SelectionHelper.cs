using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using APISTest.Tools;

namespace APISTest.Helpers
{
    public static class SelectionHelper
    {
        /// <summary>
        /// 從 Enum Type 產生下拉選單
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <param name="Name"></param>
        /// <param name="selected"></param>
        /// <param name="hasAllSelection"></param>
        /// <param name="htmlAttribute"></param>
        /// <returns></returns>
        public static IHtmlString DDL_FromEnum<T>(this HtmlHelper helper, string Name, string selected, bool hasAllSelection = true, object htmlAttribute = null)
        {
            if (string.IsNullOrEmpty(selected))
            {
                selected = "";
            }
            List<SelectListItem> list = new List<SelectListItem>();
            if (hasAllSelection)
            {
                list.Add(new SelectListItem { Text = "請選擇", Value = "" });
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
        /// 依客戶群下拉選單
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="Type"></param>
        /// <param name="Name"></param>
        /// <param name="selected"></param>
        /// <param name="htmlAttribute"></param>
        /// <returns></returns>
        public static IHtmlString DDL_CustomerTeam(this HtmlHelper helper, string Name, int selected, bool hasAllSelection = false, object htmlAttribute = null)
        {
            List<SelectListItem> list = new List<SelectListItem>(); //建立下拉選單

            using (APISTest.Models.JohnTestEntities db = new APISTest.Models.JohnTestEntities())
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

            //if (hasAllSelection)
            //{
            //    list.Add(new SelectListItem { Text = "不拘", Value = "" });
            //}
            
            return helper.DropDownList(Name, list, htmlAttribute);
        }
    }
}