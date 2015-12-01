using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIS.Models
{
    [MetadataType(typeof(_MetaDataClass))]
    public partial class CustomerTeam
    {
        private class _MetaDataClass
        {
            [DisplayName("客戶群ID")]
            public int ID { get; set; }

            [DisplayName("客戶群代碼")]
            [Required(ErrorMessage = "不可為空")]
            public string Code { get; set; }

            [DisplayName("客戶群名稱")]
            [Required(ErrorMessage = "不可為空")]
            public string Name { get; set; }

            [DisplayName("是否刪除")]
            public bool IsDelete { get; set; }
        }
    }
}