using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using APIS.Models;

namespace APIS.ViewModels
{
    public class UploadFileDetailsViewModel
    {
        //附件資訊
        public UploadFile uploadfile { get; set; }
        
        //其他資訊
        public string RDRNumber { get; set; }
        public string ModuleName { get; set; }
    }
}