using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APISTest.ViewModels
{
    public class CustomerViewModel
    {
        //ID,Code,Name,ParentID,IsDelete
        public short ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public short CustomerTeamID { get; set; }
        public string CustomerTeamCode { get; set; }
        public bool IsDelete { get; set; }
    }
}