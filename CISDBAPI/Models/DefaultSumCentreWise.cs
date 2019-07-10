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
        public string AsOn { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }

        public List<DefaultSumCentre> DfltrConsSmry = new List<DefaultSumCentre>();

        public DefaultSumCentreWise()
        {
            this.AsOn = string.Empty;
            this.CenterCode = string.Empty;
            this.CenterName = string.Empty;
        }

        public DefaultSumCentreWise(string pAsOn, string pCode, string pName, DataTable pDT)
        {
            this.AsOn= utility.GetFormatedDateYYYY(pAsOn);
            this.CenterCode = pCode;
            this.CenterName = pName;

            foreach (DataRow dr in pDT.Rows)
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

        public DefaultSumCentre(DataRow dr)
        {
            this.CenterCode = utility.GetColumnValue(dr, "CODE");
            this.CenterName = utility.GetColumnValue(dr, "NAME");
            this.TotConsumer = utility.GetColumnValue(dr, "CONSUMERS");
            this.DefaultAmount = utility.GetColumnValue(dr, "AMOUNT");
        }
    }
}