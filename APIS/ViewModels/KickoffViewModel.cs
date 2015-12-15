using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
}