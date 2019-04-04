using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using util;

namespace DashBoardAPI.Models
{
    public class DefaultSumFdrWise
    {
        public string BillingMonth { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }

        public List<DefaultSumFdr> FdrDfltrSmry_FdrWise = new List<DefaultSumFdr>();

        public DefaultSumFdrWise()
        {
            this.BillingMonth = string.Empty;
            this.CenterCode = string.Empty;
            this.CenterName = string.Empty;
        }
        public DefaultSumFdrWise(string pBillingMonth, string pCenterCode, string pCenterName, DataTable pDT)
        {
            this.BillingMonth = utility.GetFormatedDateYYYY(pBillingMonth);
            this.CenterCode = pCenterCode;
            this.CenterName = pCenterName;

            foreach (DataRow dr in pDT.Rows)
            {
                FdrDfltrSmry_FdrWise.Add(new DefaultSumFdr(dr));
            }
        }
    }

    public class DefaultSumFdr
    {

        public string FeederCode { get; set; }
        public string FeederName { get; set; }
        public string SubDiv { get; set; }
        public string Consumers { get; set; }
        public string DefaultAmount { get; set; }

        public DefaultSumFdr(DataRow dr)
        {
            this.FeederCode = utility.GetColumnValue(dr, "FDRCODE");
            this.FeederName = utility.GetColumnValue(dr, "FDRNAME");
            this.SubDiv = utility.GetColumnValue(dr, "CODE");
            this.Consumers = utility.GetColumnValue(dr, "TOT_CONS");
            this.DefaultAmount = utility.GetColumnValue(dr, "AMOUNT");
        }      
    }

}