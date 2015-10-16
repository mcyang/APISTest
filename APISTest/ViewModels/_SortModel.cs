using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APISTest.ViewModels
{
    /// <summary>
    /// 排序模型
    /// </summary>
    public interface ISortModel : ICloneable
    {
        /// <summary>目前頁數</summary>
        int? NowPage { get; set; }

        /// <summary>目前排序欄位</summary>
        string SortField { get; set; }

        /// <summary>排序方法</summary>
        string SortAction { get; set; }

        /// <summary>變更排序欄位</summary>
        string Field { get; set; }


        /// <summary>
        /// 設定排序資訊
        /// </summary>
        /// <param name="sortField">排序欄位</param>
        /// <param name="sortAction">排序方法</param>
        void SetSortInfo(string sortField, string sortAction);

        /// <summary>
        /// 複製排序資訊
        /// </summary>
        void SetSearch(ISortModel searchRouter);

        /// <summary>
        /// 變更排序欄位後輸出成 object
        /// </summary>
        /// <param name="FieldName">排序欄位</param>
        ISortModel ChangeField(string FieldName);

        /// <summary>
        /// 初始化部分 RouteValue 屬性(base)
        /// (此方法可在繼承後 override, 但還是要記得呼叫 base.reSetRoutValue())
        /// </summary>
        ISortModel reSetRoutValue();
    }

    /// <summary>
    /// 排序相關方法及欄位
    /// </summary>
    public class SortModel : ISortModel
    {
        /// <summary>目前頁數</summary>
        public int? NowPage { get; set; }

        /// <summary>目前排序欄位</summary>
        public string SortField { get; set; }

        /// <summary>排序方法</summary>
        public string SortAction { get; set; }

        /// <summary>變更排序欄位</summary>
        public string Field { get; set; }

        /// <summary>
        /// 設定排序資訊
        /// </summary>
        /// <param name="sortField">排序欄位</param>
        /// <param name="sortAction">排序方法</param>
        public void SetSortInfo(string sortField, string sortAction)
        {
            this.SortAction = sortAction;
            this.SortField = sortField;
            this.Field = sortField;
        }

        /// <summary>
        /// 複製排序資訊
        /// </summary>
        /// <param name="searchRouter"></param>
        public virtual void SetSearch(SortModel searchRouter)
        {
            this.SortAction = searchRouter.SortAction;
            this.SortField = searchRouter.SortField;
            this.Field = searchRouter.SortField;
            this.NowPage = searchRouter.NowPage;
        }

        /// <summary>
        /// 變更排序欄位後輸出成 object
        /// </summary>
        /// <param name="FieldName">排序欄位</param>
        public SortModel ChangeField(string FieldName)
        {
            SortModel cloneObject = this.Clone();
            cloneObject.Field = FieldName;
            return cloneObject;
        }

        /// <summary>
        /// 初始化部分 RouteValue 屬性(base)
        /// (此方法可在繼承後 override, 但還是要記得呼叫 base.reSetRoutValue())
        /// </summary>
        public virtual SortModel reSetRoutValue()
        {
            SortModel newValue = (SortModel)this.ChangeField(null);
            newValue.NowPage = null;

            return newValue;
        }

        /// <summary>
        /// 複製此物件
        /// </summary>
        public SortModel Clone()
        {
            return (SortModel)this.MemberwiseClone();
        }

        /// <summary>
        /// 複製此物件
        /// </summary>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// 複製排序資訊
        /// </summary>
        /// <remarks>實作介面轉換機制</remarks>
        public void SetSearch(ISortModel searchRouter)
        {
            this.SetSearch(searchRouter);
        }

        /// <summary>
        /// 變更排序欄位後輸出成 object
        /// </summary>
        /// <remarks>實作介面轉換機制</remarks>
        ISortModel ISortModel.ChangeField(string FieldName)
        {
            return this.ChangeField(FieldName);
        }

        /// <summary>
        /// 初始化部分 RouteValue 屬性
        /// </summary>
        /// <remarks>實作介面轉換機制</remarks>
        ISortModel ISortModel.reSetRoutValue()
        {
            return this.reSetRoutValue();
        }
    }
}