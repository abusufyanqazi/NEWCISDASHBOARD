using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using util;

namespace DashBoardAPI.Models
{
    public class AssmntBatchWiseObject
    {
        public string MONTH { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }

        public List<AssmntBatch> AssmntBatchWise = new List<AssmntBatch>();

        public AssmntBatchWiseObject(string pCode, string pName, string pMonth, DataTable dt)
        {
            this.MONTH = utility.GetFormatedDate(pMonth);
            this.CODE = pCode;
            this.NAME = pName;

            foreach (DataRow dr in dt.Select("SDIV_CODE = '" + pCode + "'"))
            {
                this.AssmntBatchWise.Add(new AssmntBatch(dr));
            }

            AddAssmentSummary();
        }

        public void AddAssmentSummary()
        {
            string Batch = "Total";
            Int64 BillsIssued = 0;
            Int64 OpeningBalance = 0;
            Int64 CurrentAssessment = 0;
            Int64 GovtAssessment = 0;
            Int64 NetAmount = 0;
            Int64 UnitBilled_KWH = 0;
            Int64 RuralBilled_KWH = 0;
            Int64 UrbanUnitBilled_KWH = 0;
            Int64 Adjustments = 0;
            Int64 UnitsAdjusted_KWH = 0;
            Int64 AmountAdjusted = 0;
            Int64 DetectionAdj = 0;
            Int64 DetectionAdjUnits_KWH = 0;
            Int64 DetectionAdjAmount_Rs = 0;
            Int64 ASSESS_DOM = 0;
            Int64 ASSESS_COM = 0;
            Int64 ASSESS_IND = 0;
            Int64 ASSESS_AGRI = 0;

            foreach (AssmntBatch ab in AssmntBatchWise)
            {
                BillsIssued += Int64.Parse(ab.BillsIssued);
                OpeningBalance += Int64.Parse(ab.OpeningBalance);
                CurrentAssessment += Int64.Parse(ab.CurrentAssessment);
                GovtAssessment += Int64.Parse(ab.GovtAssessment);
                NetAmount += Int64.Parse(ab.NetAmount);
                UnitBilled_KWH += Int64.Parse(ab.UnitBilled_KWH);
                RuralBilled_KWH += Int64.Parse(ab.RuralBilled_KWH);
                UrbanUnitBilled_KWH += Int64.Parse(ab.UrbanUnitBilled_KWH);
                Adjustments += Int64.Parse(ab.Adjustments);
                UnitsAdjusted_KWH += Int64.Parse(ab.UnitsAdjusted_KWH);
                AmountAdjusted += Int64.Parse(ab.AmountAdjusted);
                DetectionAdj += Int64.Parse(ab.DetectionAdj);
                DetectionAdjUnits_KWH += Int64.Parse(ab.DetectionAdjUnits_KWH);
                DetectionAdjAmount_Rs += Int64.Parse(ab.DetectionAdjAmount_Rs);
                ASSESS_DOM += Int64.Parse(ab.ASSESS_DOM);
                ASSESS_COM += Int64.Parse(ab.ASSESS_COM);
                ASSESS_IND += Int64.Parse(ab.ASSESS_IND);
                ASSESS_AGRI += Int64.Parse(ab.ASSESS_AGRI);

            }

            AssmntBatchWise.Add(new AssmntBatch(Batch, BillsIssued.ToString(), OpeningBalance.ToString(),
                CurrentAssessment.ToString(), GovtAssessment.ToString(),
                NetAmount.ToString(), UnitBilled_KWH.ToString(), RuralBilled_KWH.ToString(),
                UrbanUnitBilled_KWH.ToString(), Adjustments.ToString(),
                UnitsAdjusted_KWH.ToString(), AmountAdjusted.ToString(), DetectionAdj.ToString(),
                DetectionAdjUnits_KWH.ToString(), DetectionAdjAmount_Rs.ToString(),
                ASSESS_DOM.ToString(),
                ASSESS_COM.ToString(),
                ASSESS_IND.ToString(),
                ASSESS_AGRI.ToString()
            ));
        }
    }

