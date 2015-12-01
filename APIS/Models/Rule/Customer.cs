using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIS.Models
{
    [MetadataType(typeof(_MetaDataClass))]
    public partial class Customer
    {
        private class _MetaDataClass
        {
            [DisplayName("客戶ID")]
            public int ID { get; set; }

            [DisplayName("客戶群ID")]
            [Required(ErrorMessage = "不可為空")]
            public short ParentID { get; set; }

            [DisplayName("客戶代碼")]
            [Required(ErrorMessage = "不可為空")]
            public string Code { get; set; }

            [DisplayName("客戶名稱")]
            [Required(ErrorMessage = "不可為空")]
            public string Name { get; set; }

            [DisplayName("是否刪除")]
            public bool IsDelete { get; set; }
        }
    }
}