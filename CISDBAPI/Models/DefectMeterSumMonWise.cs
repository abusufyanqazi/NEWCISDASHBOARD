using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using util;

namespace DashBoardAPI.Models
{
    public class DefectMeterSumMonWise
    {
        public string BillingMonth { get; set; }
        public string CenterCode { get; set; }
        
        public string CenterName { get; set; }

        public List<DefectMonWise> defectMtrSumMonWise = new List<DefectMonWise>();

        public DefectMeterSumMonWise()
        {
            this.BillingMonth = string.Empty;
            this.CenterCode = string.Empty;
        }

        public DefectMeterSumMonWise(string pBillMon, string pCode, string pName,DataTable pDT)
        {
            this.BillingMonth = utility.GetBillMonth(pBillMon);
            this.CenterCode = pCode;
            this.CenterName = pName;
           
            DataView dv = pDT.DefaultView;
            StringBuilder filterExp = new StringBuilder();
            filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length + 1).ToString());
            dv.RowFilter = filterExp.ToString();
            
            foreach (DataRowView dr in dv)
            {
                defectMtrSumMonWise.Add(new DefectMonWise(dr));
            }

            UInt64 one = 0;
            UInt64 two = 0;
            UInt64 four = 0;
            UInt64 morethan6 = 0;

            foreach(DefectMonWise o in this.defectMtrSumMonWise)
            {
                one +=UInt64.Parse( o.OneMonth);
                two += UInt64.Parse(o.Two_3_Months);
                four += UInt64.Parse(o.Four_6_Months);
                morethan6 += UInt64.Parse(o.MoreThan6Months);
            }
            this.defectMtrSumMonWise.Add(new DefectMonWise("Total", "Total", "Total", one, two, four, morethan6));
        }
    }

    public class DefectMonWise
    {
        public string SrNo { get; set; }
        public string OfficeName { get; set; }
        public string CenterCode { get; set; }
        public string OneMonth { get; set; }
        public string Two_3_Months { get; set; }
        public string Four_6_Months { get; set; }
        public string MoreThan6Months { get; set; }

        public DefectMonWise(DataRowView dr)
        {
            this.SrNo = utility.GetColumnValue(dr, "SRNO");
            this.OfficeName = utility.GetColumnValue(dr, "NAME");
            this.CenterCode = utility.GetColumnValue(dr, "CODE");
            this.OneMonth = utility.GetColumnValue(dr, "ONE_MONTH");
            this.Two_3_Months = utility.GetColumnValue(dr, "TWO_TO_3_MONTH");
            this.Four_6_Months = utility.GetColumnValue(dr, "FOUR_TO_6_MONTH");
            this.MoreThan6Months = utility.GetColumnValue(dr, "MORE_THEN_6_MONTH");
        }

        public DefectMonWise(string srNo, string officeName, string centerCode, UInt64 one, UInt64 two, UInt64 four, UInt64 morethan6)
        {
            this.SrNo = srNo;
            this.OfficeName = officeName;
            this.CenterCode = centerCode;
            this.OneMonth = one.ToString();
            this.Two_3_Months = two.ToString();
            this.Four_6_Months = four.ToString();
            this.MoreThan6Months = morethan6.ToString();
        }
    }
}