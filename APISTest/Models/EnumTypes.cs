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

        /// <summary>LAE</summary>
        [Description("LAE")]
        LAE = 4,
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
    /// 預估年營業額
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

    /// <summary>
    /// 報價方式
    /// </summary>
    public enum EnumRequestType
    {
        /// <summary>EX-WORK</summary>
        [Description("EX-WORK")]
        EXWORK = 1,

        /// <summary>DAP</summary>
        [Description("DAP")]
        DAP = 2,

        /// <summary>HUB</summary>
        [Description("HUB")]
        HUB = 3,

        /// <summary>FOB</summary>
        [Description("FOB")]
        FOB = 4,

        /// <summary>其他</summary>
        [Description("其他")]
        Others = 5,
    }

    /// <summary>
    /// LOB
    /// </summary>
    public enum EnumLOB
    {
        /// <summary>VLS</summary>
        [Description("VLS")]
        VLS = 1,

        /// <summary>VSA</summary>
        [Description("VSA")]
        VSA = 2,

        /// <summary>MCM</summary>
        [Description("MCM")]
        MCM = 3,
    }

    /// <summary>
    /// RDR Status
    /// </summary>
    public enum EnumRDRStatus
    {
        /// <summary>RFQ</summary>
        [Description("RFQ")]
        RFQ = 1,

        /// <summary>Awarded</summary>
        [Description("Awarded")]
        Awarded = 2,

        /// <summary>Fail</summary>
        [Description("Fail")]
        Fail = 3,
    }

    /// <summary>
    /// 設計驗證階段
    /// </summary>
    public enum EnumDesignVerify
    {
        /// <summary>PTT</summary>
        [Description("PTT")]
        PTT = 1,

        /// <summary>EVT</summary>
        [Description("EVT")]
        EVT = 2,

        /// <summary>DVT</summary>
        [Description("DVT")]
        DVT = 3,

        /// <summary>PVT</summary>
        [Description("PVT")]
        PVT = 4,

        /// <summary>MP</summary>
        [Description("MP")]
        MP = 5,
    }
}