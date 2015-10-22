using System.ComponentModel;

namespace APISTest.Models
{
    /// <summary>
    /// 客戶群
    /// </summary>
    public enum EnumCustomerTeam
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
    public enum EnumCurrency
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
    public enum EnumSite
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
    public enum EnumQuotation
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
    public enum EnumCertainty
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

    /// <summary>
    /// 車型預估年銷售量
    /// </summary>
    public enum EnumEstimateRevenue
    {
        /// <summary>小於2千萬</summary>
        [Description("小於2千萬")]
        Less2K = 1,

        /// <summary>2-5千萬</summary>
        [Description("2-5千萬")]
        Between2K5K = 2,

        /// <summary>5千萬-1億</summary>
        [Description("5千萬-1億")]
        Between5K1E = 3,

        /// <summary>1億以上</summary>
        [Description("1億以上")]
        More1E = 4,
    }
    
}