using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using util;

namespace DashBoardAPI.Models
{
    public class DefectMeterSumTrfWise
    {
        public string BillingMonth { get; set; }
        public string CenterCode { get; set; }
        
        //public string CenterName { get; set; }

        public List<DefectTrfWise> defectTrfWise = new List<DefectTrfWise>();

        public DefectMeterSumTrfWise()
        {
            this.BillingMonth = string.Empty;
            this.CenterCode = string.Empty;
        }

        public DefectMeterSumTrfWise(string pBillMon, string pCode, DataTable pDT)
        {
            this.BillingMonth = utility.GetBillMonth(pBillMon);
            this.CenterCode = pCode;

            DataView dv = pDT.DefaultView;
            StringBuilder filterExp = new StringBuilder();
            filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length + 1).ToString());
            dv.RowFilter = filterExp.ToString();

            foreach (DataRowView dr in dv)
            {
                defectTrfWise.Add(new DefectTrfWise(dr));
            }
            filterExp = new StringBuilder();
            filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length).ToString());
            dv.RowFilter = filterExp.ToString();

            if (dv.ToTable().Rows.Count>0)
                defectTrfWise.Add(new DefectTrfWise("Total", dv[0]["CODE"].ToString(), dv[0]["NAME"].ToString(), dv[0]["DOMESTIC"].ToString(), dv[0]["COMMERCIAL"].ToString(),
                    dv[0]["INDUSTRIAL"].ToString(), dv[0]["AGRICULTURE"].ToString(), dv[0]["OTHERS"].ToString(), dv[0]["TOTAL"].ToString()));
        }
    }

    public class DefectTrfWise
    {
        public string SrNo { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string Domestic { get; set; }
        public string Commercial { get; set; }
        public string Industrial { get; set; }
        public string Agricultural { get; set; }
        public string Others { get; set; }
        public string Total { get; set; }

        public DefectTrfWise(DataRowView dr)
        {
            this.SrNo = utility.GetColumnValue(dr, "SRNO");
            this.CenterCode = utility.GetColumnValue(dr, "CODE");
            this.CenterName = utility.GetColumnValue(dr, "NAME");
            this.Domestic = utility.GetColumnValue(dr, "DOMESTIC");
            this.Commercial = utility.GetColumnValue(dr, "COMMERCIAL");
            this.Industrial = utility.GetColumnValue(dr, "INDUSTRIAL");
            this.Agricultural = utility.GetColumnValue(dr, "AGRICULTURE");
            this.Others = utility.GetColumnValue(dr, "OTHERS");
            this.Total = utility.GetColumnValue(dr, "TOTAL");
        }

        public DefectTrfWise(string pSrNo, string  pCode, string pName, string pDomestic, string pCommercial, string pInd, string pAgri, string pOther, string pTotal)
        {
            this.SrNo = pSrNo;
            this.CenterCode = pCode;
            this.CenterName = pName;
            this.Domestic = pDomestic;
            this.Commercial = pCommercial;
            this.Industrial = pInd;
            this.Agricultural = pAgri;
            this.Others = pOther;
            this.Total = pTotal;
        }
    }
}