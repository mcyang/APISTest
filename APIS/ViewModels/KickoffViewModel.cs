using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIS.Models;

namespace APIS.ViewModels
{
    public class KickoffIndexViewModel
    {
        public int ID { get; set; }
        public string RDRNumber { get; set; }
        public string ProjectName { get; set; }
        public string CustomerTeam { get; set; }
        public int Status { get; set; }

        //透過 EnumMapTool.GetDescription() 將值轉換為列舉描述
        public string _Status
        {
            get { return APIS.Tools.EnumMapTool.GetDescription((APIS.Models.EnumRDRStatus)Status); }
        }
    }

    public class KickoffListViewModel
    {
        public int ID { get; set; }
        public string RDRNumber { get; set; }
        public string ProjectName { get; set; }
    }

    public class KickoffReportViewModel
    {
        public Kickoff kickoff{ get; set; }
        public string ID { get { return kickoff.ID.ToString(); }  }
        public string KickoffNumber { get { return kickoff.KickoffNumber; } }
        public bool IsReliabilityTest { get { return kickoff.IsReliabilityTest; } }
        public bool IsPPAP { get { return kickoff.IsPPAP; } }
        public bool IsCustomerDocForm { get { return kickoff.IsCustomerDocForm; } }
        public bool IsLiteonDocForm { get { return kickoff.IsLiteonDocForm; } }
        public bool IsLiteonConFirm { get { return kickoff.IsLiteonConFirm; } }
        public bool IsCustomerConFirm { get { return kickoff.IsCustomerConFirm; } }
        public string Level { get { return kickoff.Level.ToString(); } }

        public string KickoffDate
        {
            get { return string.Format("{0:yyyy-MM-dd}", kickoff.KickoffDate); }
        }
        public string ModifyTime
        {
            get { return string.Format("{0:yyyy-MM-dd}", kickoff.ModifyTime); }
        }
    }

    public class KickoffDetailsReportViewModel
    {
        public KickoffDetail kickDetail { get; set; }
        public string Stage { get { return kickDetail.Stage; } }
        public int Quantity { get { return kickDetail.Quantity; } }
        public string Description { get { return kickDetail.Description; } }

        public string RequireDate
        {
            get { return string.Format("{0:yyyy-MM-dd}", kickDetail.RequireDate); }
        }
    }
}