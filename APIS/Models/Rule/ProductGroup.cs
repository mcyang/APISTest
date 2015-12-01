using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIS.Models
{
    [MetadataType(typeof(_MetaDataClass))]
    public partial class ProductGroup
    {
        private class _MetaDataClass
        {
            [DisplayName("產品別ID")]
            public int ID { get; set; }
            
            [DisplayName("產品別代碼")]
            [Required(ErrorMessage = "不可為空")]
            public string Code { get; set; }

            [DisplayName("產品別名稱")]
            [Required(ErrorMessage = "不可為空")]
            public string Name { get; set; }

            [DisplayName("是否刪除")]
            public bool IsDelete { get; set; }
        }
    }
}