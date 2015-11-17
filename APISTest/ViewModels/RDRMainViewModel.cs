using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using APISTest.Models;

namespace APISTest.ViewModels
{
    public class RDRMainViewModel
    {
        public RDRMain rdrMain {get;set;}
        public string CustomerTeamCode { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RFQDate
        {
            get { return rdrMain.RFQDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AcquisitionDate
        {
            get { return rdrMain.AcquisitionDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        public DateTime SOPDate
        {
            get { return rdrMain.SOPDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        public DateTime EOLDate
        {
            get { return rdrMain.EOLDate; }
        }
    }

    //RDRMain 列表資料模型
    public class RDRMainIndexViewModel
    {
        public RDRMain rdrMain { get; set; }
        public string CarMaker { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RFQDate
        {
            get { return rdrMain.RFQDate; }
            set { }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AcquisitionDate
        {
            get { return rdrMain.AcquisitionDate; }
            set { }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        public DateTime SOPDate
        {
            get { return rdrMain.SOPDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}", ApplyFormatInEditMode = true)]
        public DateTime EOLDate
        {
            get { return rdrMain.EOLDate; }
        }

    
    }

    public class RDRMainDetailsViewModel
    {
        public RDRMain rdrMain { get; set; }
        public string CarMaker { get; set; }
        

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RFQDate
        {
            get { return rdrMain.RFQDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AcquisitionDate
        {
            get { return rdrMain.AcquisitionDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}")]
        public DateTime SOPDate
        {
            get { return rdrMain.SOPDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}")]
        public DateTime EOLDate
        {
            get { return rdrMain.EOLDate; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreateDate
        {
            get { return rdrMain.CreateTime; }
        }

        public string EstimateRevenueText
        { get
          {
                return APISTest.Tools.EnumMapTool.GetDescription((EnumEstimateRevenue)rdrMain.EstimateRevenue);
          }
        }
    }

    public class RDRMainReportViewModel
    {
        public RDRMain rdrMain { get; set; }
        public string CarMakerName { get; set; }

        public string ID
        {
            get { return rdrMain.ID.ToString(); }
        }
        public string RDRNumber
        {
            get { return rdrMain.RDRNumber; }
        }
        public string CustomerTeamCode
        {
            get { return rdrMain.CustomerTeamCode; }
        }
        public string LOB
        {
            get { return rdrMain.LOB; }
        }
        public string Site
        {
            get { return rdrMain.Site; }
        }
        public string CarModel
        {
            get { return rdrMain.CarModel; }
        }
        public string EstimateSales
        {
            get { return rdrMain.EstimateSales.ToString(); }
        }
        public string RFQDate
        {
            get { return string.Format("{0:yyyy-MM-dd}", rdrMain.RFQDate); }
        }
        public string AcquisitionDate
        {
            get { return string.Format("{0:yyyy-MM-dd}", rdrMain.AcquisitionDate); }
        }
        public string SOPDate
        {
            get { return string.Format("{0:yyyy-MM}", rdrMain.SOPDate); }
        }
        public string EOLDate
        {
            get { return string.Format("{0:yyyy-MM}", rdrMain.EOLDate); }
        }
        public string Certainty
        {
            get { return rdrMain.Certainty; }
        }
        public string RequestClass
        {
            get { return rdrMain.RequestClass; }
        }
        public string RequestType
        {
            get { return rdrMain.RequestType; }
        }
        public string Currency
        {
            get { return rdrMain.Currency; }
        }
        public string ProjectName
        {
            get { return rdrMain.ProjectName; }
        }
        public string EstimateRevenueText
        {
            get
            {
                return APISTest.Tools.EnumMapTool.GetDescription((EnumEstimateRevenue)rdrMain.EstimateRevenue);
            }
        }
        public string CreateDate
        {
            get { return string.Format("{0:yyyy-MM-dd}", rdrMain.CreateTime); }
        }
    }
}