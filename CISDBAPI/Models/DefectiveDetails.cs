using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using util;

namespace DashBoardAPI.Models
{
    public class DefectiveDetails
    {
        public string BillingMonth { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }

        public List<DefectiveRefWise> RefWise = new List<DefectiveRefWise>();

        public DefectiveDetails()
        {
            this.BillingMonth = string.Empty;
            this.CenterCode = string.Empty;
            this.CenterName = string.Empty;
        }

        public DefectiveDetails(string pBillMon, string pCode, string pName, DataTable pDT)
        {
            this.BillingMonth = utility.GetBillMonth(pBillMon);
            this.CenterCode = pCode;
            this.CenterName = pName;

            foreach (DataRow dr in pDT.Rows)
            {
                RefWise.Add(new DefectiveRefWise(dr));
            }
        }
    }

    public class DefectiveRefWise
    {
        public string SrNo { get; set; }
        public string RefNo { get; set; }
        public string NameAddress { get; set; }
        public string Tariff { get; set; }
        public string Load { get; set; }
        public string Category { get; set; }
        public string Age { get; set; }
        public string Phase { get; set; }
        public string Status { get; set; }
        public DefectiveRefWise(DataRow dr)
        {
            this.SrNo = utility.GetColumnValue(dr, "SRNO");
            this.RefNo = utility.GetColumnValue(dr, "REFNO");
            this.NameAddress = utility.GetColumnValue(dr, "NAME_ADDRESS");
            this.Tariff = utility.GetColumnValue(dr, "TRF_CD");
            this.Load = utility.GetColumnValue(dr, "SAN_LOAD");
            this.Category = utility.GetColumnValue(dr, "CAT");
            this.Age = utility.GetColumnValue(dr, "DEFECT_AGE");
            this.Phase = utility.GetColumnValue(dr, "PHASE");
            this.Status = utility.GetColumnValue(dr, "STATUS");
        }
    }

    public class DefectiveDetailsR
    {
        public string BillingMonth { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }

        public List<DefectiveRegionWise> RegionWise = new List<DefectiveRegionWise>();

        public DefectiveDetailsR()
        {
            this.BillingMonth = string.Empty;
            this.CenterCode = string.Empty;
            this.CenterName = string.Empty;
        }

        public DefectiveDetailsR(string pBillMon, string pCode, string pName, DataTable pDT)
        {
            this.BillingMonth = utility.GetBillMonth(pBillMon);
            this.CenterCode = pCode;
            this.CenterName = pName;

            foreach (DataRow  dr in pDT.Rows)
            {
                RegionWise.Add(new DefectiveRegionWise(dr));
            }
        }
    }

    public class DefectiveRegionWise
    {
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string TotSinglePhase { get; set; }
        public string TotThreePhase { get; set; }
        public string TotDefCons { get; set; }

        public DefectiveRegionWise(DataRow dr)
        {
            this.CenterCode = utility.GetColumnValue(dr, "Code");
            this.CenterName = utility.GetColumnValue(dr, "Name");
            this.TotSinglePhase = utility.GetColumnValue(dr, "TotSinglePhase");
            this.TotThreePhase = utility.GetColumnValue(dr, "TotThreePhase");
            this.TotDefCons = utility.GetColumnValue(dr, "TotDefCons");
        }
    }

   
}