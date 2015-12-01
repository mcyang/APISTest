using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIS.Models
{
    [MetadataType(typeof(_MetaDataClass))]
    public partial class CarMaker
    {
        private class _MetaDataClass
        {
            [DisplayName("車廠ID")]
            public int ID { get; set; }
            
            [DisplayName("車廠代碼")]
            [Required(ErrorMessage = "不可為空")]
            public string Code { get; set; }

            [DisplayName("車廠名稱")]
            [Required(ErrorMessage = "不可為空")]
            public string Name { get; set; }

            [DisplayName("是否刪除")]
            public bool IsDelete { get; set; }
        }
    }
}