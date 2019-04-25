using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using util;

namespace DashBoardAPI.Models
{
    public class DefConsListFdrWise
    {
        public string FdrCode { get; set; }
        public string FeederName { get; set; }

        public List<DefConsList> FdrDfltrSmry_RefWise=new List<DefConsList>();
        public DefConsListFdrWise()
        {
            this.FdrCode = string.Empty;
            this.FeederName = string.Empty;
        }
        public DefConsListFdrWise(string pFdrCode,string pFeederName, DataTable pDT)
        {
            this.FdrCode = pFdrCode;
            this.FeederName = pFeederName;

            foreach (DataRow dr in pDT.Rows)
            {
                FdrDfltrSmry_RefWise.Add(new DefConsList(dr));
            }
        }
    }

    public class DefConsList
    {
        public string SrNo { get; set; }
        public string RefNo { get; set; }
        public string Tariff { get; set; }
        public string NameAddress { get; set; }
        public string Status { get; set; }
        public string Age { get; set; }
        public string DcnNo { get; set; }
        public string DcnDate { get; set; }
        public string MeterNo { get; set; }
        public string Amount { get; set; }

        public DefConsList()
        {
            this.SrNo = string.Empty;
            this.RefNo = string.Empty;
            this.Tariff = string.Empty;
            this.NameAddress = string.Empty;
            this.Status = string.Empty;
            this.Age = string.Empty;
            this.DcnNo = string.Empty;
            this.DcnDate = string.Empty;
            this.MeterNo = string.Empty;
            this.Amount = string.Empty;
        }

        public DefConsList(DataRow dr)
        {
            this.SrNo = utility.GetColumnValue(dr, "SrNO");
            this.RefNo = utility.GetColumnValue(dr, "REFNO");
            this.Tariff = utility.GetColumnValue(dr, "TARRIFCODE");
            this.NameAddress = utility.GetColumnValue(dr, "NAMEADD");
            this.Status = utility.GetColumnValue(dr, "STATUS");
            this.Age = utility.GetColumnValue(dr, "AGE");
            this.DcnNo = utility.GetColumnValue(dr, "DCN_NO");
            this.DcnDate = utility.GetColumnValue(dr, "DCN_DATE");
            this.MeterNo = utility.GetColumnValue(dr, "MTR_NO");
            this.Amount = utility.GetColumnValue(dr, "Amount");
        }
    }
}