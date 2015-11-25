using System.Collections.Generic;
using APISTest.Models;

namespace APISTest.ViewModels
{
    // RDR表單ViewModel
    public class RDRViewModel
    {
        /// <summary>RDR主表明細ViewModel</summary>
        public RDRMainDetailsViewModel rdrMainDetail { get; set; }
        /// <summary>RDR機種資料明細ViewModel</summary>
        public List<RDRModuleDetailsViewModel> rdrModuleList { get; set; }
        /// <summary>RDR其他資訊明細ViewModel</summary>
        public RDRInfoViewModel rdrInfoDetail { get; set; }
    }

    
}