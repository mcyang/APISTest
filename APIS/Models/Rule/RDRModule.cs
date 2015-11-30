using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIS.Models
{
    [MetadataType(typeof(_MetaDataClass))]
    public partial class RDRModule
    {
        private class _MetaDataClass
        {
            [DisplayName("RDR表單索引")]
            public int ParentID { get; set; }

            [DisplayName("RDR#")]
            public string RDRNumber { get; set; }

            [DisplayName("機種名稱")]
            public string ModuleName { get; set; }

            [DisplayName("產品別")]
            public int ProductGroupID { get; set; }

            [DisplayName("交貨地")]
            public int CustomerID { get; set; }

            [DisplayName("預估年產量")]
            public string EstimateProduct { get; set; }

            [DisplayName("附件")]
            public string Attachment { get; set; }

            [DisplayName("備註")]
            public string Remark { get; set; }

            [DisplayName("版本號")]
            public int ModuleVersion { get; set; }

            [DisplayName("建立時間")]
            public DateTime CreateTime { get; set; }
        }
    }
}