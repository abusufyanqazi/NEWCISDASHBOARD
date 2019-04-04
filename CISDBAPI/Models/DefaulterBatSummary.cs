using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using util;

namespace DashBoardAPI.Models
{
    public class DefaulterBatchSummary
    {
        public string AsOn { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }

        public List<DefaulterBSummary> DfltrConsSmry = new List<DefaulterBSummary>();

        public DefaulterBatchSummary()
        {
            this.AsOn = string.Empty;
            this.CenterCode = string.Empty;
            this.CenterName = string.Empty;
        }

        public DefaulterBatchSummary(string pAsOn, string pCode, string pName, DataTable pDT)
        {
            this.AsOn = utility.GetFormatedDateYYYY(pAsOn);
            this.CenterCode = pCode;
            this.CenterName = pName;
            DataView dv = pDT.DefaultView;
            StringBuilder filterExp = new StringBuilder();
            filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length+1).ToString());
            dv.RowFilter = filterExp.ToString();
            foreach (DataRowView dr in dv)
            {
                DfltrConsSmry.Add(new DefaulterBSummary(dr));
            }
        }
    }

    public class DefaulterBSummary
    {
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string Numbers { get; set; }
        public string Amount { get; set; }

        public DefaulterBSummary(DataRowView dr)
        {
            this.CenterCode = utility.GetColumnValue(dr, "CODE");
            this.CenterName = utility.GetColumnValue(dr, "NAME");
            this.Numbers = utility.GetColumnValue(dr, "CONSUMERS");
            this.Amount = utility.GetColumnValue(dr, "AMOUNT");
        }
    }
}