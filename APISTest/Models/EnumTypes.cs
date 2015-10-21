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

    /// <summary>
    /// 幣別
    /// </summary>
    public enum CurrencyType
    {
        /// <summary>NTD</summary>
        [Description("NTD")]
        NTD = 1,

        /// <summary>RMB</summary>
        [Description("RMB")]
        RMB = 2,
    }

    /// <summary>
    /// 量產地
    /// </summary>
    public enum SiteType
    {
        /// <summary>高雄LAK</summary>
        [Description("LAK")]
        LAK = 1,

        /// <summary>廣州LGA</summary>
        [Description("LGA")]
        LGA = 2,

        /// <summary>墨西哥LAMX</summary>
        [Description("LAMX")]
        LAMX = 3,
    }

    /// <summary>
    /// 報價類型
    /// </summary>
    public enum QuotationType
    {
        /// <summary>正式RFQ</summary>
        [Description("正式RFQ")]
        RFQ = 1,

        /// <summary>參考報價</summary>
        [Description("參考報價")]
        Price_Indication = 2,
    }

    /// <summary>
    /// 把握度
    /// </summary>
    public enum CertaintyType
    {
        /// <summary>H</summary>
        [Description("H")]
        H = 1,

        /// <summary>M</summary>
        [Description("M")]
        M = 2,

        /// <summary>L</summary>
        [Description("L")]
        L = 3,
    }
}