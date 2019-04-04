using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using util;


namespace DashBoardAPI.Models
{
    public class DefaultListRefWise
    {
        public string AsOn { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string Slab { get; set; }

        public List<DefaultConsumerRef> DfltrConsSmry = new List<DefaultConsumerRef>();

        public DefaultListRefWise()
        {
            this.AsOn = string.Empty;
            this.CenterCode = string.Empty;
            this.CenterName = string.Empty;
            this.Slab = string.Empty;
        }
        public DefaultListRefWise(string pAsOn, string pCode, string pName, string slab,DataTable pDT)
        {
            this.AsOn = utility.GetFormatedDateYYYY(pAsOn);
            this.CenterCode = pCode;
            this.CenterName = pName;
            this.Slab = slab;

            foreach (DataRow dr in pDT.Rows)
            {
                DfltrConsSmry.Add(new DefaultConsumerRef(dr));
            }
        }
    }

    public class DefaultConsumerRef
    {

        public string SrNo { get; set; }
        public string RefNo { get; set; }
        public string Tariff { get; set; }
        public string NameAddress { get; set; }
        public string NoticeOrderNo { get; set; }
        public string NoticeOrderDate { get; set; }
        public string MeterNo { get; set; }
        public string Age { get; set; }
        public string Amount { get; set; }

        public DefaultConsumerRef(DataRow dr)
        {
            this.SrNo = utility.GetColumnValue(dr, "SrNO");
            this.RefNo = utility.GetColumnValue(dr, "REFNO");
            this.Tariff = utility.GetColumnValue(dr, "TARRIFCODE");
            this.NameAddress = utility.GetColumnValue(dr, "NAMEADD");
            this.NoticeOrderNo = utility.GetColumnValue(dr, "DCN_NO");
            this.NoticeOrderDate = utility.GetColumnValue(dr, "DCN_DATE");
            this.MeterNo = utility.GetColumnValue(dr, "MTR_NO");
            this.Age = utility.GetColumnValue(dr, "AGE");
            this.Amount = utility.GetColumnValue(dr, "AMOUNT");
        }      
    }

}