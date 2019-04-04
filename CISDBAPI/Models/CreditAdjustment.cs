using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using util;

namespace DashBoardAPI.Models
{
    public class CreditAdjustments
    {
        public string BillingMonth { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }

        public List<CreditAdjustment> CreditAdj= new List<CreditAdjustment>();

        public CreditAdjustments()
        {
            this.BillingMonth = string.Empty;
            this.CenterCode = string.Empty;
            this.CenterName = string.Empty;
        }

        public CreditAdjustments(string pBillMon, string pCode, string pName, DataTable pDT)
        {
            this.BillingMonth = utility.GetBillMonth(pBillMon);
            this.CenterCode = pCode;
            this.CenterName = pName;

            foreach (DataRow dr in pDT.Rows)
            {
                CreditAdj.Add(new CreditAdjustment(dr));
            }
        }
    }

    public class CreditAdjustment
    {
        public string SrNo { get; set; }
        public string RefNo { get; set; }
        public string Name { get; set; }
        public string Adjustment { get; set; }
        public string ROAdjDate { get; set; }
        public string MainDate { get; set; }
        public string PostingDate { get; set; }
        public string BillingPeriod { get; set; }
        public string Units { get; set; }
        public string Amount { get; set; }
        public CreditAdjustment(DataRow dr)
        {
            this.SrNo = utility.GetColumnValue(dr, "SrNO");
            this.RefNo = utility.GetColumnValue(dr, "REF_NO");
            this.Name = utility.GetColumnValue(dr, "CONS_NAME");
            this.Adjustment = utility.GetColumnValue(dr, "ADJ_NO");
            this.ROAdjDate = utility.GetColumnValue(dr, "RO_ADJ_DATE");
            this.MainDate = utility.GetColumnValue(dr, "MAIN_DATE");
            this.PostingDate = utility.GetColumnValue(dr, "POSTING_DATE");
            this.BillingPeriod = utility.GetColumnValue(dr, "BILLMONTH");
            this.Units = utility.GetColumnValue(dr, "UNITS_ADJ");
            this.Amount = utility.GetColumnValue(dr, "AMOUNT_ADJ");
        }
    }
}