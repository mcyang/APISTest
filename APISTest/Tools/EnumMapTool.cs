using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Reflection;

namespace APISTest.Tools
{
    /// <summary>
    /// Enum 相關工具
    /// </summary>
    public class EnumMapTool
    {
        /// <summary>
        /// 取得自訂 Enum 描述
        /// </summary>
        /// <param name="item"></param>
        public static string GetDescription(object enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return "";
        }

        /// <summary>
        /// 依描述取得Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T FromDescription<T>(string description)
        {
            Type t = typeof(T);
            foreach (FieldInfo fi in t.GetFields())
            {
                object[] attrs =
            fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    foreach (DescriptionAttribute attr in attrs)
                    {
                        if (attr.Description.Equals(description))
                            return (T)fi.GetValue(null);
                    }
                }
            }
            return default(T);
        }
    }
}