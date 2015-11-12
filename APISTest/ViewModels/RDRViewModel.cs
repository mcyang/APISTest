using System.Collections.Generic;
using APISTest.Models;

namespace APISTest.ViewModels
{
    public class RDRViewModel
    {
        //#region RDRMain相關欄位
        //public RDRMain rdrMain { get; set; }

        //public string CarMakerName { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime RFQDate
        //{
        //    get { return rdrMain.RFQDate; }
        //}

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime AcquisitionDate
        //{
        //    get { return rdrMain.AcquisitionDate; }
        //}

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        //public DateTime SOPDate
        //{
        //    get { return rdrMain.SOPDate; }
        //}

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        //public DateTime EOLDate
        //{
        //    get { return rdrMain.EOLDate; }
        //}

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime CreateDate
        //{
        //    get { return rdrMain.CreateTime; }
        //}

        //public string EstimateRevenueText
        //{
        //    get
        //    {
        //        return APISTest.Tools.EnumMapTool.GetDescription((EnumEstimateRevenue)rdrMain.EstimateRevenue);
        //    }
        //}
        //#endregion
        public RDRMainDetailsViewModel rdrMainDetail { get; set; }
        //public RDRModule rdrModule { get; set; }
        //public RDRInformation rdrInfo { get; set; }
        public List<RDRModuleDetailsViewModel> rdrModuleList { get; set; }

        public RDRInformation rdrInfoDetail { get; set; }
    }

    
}