using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Reflection;

namespace APISTest.Tools
{
    /// <summary>
    /// 資料表物件工具
    /// </summary>
    public class EntityTool
    {
        #region 複製物件函式

        /// <summary>
        /// 複製資料庫物件[僅複製屬性, 不複製關聯或實體屬性]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public T CloneEntity<T>(T entity)
        {
            T targetEntity = System.Activator.CreateInstance<T>();
            return CloneEntity<T>(entity, targetEntity);
        }

        /// <summary>
        /// 複製資料庫物件[僅複製屬性, 不複製關聯或實體屬性]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="targetEntity"></param>
        public T CloneEntity<T>(T entity, T targetEntity)
        {
            // 取得屬性清單(public)
            PropertyInfo[] i = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (PropertyInfo property in i)
            {
                // 先取得屬性的型別
                var pType = property.PropertyType;

                // 必須是可寫入且不為索引欄位
                bool IsBaseAttribute = property.CanWrite && property.Name != "ID";

                // 去除實體關聯屬性
                IsBaseAttribute = IsBaseAttribute && !pType.IsSubclassOf(typeof(System.Data.Objects.DataClasses.EntityObject));
                IsBaseAttribute = IsBaseAttribute && !pType.IsSubclassOf(typeof(System.Data.Objects.DataClasses.EntityReference<>));
                IsBaseAttribute = IsBaseAttribute && !pType.IsSubclassOf(typeof(System.Data.Objects.DataClasses.RelatedEnd));

                // 去除實體類別索引屬性
                IsBaseAttribute = IsBaseAttribute && pType != typeof(System.Data.EntityKey);
                IsBaseAttribute = IsBaseAttribute && pType != typeof(System.Data.EntityState);

                if (IsBaseAttribute)
                {
                    try
                    {
                        property.SetValue(targetEntity, property.GetValue(entity, null), null);
                    }
                    catch
                    {
                        // 將錯誤訊息記錄至 log
                        //ErrorHelpers.ExceptionLog(ex, ApplicationErrorType.ValidError);
                    }
                }
            }

            return targetEntity;
        }

        #endregion

        #region SpellFormCollection

        ///// <summary>
        ///// 指定 FormCollection 的值至指定的物件
        ///// </summary>
        //public T SpellFormCollection<T>(Dictionary<string, object> fc, T obj)
        //{
        //    return SpellFormCollection<T>("{0}", fc, obj);
        //}

        ///// <summary>
        ///// 指定 FormCollection 的值至指定的物件
        ///// </summary>
        //public T SpellFormCollection<T>(NameValueCollection fc, T obj)
        //{
        //    Dictionary<string, object> data = fc.AllKeys.ToDictionary(k => k, v => (object)fc[v]);
        //    return SpellFormCollection<T>("{0}", data, obj);
        //}

        ///// <summary>
        ///// 指定 FormCollection 的值至指定的物件
        ///// </summary>
        //public T SpellFormCollection<T>(string foramt, NameValueCollection fc, T obj)
        //{
        //    Dictionary<string, object> data = fc.AllKeys.ToDictionary(k => k, v => (object)fc[v]);
        //    return SpellFormCollection<T>(foramt, data, obj);
        //}

        ///// <summary>
        ///// 指定 FormCollection 的值至指定的物件
        ///// </summary>
        //public T SpellFormCollection<T>(string foramt, Dictionary<string, object> fc, T obj)
        //{
        //    // 若傳進來的物件為 null, 則 new 物件 (base)
        //    if (obj == null)
        //    {
        //        obj = System.Activator.CreateInstance<T>();
        //    }

        //    // 取得所有屬性
        //    PropertyInfo[] i = typeof(T).GetProperties();

        //    foreach (PropertyInfo property in i)
        //    {
        //        // 不對 ID 及 LangNo 做任何變更
        //        if (property.Name != "ID")
        //        {
        //            // 若抓不到有分語系的值, 則抓無語系分類的值
        //            object inputValue = null;
        //            if (fc.ContainsKey(string.Format(foramt, property.Name)))
        //            {
        //                inputValue = fc[string.Format(foramt, property.Name)];
        //                SetProperty(property, obj, inputValue);
        //            }
        //        }
        //    }

        //    // 回傳有值的資料庫物件
        //    return obj;
        //}

        /// <summary>
        /// 依 type 進行轉換, 若轉換成功再指定值
        /// </summary>
        /// <param name="property">屬性</param>
        /// <param name="obj">物件</param>
        /// <param name="value">要轉換的指定值</param>
        public void SetProperty(PropertyInfo property, object obj, object value)
        {
            Type type = property.PropertyType;
            if (value == null)
            {
                // 若為基本型別, 則預設值不可為 null
                if (type.IsValueType)
                {
                    value = Activator.CreateInstance(type);
                }
            }
            else
            {
                if (type == typeof(bool) || type == typeof(bool?) && value is string)
                {
                    // 針對 bool 做前置的參數修正
                    value = (value ?? "").ToString().Split(',').Where(i => i.Length > 0).FirstOrDefault();
                }

                if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    // 若 type 為 Nullable<T>
                    NullableConverter convert = new NullableConverter(type);
                    try
                    {
                        // 進行轉換
                        value = convert.ConvertFrom(value);
                    }
                    catch
                    {
                        // 轉換失敗則直接指定 null 值
                        value = null;
                    }
                }
                else
                {
                    try
                    {
                        // 基本型別轉換
                        value = System.Convert.ChangeType(value, type);
                    }
                    catch
                    {
                        if (type.IsValueType)
                        {
                            value = Activator.CreateInstance(type);
                        }
                        else
                        {
                            value = null;
                        }
                    }
                }
            }

            // 指定值
            property.SetValue(obj, value, null);
        }

        #endregion

        /// <summary>
        /// 取得自訂 CheckBoxBit 的正確填寫值
        /// </summary>
        /// <param name="value"></param>
        public bool GetBitCheckBoxValue(string value)
        {
            bool result;
            return bool.TryParse((value ?? "").Split(',').FirstOrDefault(), out result) && result;
        }
    }
}