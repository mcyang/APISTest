using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace APISTest.Models
{
    /// <summary>
    /// 客戶群
    /// </summary>
    public enum CustomerTeamType
    {
        /// <summary>AL</summary>
        [Description("AL")]
        AL = 1,

        /// <summary>HELLA</summary>
        [Description("HL")]
        HL = 2,

        /// <summary>KOITO</summary>
        [Description("KOITO")]
        KOITO = 3,

        /// <summary>VALEO</summary>
        [Description("VALEO")]
        VALEO = 4,

        /// <summary>大億</summary>
        [Description("TY")]
        TY = 4,
    }
}