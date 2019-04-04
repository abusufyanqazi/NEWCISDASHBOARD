using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using util;

namespace DashBoardAPI.Models
{
    public class ExtraHeaveyBillRegion
    {
        public String BillingMonth { get; set; }
        public List<RegWise> regWise = new List<RegWise>();

        public ExtraHeaveyBillRegion()
        {
            this.BillingMonth = "";

        }

        public ExtraHeaveyBillRegion(string bMonth, DataTable dt)
        {
            this.BillingMonth = utility.GetFormatedDateYYYY(bMonth);

            foreach (DataRow dr in dt.Rows)
            {
                regWise.Add(new RegWise(dr));
            }

        }

        private void AddExtraHeaveySummary()
        {
            UInt64 UnitsConsumed = 0;
            Double BillAmount = 0;
            foreach (RegWise rw in regWise)
            {
                UnitsConsumed += rw.UnitsConsumed;
                BillAmount += rw.BillAmount;
            }
            regWise.Add(new RegWise("Total", "Total", UnitsConsumed, BillAmount));
        }
    }

    

    public class RegWise
    {

        public String   RegCode { get; set; }
        public String RegName { get; set; }
        public UInt64 UnitsConsumed { get; set; }
        public Double BillAmount { get; set; }

        public RegWise(string rCode, string rName, UInt64 uConsumed, Double bAmnt)
        {
            this.RegCode = rCode;
            this.RegName = rName;
            this.UnitsConsumed = uConsumed;
            this.BillAmount = bAmnt;
        }
        public RegWise(DataRow dr)
        {
            this.RegCode = utility.GetColumnValue(dr, "CODE");
            this.RegName = utility.GetColumnValue(dr, "NAME");
            this.UnitsConsumed = UInt64.Parse(utility.GetColumnValue(dr, "UNITS"));
            this.BillAmount = Double.Parse(utility.GetColumnValue(dr, "AMOUNT"));
        }
    }

    public class ExtraHeaveyBill
    {
        public String BillingMonth { get; set; }
        public String CenterCode { get; set; }
        public String CenterName { get; set; }

        public List<RefWise> refWise= new List<RefWise>();

        public ExtraHeaveyBill(string cd, string name, string bMonth, DataTable dt)
        {
            this.BillingMonth = utility.GetFormatedDateYYYY(bMonth);
            this.CenterCode = cd;
            this.CenterName = name;
            foreach (DataRow dr in dt.Rows)
            {
                refWise.Add(new RefWise(dr));
            }
        }
        public ExtraHeaveyBill()
        {
            this.BillingMonth = string.Empty;
            this.CenterCode = string.Empty;
            this.CenterName = string.Empty;
        }
    }

    public class RefWise
    {
        public String SrNo { get; set; }
        public String ReferenceNo { get; set; }
        public String Tariff { get; set; }
        public Decimal SanctionLoad { get; set; }
        public UInt64 UnitConsumed { get; set; }
        public Double BillAmount { get; set; }
        public String BilledMonth { get; set; }

        public RefWise(DataRow dr)
        {
            this.SrNo = utility.GetColumnValue(dr, "SrNo");
            this.ReferenceNo = utility.GetColumnValue(dr, "REFNO");  
            this.Tariff = utility.GetColumnValue(dr, "TRF"); 
            this.SanctionLoad = decimal.Parse(utility.GetColumnValue(dr, "SLOAD"));
            this.UnitConsumed = UInt64.Parse(utility.GetColumnValue(dr, "UNITS")); 
            this.BillAmount =Double.Parse(utility.GetColumnValue(dr, "AMOUNT"));
            this.BilledMonth = utility.GetColumnValue(dr, "BMONTH"); 

        }
        public RefWise(string srN, string refNo, string trf, Decimal sanLoad, UInt64 uConsumed, Double bAmnt,
            string billedMon)
        {
            this.SrNo = srN;
            this.ReferenceNo = refNo;
            this.Tariff = trf;
            this.SanctionLoad = sanLoad;
            this.UnitConsumed = uConsumed;
            this.BillAmount = bAmnt;
            this.BilledMonth = billedMon;

        }

        
    }
}