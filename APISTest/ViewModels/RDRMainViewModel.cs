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

    public class RDRMainIndexViewModel
    {
        public RDRMain rdrMain { get; set; }
        public string CarMaker { get; set; }


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
}