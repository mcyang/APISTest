//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace APISTest.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RDRModule
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string RDRNumber { get; set; }
        public string ModuleName { get; set; }
        public int ProductGroupID { get; set; }
        public int CustomerID { get; set; }
        public string EstimateProduct { get; set; }
        public string Attachment { get; set; }
        public string Remark { get; set; }
        public int ModuleVersion { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
