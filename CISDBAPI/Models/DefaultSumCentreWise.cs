using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using util;

namespace DashBoardAPI.Models
{

    public class DefaultSumCentreWise
    {
        public string BillingMonth { get; set; }

        public List<DefaultSumCentre> DfltrConsSmry = new List<DefaultSumCentre>();

        public DefaultSumCentreWise()
        {
            this.BillingMonth = string.Empty;
        }

        public DefaultSumCentreWise(string pAsOn, string pCode, string pName, DataTable pDT)
        {
            this.BillingMonth = utility.GetFormatedDateYYYY(pAsOn);
            DataView dv = pDT.DefaultView;
            StringBuilder filterExp = new StringBuilder();
            filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length+1).ToString());
            dv.RowFilter = filterExp.ToString();
            foreach (DataRowView dr in dv)
            {
                DfltrConsSmry.Add(new DefaultSumCentre(dr));
            }
        }
    }

    public class DefaultSumCentre
    {
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string TotConsumer { get; set; }
        public string DefaultAmount { get; set; }

        public DefaultSumCentre(DataRowView dr)
        {
            this.CenterCode = utility.GetColumnValue(dr, "CODE");
            this.CenterName = utility.GetColumnValue(dr, "NAME");
            this.TotConsumer = utility.GetColumnValue(dr, "CONSUMERS");
            this.DefaultAmount = utility.GetColumnValue(dr, "AMOUNT");
        }
    }
}