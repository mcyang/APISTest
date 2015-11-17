using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APISTest.Models
{
    public interface IEntity
    {
        int ID { get; set; }
        //bool IsDelete { get; set; }
        DateTime CreateTime { get; set; }
        DateTime ModifyTime { get; set; }
        int CreateUserID { get; set; }
        int LastMaintainUserID { get; set; }
    }

    public interface IIsDelete
    {
        bool IsDelete { get; set; }
    }
}