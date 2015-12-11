using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIS.Models
{
    [MetadataType(typeof(_MetaDataClass))]
    public partial class UploadFile
    {
        private class _MetaDataClass
        {
            //附件資訊
            [DisplayName("附件類別")]
            public int UploadType { get; set; }
            [DisplayName("檔名")]
            public string Name { get; set; }
            [DisplayName("路徑")]
            public string Location { get; set; }
            [DisplayName("檔案類型")]
            public string ContentType { get; set; }
            [DisplayName("檔案大小")]
            public int FileSize { get; set; }
            [DisplayName("產品規範")]
            public bool IsProductSpec { get; set; }
            [DisplayName("測試規範")]
            public bool IsTestInstruction { get; set; }
            [DisplayName("物料清單")]
            public bool IsCustomerBOM { get; set; }
            [DisplayName("配阻表")]
            public bool IsBinResistorTable { get; set; }
            [DisplayName("PCBA圖紙")]
            public bool IsPCBA { get; set; }
            [DisplayName("PCB圖紙")]
            public bool IsPCB { get; set; }
            [DisplayName("線材圖")]
            public bool IsHarness { get; set; }
            [DisplayName("GERBER")]
            public bool IsGerber { get; set; }
            [DisplayName("座標檔")]
            public bool IsCoordinate { get; set; }
            [DisplayName("電路圖")]
            public bool IsSchematics { get; set; }
            [DisplayName("零件位置圖")]
            public bool IsComp { get; set; }
            [DisplayName("PV Test Plan")]
            public bool IsPVTestPlan { get; set; }
            [DisplayName("SVRF")]
            public bool IsSVRF { get; set; }
        }
    }
}