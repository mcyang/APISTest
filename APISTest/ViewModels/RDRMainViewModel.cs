using APISTest.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APISTest.ViewModels
{
    public class RDRMainViewModel
    {
        public RDRMain rdrMain {get;set;}

        public int ID { get; set; }

        [DisplayName("RDR#")]
        public string RDRNumber { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("專案名稱")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("客戶群名稱")]
        public int CustomerTeamID { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("客戶群代碼")]
        public string CustomerTeamCode { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("LOB")]
        public string LOB { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("量產地")]
        public string Site { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("車廠ID")]
        public int CarMakerID { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("車型")]
        public string CarModel { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("車型預估年銷售量")]
        public int EstimateSales { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("把握度")]
        public string Certainty { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("報價類型")]
        public string RequestClass { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("報價方式")]
        public string RequestType { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("報價貨幣")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("預估年營業額")]
        public int EstimateRevenue { get; set; }

        [DisplayName("單據狀態")]
        public int Status { get; set; }

        [DisplayName("建立時間")]
        public System.DateTime CreateTime { get; set; }

        [DisplayName("修改時間")]
        public System.DateTime ModifyTime { get; set; }

        [DisplayName("是否刪除")]
        public bool IsDelete { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DisplayName("建立者")]
        public string CreateUserID { get; set; }

        [Required(ErrorMessage = "不可為空")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RFQDate
        {
            get { return rdrMain.RFQDate; }
        }

        [Required(ErrorMessage = "不可為空")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AcquisitionDate
        {
            get { return rdrMain.AcquisitionDate; }
        }

        [Required(ErrorMessage = "不可為空")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        public DateTime SOPDate
        {
            get { return rdrMain.SOPDate; }
        }

        [Required(ErrorMessage = "不可為空")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        public DateTime EOLDate
        {
            get { return rdrMain.EOLDate; }
        }
    }

    //RDRMain 列表資料模型
    public class RDRMainIndexViewModel
    {
        public RDRMain rdrMain { get; set; }
        public string CarMaker { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RFQDate
        {
            get { return rdrMain.RFQDate; }
            set { }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AcquisitionDate
        {
            get { return rdrMain.AcquisitionDate; }
            set { }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        public DateTime SOPDate
        {
            get { return rdrMain.SOPDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        public DateTime EOLDate
        {
            get { return rdrMain.EOLDate; }
        }

    
    }

    public class RDRMainDetailsViewModel
    {
        public RDRMain rdrMain { get; set; }
        public string CarMaker { get; set; }
        

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RFQDate
        {
            get { return rdrMain.RFQDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AcquisitionDate
        {
            get { return rdrMain.AcquisitionDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}")]
        public DateTime SOPDate
        {
            get { return rdrMain.SOPDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}")]
        public DateTime EOLDate
        {
            get { return rdrMain.EOLDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreateDate
        {
            get { return rdrMain.CreateTime; }
        }

        public string EstimateRevenueText
        { get
          {
                return APISTest.Tools.EnumMapTool.GetDescription((EnumEstimateRevenue)rdrMain.EstimateRevenue);
          }
        }
    }

    public class RDRMainReportViewModel
    {
        public RDRMain rdrMain { get; set; }
        public string CarMakerName { get; set; }

        public string ID
        {
            get { return rdrMain.ID.ToString(); }
        }
        public string RDRNumber
        {
            get { return rdrMain.RDRNumber; }
        }
        public string CustomerTeamCode
        {
            get { return rdrMain.CustomerTeamCode; }
        }
        public string LOB
        {
            get { return rdrMain.LOB; }
        }
        public string Site
        {
            get { return rdrMain.Site; }
        }
        public string CarModel
        {
            get { return rdrMain.CarModel; }
        }
        public string EstimateSales
        {
            get { return rdrMain.EstimateSales.ToString(); }
        }
        public string RFQDate
        {
            get { return string.Format("{0:yyyy-MM-dd}", rdrMain.RFQDate); }
        }
        public string AcquisitionDate
        {
            get { return string.Format("{0:yyyy-MM-dd}", rdrMain.AcquisitionDate); }
        }
        public string SOPDate
        {
            get { return string.Format("{0:yyyy-MM}", rdrMain.SOPDate); }
        }
        public string EOLDate
        {
            get { return string.Format("{0:yyyy-MM}", rdrMain.EOLDate); }
        }
        public string Certainty
        {
            get { return rdrMain.Certainty; }
        }
        public string RequestClass
        {
            get { return rdrMain.RequestClass; }
        }
        public string RequestType
        {
            get { return rdrMain.RequestType; }
        }
        public string Currency
        {
            get { return rdrMain.Currency; }
        }
        public string ProjectName
        {
            get { return rdrMain.ProjectName; }
        }
        public string EstimateRevenueText
        {
            get
            {
                return APISTest.Tools.EnumMapTool.GetDescription((EnumEstimateRevenue)rdrMain.EstimateRevenue);
            }
        }
        public string CreateDate
        {
            get { return string.Format("{0:yyyy-MM-dd}", rdrMain.CreateTime); }
        }
    }
}