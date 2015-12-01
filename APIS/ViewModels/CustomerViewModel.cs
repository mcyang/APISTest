using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using APIS.Models;

namespace APIS.ViewModels
{
    public class CustomerViewModel
    {
        [DisplayName("客戶ID")]
        public int ID { get; set; }

        [DisplayName("客戶代碼")]
        [Required(ErrorMessage = "不可為空")]
        public string Code { get; set; }

        [DisplayName("客戶名稱")]
        [Required(ErrorMessage = "不可為空")]
        public string Name { get; set; }

        [DisplayName("是否刪除")]
        public bool IsDelete { get; set; }

        [DisplayName("客戶群ID")]
        public int CustomerTeamID { get; set; }

        [DisplayName("客戶群")]
        public string CustomerTeamCode { get; set; }
    }
}