    public class AssmntBatch
    {
        public string Batch { get; set; }
        public string BillsIssued { get; set; }
        public string OpeningBalance     { get; set; }
        public string CurrentAssessment { get; set; }
        public string GovtAssessment { get; set; }
        public string NetAmount { get; set; }
        public string UnitBilled_KWH { get; set; }
        public string RuralBilled_KWH { get; set; }
        public string UrbanUnitBilled_KWH { get; set; }
        public string Adjustments { get; set; }
        public string UnitsAdjusted_KWH { get; set; }
        public string AmountAdjusted { get; set; }
        public string DetectionAdj { get; set; }
        public string DetectionAdjUnits_KWH { get; set; }
        public string DetectionAdjAmount_Rs { get; set; }
        public string ASSESS_DOM{ get; set; }
        public string ASSESS_COM{ get; set; }
        public string ASSESS_IND{ get; set; }
        public string ASSESS_AGRI { get; set; }

        public AssmntBatch(DataRow dr)
        {
            this.Batch = utility.GetColumnValue(dr, "Batch");
            this.BillsIssued = utility.GetColumnValue(dr, "NOBILLSISSUED");
            this.OpeningBalance = utility.GetColumnValue(dr, "OPB");
            this.CurrentAssessment = utility.GetColumnValue(dr, "CURASSESS");
            this.GovtAssessment = utility.GetColumnValue(dr, "GOVTASSESS");
            this.NetAmount = utility.GetColumnValue(dr, "NET");
            this.UnitBilled_KWH = utility.GetColumnValue(dr, "UNITBILLED");
            this.RuralBilled_KWH = utility.GetColumnValue(dr, "RURALUNITBILLED");
            this.UrbanUnitBilled_KWH = utility.GetColumnValue(dr, "URBANUNITBILLED");
            this.Adjustments = utility.GetColumnValue(dr, "NOADJUSTM");
            this.UnitsAdjusted_KWH = utility.GetColumnValue(dr, "UNITADJ");
            this.AmountAdjusted = utility.GetColumnValue(dr, "AMTADJ");
            this.DetectionAdj = utility.GetColumnValue(dr, "NODETADJ");
            this.DetectionAdjUnits_KWH = utility.GetColumnValue(dr, "DETADJUNITS");
            this.DetectionAdjAmount_Rs = utility.GetColumnValue(dr, "DETADJAMT");
            this.ASSESS_DOM = utility.GetColumnValue(dr, "ASSESS_DOM");
            this.ASSESS_COM = utility.GetColumnValue(dr, "ASSESS_COM");
            this.ASSESS_IND = utility.GetColumnValue(dr, "ASSESS_IND");
            this.ASSESS_AGRI = utility.GetColumnValue(dr, "ASSESS_AGRI");
        }

        public AssmntBatch(string pBatch, string pBillsIssued, string pOpeningBalance, string pCurrentAssessment, string pGovtAssessment,
            string pNetAmount, string pUnitBilled_KWH, string pRuralBilled_KWH, string pUrbanUnitBilled_KWH, string pAdjustments,
            string pUnitsAdjusted_KWH, string pAmountAdjusted, string pDetectionAdj, string pDetectionAdjUnits_KWH, string pDetectionAdjAmount_Rs,
            string pASSESS_DOM, string pASSESS_COM, string pASSESS_IND, string pASSESS_AGRI)
        {
            this.Batch = pBatch;
            this.BillsIssued = pBillsIssued;
            this.OpeningBalance = pOpeningBalance;
            this.CurrentAssessment = pCurrentAssessment;
            this.GovtAssessment = pGovtAssessment;
            this.NetAmount = pNetAmount;
            this.UnitBilled_KWH = pUnitBilled_KWH;
            this.RuralBilled_KWH = pRuralBilled_KWH;
            this.UrbanUnitBilled_KWH = pUrbanUnitBilled_KWH;
            this.Adjustments = pAdjustments;
            this.UnitsAdjusted_KWH = pUnitsAdjusted_KWH;
            this.AmountAdjusted = pAmountAdjusted;
            this.DetectionAdj = pDetectionAdj;
            this.DetectionAdjUnits_KWH = pDetectionAdjUnits_KWH;
            this.DetectionAdjAmount_Rs = pDetectionAdjAmount_Rs;
            this.ASSESS_DOM = pASSESS_DOM;
            this.ASSESS_COM = pASSESS_COM;
            this.ASSESS_IND = pASSESS_IND;
            this.ASSESS_AGRI = pASSESS_AGRI;
        }
    }

   
}