using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

    public class CreditAdjustmentsCentreWise
    {
        public string BillingMonth { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }

        public List<CreditAdjCentreWise> CreditAdj = new List<CreditAdjCentreWise>();

        public CreditAdjustmentsCentreWise()
        {
            this.BillingMonth = string.Empty;
            this.CenterCode = string.Empty;
            this.CenterName = string.Empty;
        }

        public CreditAdjustmentsCentreWise(string pCode, DataTable pDT)
        {
           
            this.CenterCode = pCode;
            DataView dv = pDT.DefaultView;
            StringBuilder filterExp = new StringBuilder();
            filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length + 1).ToString());
            dv.RowFilter = filterExp.ToString();
            
            foreach (DataRowView dr in dv)
            {
                CreditAdj.Add(new CreditAdjCentreWise(dr));
            }

            filterExp = new StringBuilder();
            filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length).ToString());
            dv.RowFilter = filterExp.ToString();



            if (dv.ToTable().Rows.Count > 0)
            {
                this.BillingMonth = utility.GetBillMonth(dv[0]["BILLMONTH"].ToString());
                this.CenterName = dv[0]["NAME"].ToString();

                CreditAdj.Add(new CreditAdjCentreWise("Total", "Total",
              dv[0]["TOTREFFNOS"].ToString(), dv[0]["UNITS"].ToString(), dv[0]["AMOUNT"].ToString()));

                //CreditAdj.Add(new CreditAdjCentreWise(dv[0]["CODE"].ToString(),dv[0]["NAME"].ToString(),
                //    dv[0]["TOTREFFNOS"].ToString(),dv[0]["UNITS"].ToString(),dv[0]["AMOUNT"].ToString()));
            }
        }
    }

    public class CreditAdjCentreWise
    {
        public string  CenterCode{ get; set; }
        public string CenterName { get; set; }
        public string TotRefNos { get; set; }
        public string Units { get; set; }
        public string Amount { get; set; }

        public CreditAdjCentreWise(string pName, string pCode, string pTotRefNo,string pUnits, string pAmount)
        {
            this.CenterCode = pName;
            this.CenterName = pCode;
            this.TotRefNos = pTotRefNo;
            this.Units = pUnits;
            this.Amount = pAmount;
        }

        public CreditAdjCentreWise(DataRowView dr)
        {
            this.CenterCode = utility.GetColumnValue(dr, "CODE");
            this.CenterName = utility.GetColumnValue(dr, "NAME");
            this.TotRefNos = utility.GetColumnValue(dr, "TOTREFFNOS");
            this.Units = utility.GetColumnValue(dr, "Units");
            this.Amount = utility.GetColumnValue(dr, "Amount");
        }
    }
}