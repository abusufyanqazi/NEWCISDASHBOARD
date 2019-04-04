using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using util;

namespace DashBoardAPI.Models
{
    public class DefaulterSummary
    {
        public string AsOn { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }

        public List<DefaulterConsSummary> DfltrConsSmry = new List<DefaulterConsSummary>();

        public DefaulterSummary()
        {
            this.AsOn = string.Empty;
            this.CenterCode = string.Empty;
            this.CenterName = string.Empty;
        }

        public DefaulterSummary(string pAsOn, string pCode, string pName, DataTable pDT)
        {
            this.AsOn = utility.GetFormatedDateYYYY(pAsOn);
            this.CenterCode = pCode;
            this.CenterName = pName;

            foreach (DataRow dr in pDT.Rows)
            {
                DfltrConsSmry.Add(new DefaulterConsSummary(dr));
            }

            Double totAmnt = 0;
            UInt64 totCons = 0;
            foreach(DefaulterConsSummary tot in this.DfltrConsSmry)
            {
                totAmnt += Double.Parse(tot.Amount);
                totCons += UInt64.Parse(tot.Numbers);
            }
            this.DfltrConsSmry.Add(new DefaulterConsSummary("Total", "Total", totCons.ToString(), totAmnt.ToString()));
        }
    }

    public class DefaulterConsSummary
    {
        public string SrNo { get; set; }
        public string Slabs { get; set; }
        public string Numbers { get; set; }
        public string Amount { get; set; }

        public DefaulterConsSummary(string srNo, string slab, string numbers, string amnt)
        {

            this.SrNo = srNo;
            this.Slabs = slab;
            this.Numbers = numbers;
            this.Amount = amnt;
        }
        public DefaulterConsSummary(DataRow dr)
        {
            this.SrNo = utility.GetColumnValue(dr, "SrNO");
            this.Slabs = utility.GetColumnValue(dr, "SLAB_NAME");
            this.Numbers = utility.GetColumnValue(dr, "CONSUMERS");
            this.Amount = utility.GetColumnValue(dr, "AMOUNT");
        }
    }
}