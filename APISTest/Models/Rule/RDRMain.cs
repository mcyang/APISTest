using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APISTest.Models
{
    [MetadataType(typeof(_MetaDataClass))]
    public partial class RDRMain
    {
        private class _MetaDataClass
        {
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
            [DisplayName("RFQ Date")]
            public System.DateTime RFQDate { get; set; }

            [Required(ErrorMessage = "不可為空")]
            [DisplayName("報價需求日")]
            public System.DateTime AcquisitionDate { get; set; }

            [Required(ErrorMessage = "不可為空")]
            [DisplayName("SOP Date")]
            public System.DateTime SOPDate { get; set; }

            [Required(ErrorMessage = "不可為空")]
            [DisplayName("EOL Date")]
            public System.DateTime EOLDate { get; set; }

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
        }
    }
}