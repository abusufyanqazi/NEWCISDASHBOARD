using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Web;
using DashBoardAPI.Models;
using  util;
using DAL;

namespace DashBoardAPI.Models
{
    public class DBoardBridge
    {
        static string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CONSTR"].ToString();

        //
        public DefaultSumFdrWise GetDefConsFdrCdWise(string code)
        {
            DefaultSumFdrWise _DefaultSumFdrWise = new DefaultSumFdrWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefConsFdrCdWise(code, DateTime.Now.AddMonths(-1));
            if (dt != null && dt.Rows.Count > 0)
            {
                string billMonth = utility.GetFormatedDateYYYY(utility.GetColumnValue(dt.Rows[0], "BILLMONTH"));
                string cd = utility.GetColumnValue(dt.Rows[0], "CODE");
                string name = utility.GetColumnValue(dt.Rows[0], "NAME");
                _DefaultSumFdrWise = new DefaultSumFdrWise(billMonth, cd,name, dt);

            }

            return _DefaultSumFdrWise;
        }
        public DefaultListRefWise GetDefListRefWise(string code, string type, string status, string tariff, string pSlab)
        {
            DefaultListRefWise _DefaulterSummary = new DefaultListRefWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefListRefWise(code, DateTime.Now.AddMonths(-1), type, status, tariff, pSlab);
            if (dt != null && dt.Rows.Count > 0)
            {
                    string billMonth = utility.GetFormatedDateYYYY(utility.GetColumnValue(dt.Rows[0], "BILLMONTH"));
                    string cd = utility.GetColumnValue(dt.Rows[0], "CODE");
                    string name = utility.GetColumnValue(dt.Rows[0], "NAME");
                    string slab = utility.GetColumnValue(dt.Rows[0], "SLAB");
                    _DefaulterSummary = new DefaultListRefWise(billMonth, cd, name, slab, dt);

            }

            return _DefaulterSummary;
        }
        public DefaulterBatchSummary GetDefaulterSummaryCntrWise(string pCode, string pAge, string pPvtGvt, string pRundisc, string pTrf)
        {
            DefaulterBatchSummary _DefaulterSummary = new DefaulterBatchSummary();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefConsSumBatch(pCode, DateTime.Now.AddMonths(-1),pAge,pPvtGvt,pRundisc,pTrf);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dvHead = dt.DefaultView;
                filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length).ToString());
                dvHead.RowFilter = filterExp.ToString();
                if (dvHead != null && dvHead.ToTable().Rows.Count > 0)
                {
                    string billMonth = utility.GetFormatedDateYYYY(utility.GetColumnValue(dvHead.ToTable().Rows[0], "BILLMONTH"));
                    string cd = utility.GetColumnValue(dvHead.ToTable().Rows[0], "CODE");
                    string name = utility.GetColumnValue(dvHead.ToTable().Rows[0], "NAME");
                    _DefaulterSummary = new DefaulterBatchSummary(billMonth, cd, name, dt);
                }

            }

            return _DefaulterSummary;
        }

        /// <summary>
        /// IfCompany code is given then return its circle data
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public DefectMeterSumMonWise GetDefectMeterSumMonWise(string code)
        {
            DefectMeterSumMonWise _DefectMeterSumMonWise = new DefectMeterSumMonWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.GetDefectMeterSumMonWise(code, DateTime.Now.AddMonths(-1));
            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                StringBuilder filterExp = new StringBuilder();
                    filterExp.AppendFormat("LEN(CODE) = {0}", (code.Length).ToString());
                    dv.RowFilter = filterExp.ToString();
                dv.Sort = "CODE ASC";
                DataTable dt1 = dv.ToTable();

                if (dt1 != null)
                {
                    string billMonth = utility.GetColumnValue(dt1.Rows[0], "BILLMONTH");
                    string cd = utility.GetColumnValue(dt1.Rows[0], "CODE");
                    string name = utility.GetColumnValue(dt1.Rows[0], "NAME");

                    _DefectMeterSumMonWise = new DefectMeterSumMonWise(billMonth, cd, name,dt);
                }
            }

            return _DefectMeterSumMonWise;
        }

        /// <summary>
        /// If Company Code is given then return its cricle data
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        public DefaulterBatchSummary GetDefaulterSummaryBatch(string pCode)
        {
            DefaulterBatchSummary _DefaulterSummary = new DefaulterBatchSummary();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefConsSumBatch(pCode, DateTime.Now.AddMonths(-1));
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dvHead = dt.DefaultView;
                filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length).ToString());
                dvHead.RowFilter = filterExp.ToString();
                if (dvHead != null && dvHead.ToTable().Rows.Count > 0)
                {
                    string billMonth = utility.GetFormatedDateYYYY(utility.GetColumnValue(dvHead.ToTable().Rows[0], "BILLMONTH"));
                    string cd = utility.GetColumnValue(dvHead.ToTable().Rows[0], "CODE");
                    string name = utility.GetColumnValue(dvHead.ToTable().Rows[0], "NAME");
                    _DefaulterSummary = new DefaulterBatchSummary(billMonth, cd, name, dt);
                }

            }

            return _DefaulterSummary;
        }
        public DefaulterBatchSummary GetDefaulterSummaryBatch(string pCode, string pPvtGvt, string pRundisc, string pTrf)
        {
            DefaulterBatchSummary _DefaulterSummary = new DefaulterBatchSummary();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefConsSumBatch(pCode,DateTime.Now.AddMonths(-1), pPvtGvt, pRundisc,pTrf);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dvHead = dt.DefaultView;
                filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length).ToString());
                dvHead.RowFilter = filterExp.ToString();
                if(dvHead!=null && dvHead.ToTable().Rows.Count>0)
                {
                    string billMonth = utility.GetFormatedDateYYYY(utility.GetColumnValue(dvHead.ToTable().Rows[0], "BILLMONTH"));
                    string cd = utility.GetColumnValue(dvHead.ToTable().Rows[0], "CODE");
                    string name = utility.GetColumnValue(dvHead.ToTable().Rows[0], "NAME");
                    _DefaulterSummary = new DefaulterBatchSummary(billMonth, cd, name, dt);
                }

            }

            return _DefaulterSummary;
        }

        public DefaultListRefWise GetDefListRefWise(string code, string rs, string age, string batch, string pgvt, string rundc, string trf, string srt)
        {
            DefaultListRefWise _DefaulterSummary = new DefaultListRefWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefListRefWise(code, DateTime.Now.AddMonths(-1), rs, age, batch,pgvt,rundc,trf,srt);
            if (dt != null && dt.Rows.Count > 0)
            {
                string billMonth = utility.GetFormatedDateYYYY(utility.GetColumnValue(dt.Rows[0], "BILLMONTH"));
                string cd = utility.GetColumnValue(dt.Rows[0], "CODE");
                string name = utility.GetColumnValue(dt.Rows[0], "NAME");
                string slab = utility.GetColumnValue(dt.Rows[0], "SLAB");
                _DefaulterSummary = new DefaultListRefWise(billMonth, cd, name, slab, dt);

            }

            return _DefaulterSummary;
        }
        public DefaulterBatchSummary GetDefaultListCntrWise(string code, string rs, string age, string batch, string pgvt, string rundc, string trf, string srt)
        {
            DefaulterBatchSummary _DefaulterSummary = new DefaulterBatchSummary();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefConsListCenterWise(code, DateTime.Now.AddMonths(-1),rs,age,batch,pgvt,rundc,trf,srt);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dvHead = dt.DefaultView;
                filterExp.AppendFormat("LEN(CODE) = {0}", (code.Length).ToString());
                dvHead.RowFilter = filterExp.ToString();
                if (dvHead != null && dvHead.ToTable().Rows.Count > 0)
                {
                    string billMonth = utility.GetFormatedDateYYYY(utility.GetColumnValue(dvHead.ToTable().Rows[0], "BILLMONTH"));
                    string cd = utility.GetColumnValue(dvHead.ToTable().Rows[0], "CODE");
                    string name = utility.GetColumnValue(dvHead.ToTable().Rows[0], "NAME");
                    _DefaulterSummary = new DefaulterBatchSummary(billMonth, cd, name, dt);
                }

            }

            return _DefaulterSummary;
        }
        public DefaulterSummary GetDefaulterSummaryAge(string code, string type, string status, string tariff)
        {
            DefaulterSummary _DefaulterSummary = new DefaulterSummary();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.GetDefSummAgeSlab(code, DateTime.Now.AddMonths(-1), type, status, tariff);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "CODE DESC";
                DataTable dt1 = dv.ToTable();
                
                if (dt1 != null)
                {
                    string billMonth = utility.GetColumnValue(dt1.Rows[0], "BILLMONTH");
                    string cd = utility.GetColumnValue(dt1.Rows[0], "CODE");
                    string name = utility.GetColumnValue(dt1.Rows[0], "NAME");
                    _DefaulterSummary = new DefaulterSummary(billMonth, cd, name, dt1);
                }
            }

            return _DefaulterSummary;
        }
        public DefaulterSummary GetDefaulterSummaryAmnt(string code, string type, string status, string tariff)
        {
            DefaulterSummary _DefaulterSummary = new DefaulterSummary();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.GetDefSummAmntSlab(code, DateTime.Now.AddMonths(-1), type, status, tariff);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "CODE DESC";
                DataTable dt1 = dv.ToTable();
               
                if (dt1 != null)
                {
                    string billMonth = utility.GetColumnValue(dt1.Rows[0], "BILLMONTH");
                    string cd = utility.GetColumnValue(dt1.Rows[0], "CODE");
                    string name = utility.GetColumnValue(dt1.Rows[0], "NAME");
                    _DefaulterSummary = new DefaulterSummary(billMonth, cd, name, dt1);
                }
            }

            return _DefaulterSummary;
        }
        public CreditAdjustments GetCRAdjustments(string code,string BatchFrom, char unitFlag)
        {
            CreditAdjustments _CreditAdjustments = new CreditAdjustments();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.GetCRAdjustments(code, DateTime.Now.AddMonths(-1),BatchFrom, unitFlag);
            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "CODE DESC";
                DataTable dt1 = dv.ToTable();
                
                if (dt1 != null)
                {
                    string billMonth = utility.GetColumnValue(dt1.Rows[0], "BILLMONTH");
                    string cd = utility.GetColumnValue(dt1.Rows[0], "CODE");
                    string name = utility.GetColumnValue(dt1.Rows[0], "NAME");
                    _CreditAdjustments = new CreditAdjustments(billMonth, cd, name, dt1);
                }
            }

            return _CreditAdjustments;
        }
        
        public CreditAdjustmentsCentreWise GetCRAdjCentreWise(string pCode, string pBatchFrom, char pUnitFlag)
        {
            CreditAdjustmentsCentreWise _CreditAdjustments = new CreditAdjustmentsCentreWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.GetCRAdjustmentsCentreWise(pCode, DateTime.Now.AddMonths(-1), pBatchFrom,  pUnitFlag);
            if (dt != null)
            {
                //DataView dv = dt.DefaultView;
                //dv.Sort = "CODE DESC";
                //DataTable dt1 = dv.ToTable();

                if (dt != null)
                {
                    _CreditAdjustments = new CreditAdjustmentsCentreWise(pCode, dt);
                }
            }

            return _CreditAdjustments;
        }
        public DefectMeterSumTrfWise GetDefectMeterSumTrfWise(string pCode)
        {
            DefectMeterSumTrfWise _DefectMeterSumTrfWise = new DefectMeterSumTrfWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.GetDefectMeterSumTrfWise(pCode, DateTime.Now.AddMonths(-1));
            if (dt != null)
            {
                //DataView dv = dt.DefaultView;
                //dv.Sort = "CODE DESC";
                //DataTable dt1 = dv.ToTable();
                
                if (dt != null)
                {
                    string billMonth = utility.GetColumnValue(dt.Rows[0], "BILLMONTH");
                    string cd = utility.GetColumnValue(dt.Rows[0], "CODE");

                    _DefectMeterSumTrfWise = new DefectMeterSumTrfWise(billMonth, pCode, dt);
                }
            }

            return _DefectMeterSumTrfWise;
        }
    
               public DefectiveDetails GetDefectiveDetails(string code, string age, string phase, string trf)
        {
           
            DefectiveDetails _DefectiveDetails = new DefectiveDetails();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.GetDefectiveMeterDetails(code, DateTime.Now.AddMonths(-1), age, phase, trf);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "CODE DESC";
                DataTable dt1 = dv.ToTable();

                if (dt1 != null)
                {
                    string billMonth = utility.GetColumnValue(dt1.Rows[0], "BILL_MONTH");
                    string cd = utility.GetColumnValue(dt1.Rows[0], "CODE");
                    string name = utility.GetColumnValue(dt1.Rows[0], "NAME");

                    _DefectiveDetails = new DefectiveDetails(billMonth, cd, name, dt1);
                }
            }

            return _DefectiveDetails;
        }

               public DefectiveDetailsR GetDefectiveMeterRegionWise(string code, string age, string trf)
               {
                   DefectiveDetailsR _DefectiveRegionWise = new DefectiveDetailsR();
                   utility util = new utility();
                   DB_Utility objDbuTil = new DB_Utility(conStr);
                   DataTable dt = objDbuTil.GetDefectiveMeterRegionWise(code, DateTime.Now.AddMonths(-1), age, trf);
                   
                   if (dt != null && dt.Rows.Count > 0)
                   {
                       StringBuilder filterExp = new StringBuilder();
                       DataView dv = dt.DefaultView;
                       filterExp.AppendFormat("LEN(CODE) = {0}", (code.Length).ToString());
                       dv.RowFilter = filterExp.ToString();
                       dv.Sort = "CODE ASC";
                       DataTable dt1 = dv.ToTable();

                       if (dt1 != null)
                       {
                           string billMonth = utility.GetColumnValue(dt1.Rows[0], "BILL_MONTH");
                           string cd = utility.GetColumnValue(dt1.Rows[0], "CODE");
                           string name = utility.GetColumnValue(dt1.Rows[0], "NAME");

                           _DefectiveRegionWise = new DefectiveDetailsR(billMonth, cd, name, dt);
                       }
                   }

                   return _DefectiveRegionWise;
               }
        public ExtraHeaveyBillRegion GetExtraHeaveyBillRegion(string code)
        {
            ExtraHeaveyBillRegion _ExtraHeaveyBillRegion = new ExtraHeaveyBillRegion();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.GetExtraHeaveyBillRegion(code, DateTime.Now.AddMonths(-1));
            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "CODE DESC";
                DataTable dt1 = dv.ToTable();
                
                if (dt1 != null)
                {
                    string billMonth = utility.GetColumnValue(dt1.Rows[0], "BillingMonth");
                    _ExtraHeaveyBillRegion = new ExtraHeaveyBillRegion(billMonth, dt1);
                }
            }

            return _ExtraHeaveyBillRegion;
        }

        public ExtraHeaveyBill GetExtraHeaveyBill(string code)
        {
            ExtraHeaveyBill _ExtraHeaveyBill = new ExtraHeaveyBill();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.GetExtraHeaveyBill(code, DateTime.Now.AddMonths(-1));
            StringBuilder filterExp = new StringBuilder();
            
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;

                filterExp.AppendFormat("LEN(CODE) = {0} ", (code.Length).ToString());
                string cd = utility.GetColumnValue(dt.Rows[0], "CODE");
                string name = utility.GetColumnValue(dt.Rows[0], "NAME");
                string billMonth = utility.GetColumnValue(dt.Rows[0], "BillingMonth");
                _ExtraHeaveyBill = new ExtraHeaveyBill(cd, name, billMonth, dt);
            }

            return _ExtraHeaveyBill;
        }

        public List<CashCollection> GetCashCollSummary(string code)
        {
            List<CashCollection> coll = new List<CashCollection>();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.GetCashCollSummary(code, DateTime.Now.AddMonths(-1));
            StringBuilder filterExp = new StringBuilder();
            
            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                if (!string.IsNullOrEmpty(code))
                {
                    filterExp.AppendFormat("LEN(CODE) >= {0} and LEN(CODE) <= {1}", (code.Length).ToString(),
                        (code.Length + 1).ToString());
                }

                dv.RowFilter = filterExp.ToString();
                dv.Sort = "SRT_ORDER2 ASC";
                DataView distinctview = new DataView(dt);
                DataTable distinctValues = distinctview.ToTable(true, "CODE", "NAME");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    string cd = utility.GetColumnValue(dr, "CODE");
                    string name = utility.GetColumnValue(dr, "NAME");
                    dv.RowFilter = string.Format("CODE = '{0}'", cd);
                    DataTable dt1 = dv.ToTable();
                    coll.Add(new CashCollection(cd, name, dt1));
                }
            }

            return coll;
        }

        public List<FeederLosses> GetFeederLosses(string token)
        {
            List<FeederLosses> coll = new List<FeederLosses>();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.GetFeederLosses(DateTime.Now.AddMonths(-1));
            StringBuilder filterExp = new StringBuilder();
           
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    coll.Add(new FeederLosses(dr));
                }
            }

            return coll;
        }

        public Bill GetBill(string kwh, string trf)
        {
            DataTable dt;
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            Bill bObj;

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            dt = objDbuTil.getBillData("1", kwh, firstDayOfMonth, lastDayOfMonth, trf);

            if (dt != null && dt.Rows.Count > 0)
            {
                return new Bill(dt.Rows[0]["ENRCHRG"].ToString(), dt.Rows[0]["TR_SUR"].ToString(), "0", "0", "0", "0",
                    "0", "0", "0", dt.Rows[0]["BILLSLABS"].ToString());
            }

            return null;

        }

        public List<CollectCompAssMnt> GetCollVsCompAssMnt(string code)
        {
            DataTable dt;
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            List<CollectCompAssMnt> collCompAssMnt = new List<CollectCompAssMnt>();
            StringBuilder filterExp = new StringBuilder();

            if (!string.IsNullOrEmpty(code))
            {
                filterExp.AppendFormat("LEN(SDIVCODE) >= {0} and LEN(SDIVCODE) <= {1}", (code.Length).ToString(),
                    (code.Length + 1).ToString());
            }

            dt = objDbuTil.GetCollVsCompAssmnt(code);

            if (dt != null)
            {
                //int i = 0;
                DataView dv = dt.DefaultView;
                dv.RowFilter = filterExp.ToString();
                dv.Sort = "SRT_ORDER2 ASC";
                foreach (DataRowView dr in dv)
                {
                    collCompAssMnt.Add(new CollectCompAssMnt(dr));

                }
            }

            return collCompAssMnt;
        }

        public List<CollectMonBilling> GetCollVsBilling(string code)
        {
            DataTable dt;
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            List<CollectMonBilling> collVsBilling = new List<CollectMonBilling>();
            StringBuilder filterExp = new StringBuilder();

            if (!string.IsNullOrEmpty(code))
            {
                filterExp.AppendFormat("LEN(SDIVCODE) >= {0} and LEN(SDIVCODE) <= {1}", (code.Length).ToString(),
                    (code.Length + 1).ToString());
            }

            dt = objDbuTil.GetCollVsBilling(code);

            if (dt != null)
            {
                //int i = 0;
                DataView dv = dt.DefaultView;
                dv.RowFilter = filterExp.ToString();
                dv.Sort = "SRT_ORDER2 ASC";
                foreach (DataRowView dr in dv)
                {
                    collVsBilling.Add(new CollectMonBilling(dr));

                }
            }

            return collVsBilling;
        }

        public List<ReceivSpillArrear> GetReceivable(string code)
        {
            DataTable dt;
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            List<ReceivSpillArrear> receivSpillArrears = new List<ReceivSpillArrear>();
            StringBuilder filterExp = new StringBuilder();

            if (!string.IsNullOrEmpty(code))
            {
                filterExp.AppendFormat("LEN(CODE) >= {0} and LEN(CODE) <= {1}", (code.Length).ToString(),
                    (code.Length + 1).ToString());
            }

            dt = objDbuTil.getReceiveables(code, DateTime.Now.AddMonths(-1));

            if (dt != null)
            {
                //int i = 0;
                DataView dv = dt.DefaultView;
                dv.RowFilter = filterExp.ToString();
                dv.Sort = "SRT_ORDER2 ASC";
                foreach (DataRowView dr in dv)
                {
                    receivSpillArrears.Add(new ReceivSpillArrear(dr));

                }
            }

            return receivSpillArrears;
        }

        public List<MonLosses> GetMonLosses(string code)
        {
            DataTable dt;
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            List<MonLosses> monLosseses = new List<MonLosses>();
            StringBuilder filterExp = new StringBuilder();

            if (!string.IsNullOrEmpty(code))
            {
                filterExp.AppendFormat("LEN(SDIV) >= {0} and LEN(SDIV) <= {1}", (code.Length).ToString(),
                    (code.Length + 1).ToString());
            }

            dt = objDbuTil.getMonLosses(code, DateTime.Now.AddMonths(-1));

            if (dt != null)
            {
                //int i = 0;
                DataView dv = dt.DefaultView;
                dv.RowFilter = filterExp.ToString();
                dv.Sort = "SRT_ORDER2 ASC";
                foreach (DataRowView dr in dv)
                {
                    monLosseses.Add(new MonLosses(dr));

                }
            }

            return monLosseses;
        }

        public List<MonLosses> GetPrgsLosses(string code)
        {
            DataTable dt;
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            List<MonLosses> prgsLosseses = new List<MonLosses>();
            StringBuilder filterExp = new StringBuilder();

            if (!string.IsNullOrEmpty(code))
            {
                filterExp.AppendFormat("LEN(SDIV) >= {0} and LEN(SDIV) <= {1}", (code.Length).ToString(),
                    (code.Length + 1).ToString());
            }

            dt = objDbuTil.getPrgsLosses(code, DateTime.Now.AddMonths(-1));

            if (dt != null)
            {
                //int i = 0;
                DataView dv = dt.DefaultView;
                dv.RowFilter = filterExp.ToString();
                dv.Sort = "SRT_ORDER2 ASC";
                foreach (DataRowView dr in dv)
                {
                    prgsLosseses.Add(new MonLosses(dr));

                }
            }

            return prgsLosseses;
        }

        public BillStatsContainer GetBillingStatsBatchWise(string code)
        {
            DataTable dtD, dtB;
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            StringBuilder filterExp = new StringBuilder();

            if (!string.IsNullOrEmpty(code))
            {
                filterExp.AppendFormat("LEN(CODE) >= {0} and LEN(CODE) <= {1}", (code.Length).ToString(),
                    (code.Length + 1).ToString());
            }

            dtD = objDbuTil.getBillingStatsDaily(code, DateTime.Now.AddMonths(-1));
            dtB = objDbuTil.getBillingStatsBatchWise(code, DateTime.Now.AddMonths(-1));

            if (dtD != null)
            {
                DataView dvD = dtD.DefaultView;
                dvD.RowFilter = filterExp.ToString();
                dvD.Sort = "SRT_ORDER2 ASC";

                DataView dvB = dtB.DefaultView;
                dvB.RowFilter = filterExp.ToString();
                dvB.Sort = "SRT_ORDER2 ASC";


                BillStatsContainer billStContainer = new BillStatsContainer(code, dvD.ToTable(), dvB.ToTable());
                return billStContainer;

            }
            return null;
        }

        //
        public BillStatsDailyContainer GetBillingStatsDaily(string code)
        {
            DataTable dt;
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            StringBuilder filterExp = new StringBuilder();

            if (!string.IsNullOrEmpty(code))
            {
                filterExp.AppendFormat("LEN(CODE) >= {0} and LEN(CODE) <= {1}", (code.Length).ToString(),
                    (code.Length + 1).ToString());
            }

            dt = objDbuTil.getBillingStatsDaily(code, DateTime.Now.AddMonths(-1));

            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = filterExp.ToString();
                dv.Sort = "SRT_ORDER2 ASC";
                BillStatsDailyContainer billStContainer = new BillStatsDailyContainer(code, dv.ToTable());
                return billStContainer;
            }

            return null;
        }

        public List<TheftMND> GetTheftFromMND(string refNo)
        {
            DataTable dt;
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            StringBuilder filterExp = new StringBuilder();
            List<TheftMND> theftData = new List<TheftMND>();

            dt = objDbuTil.getTheftData(refNo);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    theftData.Add(new TheftMND(dr["BILL_MNTH"].ToString(), dr["NOTE_NO"].ToString(),
                        dr["ADJ_DT"].ToString(), dr["UNITS"].ToString(), dr["AMOUNT"].ToString(),
                        dr["PAY_AGAINTS_DET"].ToString()));
                }
            }

            return theftData;
        }

        public List<AssmntBatchWiseObject> GetAssesmentBatchWise(string code)
        {
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility(conStr);
            DataTable dt = objDbuTil.getAssesmentBatchWise(code, DateTime.Now.AddMonths(-1));
            StringBuilder filterExp = new StringBuilder();
            List<AssmntBatchWiseObject> coll = new List<AssmntBatchWiseObject>();
            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                if (!string.IsNullOrEmpty(code))
                {
                    filterExp.AppendFormat("LEN(SDIV_CODE) >= {0} and LEN(SDIV_CODE) <= {1}", (code.Length).ToString(),
                        (code.Length + 1).ToString());
                }

                dv.RowFilter = filterExp.ToString();
                dv.Sort = "SRT_ORDER2 ASC";
                DataView distinctview = new DataView(dt);
                DataTable distinctValues = distinctview.ToTable(true, "SDIV_CODE", "SDIV_NAME", "MONTH");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    string cd = utility.GetColumnValue(dr, "SDIV_CODE");
                    string name = utility.GetColumnValue(dr, "SDIV_NAME");
                    string month = utility.GetFormatedDate(utility.GetColumnValue(dr, "MONTH"));
                    dv.RowFilter = string.Format("SDIV_CODE = '{0}'", cd);
                    DataTable dt1 = dv.ToTable();
                    coll.Add(new AssmntBatchWiseObject(cd, name, month, dt1));
                }
            }

            return coll;
        }

        public static string GetBillingStatus(string token)
        {
            string ret = "Error";
           
            try
            {
                DB_Utility objDBUTil = new DB_Utility(conStr);
                DataTable dt = objDBUTil.getBillingStatus();
                utility util = new utility();

                ret = util.DataTableToJSONWithStringBuilder(dt);
            }
            catch (Exception ex)
            {
                ret = ex.ToString();
            }

            return ret;
        }

    }
}