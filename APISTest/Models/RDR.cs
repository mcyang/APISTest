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
    
    public partial class RDR
    {
        public int ID { get; set; }
        public string RDRNumber { get; set; }
        public short CustomerTeamID { get; set; }
        public string CustomerTeamCode { get; set; }
        public string Site { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> CarMakerID { get; set; }
        public string CarModel { get; set; }
        public Nullable<int> EstimateSales { get; set; }
        public Nullable<int> LOBID { get; set; }
        public Nullable<System.DateTime> RFQDate { get; set; }
        public Nullable<System.DateTime> AcquisitionDate { get; set; }
        public Nullable<System.DateTime> SOPDate { get; set; }
        public Nullable<System.DateTime> EOLDate { get; set; }
        public string Certainty { get; set; }
        public string RequestType { get; set; }
        public string RequestClass { get; set; }
        public string Currency { get; set; }
        public string ProjectName { get; set; }
        public string ModuleName { get; set; }
        public Nullable<int> EstimateRevenue { get; set; }
    }
}
