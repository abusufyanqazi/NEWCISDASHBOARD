using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using util;

namespace DashBoardAPI.Models
{
    public class DBoardBridge
    {
        //updated for local use only at obaid machine
        

        public GvtVsAssmntDeptWise DepWiseInfo(string code, string bmonth)
        {

            DataSet resultSet = new DB_Utility().GetDepWiseInfo(code, bmonth);
            DeptInfo temp = null;
            List<DeptInfo> depts = new List<DeptInfo>();
            foreach (DataRow row in resultSet.Tables[1].Rows)
            {
                temp = new DeptInfo(row["DEPTCODE"].ToString(), row["DEPTTYPEDESC"].ToString(), row["NO_OF_CON"].ToString(), row["OP_BAL"].ToString(),
                    row["TOT_ASSESS"].ToString(), row["PAYMENT"].ToString(), row["CL_BAL"].ToString());
                depts.Add(temp);
            }

            GvtVsAssmntDeptWise result = new GvtVsAssmntDeptWise(bmonth, code, resultSet.Tables[0].Rows[0]["SDIV_NAME"].ToString(), depts);
            return result;


        }
        public GvtVsAssmntCenterWiseBillData CenterWiseDepInfo(string code, string bmonth)
        {
            DataSet resultSet = new DB_Utility().GetCenterWiseDeptInfo(code, bmonth);
            List<CenterWiseDepts> centerWiseList = new List<CenterWiseDepts>();
            DeptInfo temp = null;
            for (int i = 1; i < resultSet.Tables.Count; i++)
            {
                List<DeptInfo> depts = new List<DeptInfo>();
                if(resultSet.Tables[i].Rows.Count != 0)
                {
                    foreach(DataRow row in resultSet.Tables[i].Rows)
                    {
                        temp = new DeptInfo(row["DEPTCODE"].ToString(), row["DEPTTYPEDESC"].ToString(), row["NO_OF_CON"].ToString(), row["OP_BAL"].ToString(),
                            row["TOT_ASSESS"].ToString(), row["PAYMENT"].ToString(), row["CL_BAL"].ToString());
                        depts.Add(temp);
                    }

                    CenterWiseDepts temp2 = new CenterWiseDepts(resultSet.Tables[i].Rows[0]["SDIV_CODE"].ToString().Substring(0, code.Length + 1),
                        resultSet.Tables[i].Rows[0]["SDIV_NAME"].ToString(), depts);
                    centerWiseList.Add(temp2);
                }

            }
            GvtVsAssmntCenterWiseBillData result = new GvtVsAssmntCenterWiseBillData(bmonth, code, resultSet.Tables[0].Rows[0]["SDIV_NAME"].ToString(), centerWiseList);
            return result;
        }

        public TariffWiseBilling TrfWiseBillData(string code, string bmonth)
        {
            DB_Utility dbUtilObj = new DB_Utility();
            DataSet records = dbUtilObj.GetTariffWiseBill(code, bmonth);

            List<RgnWiseTrf> rgnlist = new List<RgnWiseTrf>();
            TariffWise temp = null;
            TariffWise temp2 = null;
            for (int i = 1; i < records.Tables.Count; i += 2)
            {
                List<TariffWise> domestic = new List<TariffWise>();
                List<TariffWise> commercial = new List<TariffWise>();
                if (records.Tables[i].Rows.Count != 0 || records.Tables[i + 1].Rows.Count != 0)
                {
                    if (records.Tables[i].Rows.Count != 0)
                    {
                        foreach (DataRow row in records.Tables[i].Rows)
                        {
                            temp = new TariffWise(row["TARIFF_CATEGORY"].ToString(), row["CONNECTIONS"].ToString(),
                            row["UNITS"].ToString(), row["BILLING"].ToString(), row["PAYMENT"].ToString(),
                            row["CLOSING"].ToString(), row["SPILLOVER"].ToString(), row["TOT_PERCENT"].ToString());
                            domestic.Add(temp);
                        }

                    }
                    if (records.Tables[i + 1].Rows.Count != 0)
                    {
                        foreach (DataRow row in records.Tables[i + 1].Rows)
                        {
                            temp2 = new TariffWise(row["TARIFF_CATEGORY"].ToString(), row["CONNECTIONS"].ToString(),
                            row["UNITS"].ToString(), row["BILLING"].ToString(), row["PAYMENT"].ToString(),
                            row["CLOSING"].ToString(), row["SPILLOVER"].ToString(), row["TOT_PERCENT"].ToString());
                            commercial.Add(temp2);
                        }
                    }

                    TrfWiseTypes tObj = new TrfWiseTypes(domestic, commercial);
                    if (records.Tables[i].Rows.Count != 0)
                    {
                        RgnWiseTrf tObj2 = new RgnWiseTrf(records.Tables[i].Rows[0]["SDIV_CODE"].ToString().Substring(0, code.Length+1), 
                            records.Tables[i].Rows[0]["SDIV_NAME"].ToString(), tObj);
                        rgnlist.Add(tObj2);
                    }
                    else if (records.Tables[i + 1].Rows.Count != 0)
                    {
                        RgnWiseTrf tObj2 = new RgnWiseTrf(records.Tables[i + 1].Rows[0]["SDIV_CODE"].ToString().Substring(0, code.Length+1), 
                            records.Tables[i + 1].Rows[0]["SDIV_NAME"].ToString(), tObj);
                        rgnlist.Add(tObj2);
                    }

                }
            }
            TariffWiseBilling result = new TariffWiseBilling(bmonth, code, "", rgnlist);
            return result;

        }


        public TentativeRefWiseList ListTentativeRefWise(string code, string ason)
        {
            DB_Utility dbUtilObj = new DB_Utility();
            DataTable records = dbUtilObj.GetRefWiseTentative(code, ason);
            List<RefWiseTentative> defaulters = new List<RefWiseTentative>();
            if (records != null)
            {
                foreach (DataRow row in records.Rows)
                {
                    RefWiseTentative temp = new RefWiseTentative(row["SRNO"].ToString(), row["REF_NO"].ToString(), row["TARIFF_ACTIVE"].ToString(),
                        System.Text.RegularExpressions.Regex.Replace(row["CONSUMER_NAME"].ToString(), @"\s+", " "),
                        System.Text.RegularExpressions.Regex.Replace(row["ADDRESS"].ToString(), @"\s+", " "), row["ER0NO"].ToString(),
                        row["ERODATE"].ToString(), row["AGE"].ToString(), row["UNPAID_AMOUNT"].ToString(), row["DEFERREDAMOUNT"].ToString(),
                        row["DFL_OWNING_AMNT"].ToString(), row["CL_TOT_AMNT"].ToString());
                    defaulters.Add(temp);
                }

                //string monend = records.Rows[0]["B_PERIOD"].ToString().Split(' ')[0];
                //int a = monend.Split('-')[1].Length;
                //if (a == 2)
                //{
                //    monend = monend.Split('-')[0] + "-" + monend.Split('-')[2];
                //}
                //else
                //{
                //    monend = "0" + monend.Split('-')[0] + "-" + monend.Split('-')[2];
                //}
                
                DateTime monend = DateTime.Parse(records.Rows[0]["B_PERIOD"].ToString());
                TentativeRefWiseList result = new TentativeRefWiseList(monend.ToString("MMM-yyyy", CultureInfo.InvariantCulture), monend.AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture), records.Rows[0]["RGNCODE"].ToString(),
                    records.Rows[0]["RGNNAME"].ToString(), defaulters);
                return result;
            }
            else
            {
                return null;
            }
        }
        public MonthlyDflRegWise DefListRegWise(string code, string asOn)
        {
            DB_Utility dbUtilObj = new DB_Utility();
            DataTable records = dbUtilObj.GetRegWiseDefaulters(code, asOn);
            List<RegWiseDfl> defaulters = new List<RegWiseDfl>();
            if (records != null)
            {
                foreach (DataRow row in records.Rows)
                {
                    RegWiseDfl temp = new RegWiseDfl(row["CENTERCODE"].ToString(), row["CENTERNAME"].ToString(), row["UNPAIDAMOUNT"].ToString(), row["DEFFEREDAMOUNT"].ToString(),
                    row["OWNINGAMOUNT"].ToString(), row["CLOSINGAMOUNT"].ToString());
                    defaulters.Add(temp);
                }

                DateTime monend = DateTime.Parse(records.Rows[0]["B_PERIOD"].ToString());

                MonthlyDflRegWise regWiseDfl = new MonthlyDflRegWise(monend.ToString("MMM-yyyy", CultureInfo.InvariantCulture), monend.AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture),
                    records.Rows[0]["RGNCODE"].ToString(),
                    records.Rows[0]["RGNNAME"].ToString(), defaulters);
                return regWiseDfl;
            }
            else
            {
                return null;
            }
        }
        public DefConsListFdrWise GetDefConsListFdrCdWise(string code, string fdrCode)
        {
            DefConsListFdrWise _DefConsListFdrWise = new DefConsListFdrWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefListFeederWise(code, DateTime.Now.AddMonths(-1), fdrCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                string billMonth = utility.GetFormatedDateYYYY(utility.GetColumnValue(dt.Rows[0], "BILLMONTH"));
                string cd = utility.GetColumnValue(dt.Rows[0], "CODE");
                //string name = utility.GetColumnValue(dt.Rows[0], "FEEDER_NAME");
                string name = objDbuTil.GetFeederNameByCode(code, fdrCode);
                _DefConsListFdrWise = new DefConsListFdrWise(cd, name, dt);

            }
            return _DefConsListFdrWise;
        }

        //
        public DefaultSumFdrWise GetDefConsFdrCdWise(string code)
        {
            DefaultSumFdrWise _DefaultSumFdrWise = new DefaultSumFdrWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefConsFdrCdWise(code, DateTime.Now.AddMonths(-1));
            if (dt != null && dt.Rows.Count > 0)
            {
                string billMonth = utility.GetFormatedDateYYYY(utility.GetColumnValue(dt.Rows[0], "BILLMONTH"));
                string cd = utility.GetColumnValue(dt.Rows[0], "CODE");
                string name = utility.GetColumnValue(dt.Rows[0], "NAME");
                _DefaultSumFdrWise = new DefaultSumFdrWise(billMonth, cd, name, dt);

            }

            return _DefaultSumFdrWise;
        }
        public DefaultListRefWise GetDefListRefWise(string code, string type, string status, string tariff, string pSlab, char flagAgAMnt)
        {
            DefaultListRefWise _DefaulterSummary = new DefaultListRefWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefListRefWise(code, DateTime.Now.AddMonths(-1), type, status, tariff, pSlab, flagAgAMnt);
            if (dt != null && dt.Rows.Count > 0)
            {
                string billMonth = utility.GetFormatedDateYYYY(utility.GetColumnValue(dt.Rows[0], "BILLMONTH"));
                string cd = utility.GetColumnValue(dt.Rows[0], "CODE");
                string name = utility.GetColumnValue(dt.Rows[0], "NAME");
                string slab = utility.GetColumnValue(dt.Rows[0], "SLAB");
                _DefaulterSummary = new DefaultListRefWise(billMonth, cd, name, pSlab, dt);

            }

            return _DefaulterSummary;
        }
        public DefaulterBatchSummary GetDefaulterSummaryCntrWise(string pCode, string pAge, string pPvtGvt, string pRundisc, string pTrf)
        {
            DefaulterBatchSummary _DefaulterSummary = new DefaulterBatchSummary();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefConsSumBatch(pCode, DateTime.Now.AddMonths(-1), pAge, pPvtGvt, pRundisc, pTrf);
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
            DB_Utility objDbuTil = new DB_Utility();
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

                    _DefectMeterSumMonWise = new DefectMeterSumMonWise(billMonth, cd, name, dt);
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefConsSumBatch(pCode, DateTime.Now.AddMonths(-1), pPvtGvt, pRundisc, pTrf);
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

        public DefaultListRefWise GetDefListRefWise(string code, string rs, string age, string batch, string pgvt, string rundc, string trf, string srt)
        {
            DefaultListRefWise _DefaulterSummary = new DefaultListRefWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefListRefWise(code, DateTime.Now.AddMonths(-1), rs, age, batch, pgvt, rundc, trf, srt);
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
            DB_Utility objDbuTil = new DB_Utility();
            StringBuilder filterExp = new StringBuilder();

            DataTable dt = objDbuTil.GetDefConsListCenterWise(code, DateTime.Now.AddMonths(-1), rs, age, batch, pgvt, rundc, trf, srt);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dvHead = dt.DefaultView;
                //filterExp.AppendFormat("LEN(CODE) = {0}", (code.Length).ToString());
                //dvHead.RowFilter = filterExp.ToString();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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

        public DefaultSumCentreWise GetDefaulterSummaryAgeCentreWise(string pCode, string pType, string pStatus, string pTariff)
        {
            DefaultSumCentreWise _DefaulterSummary = new DefaultSumCentreWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
            DataTable dt = objDbuTil.GetDefSummAgeSlabCentreWise(pCode, DateTime.Now.AddMonths(-1), pType, pStatus, pTariff);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                StringBuilder filterExp = new StringBuilder();
                filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length).ToString());
                dv.RowFilter = filterExp.ToString();
                dv.Sort = "SRT_ORDER1";
                DataTable dt1 = dv.ToTable();
                if (dt1 != null)
                {
                    string billMonth = utility.GetColumnValue(dt1.Rows[0], "BILLMONTH");
                    string cd = utility.GetColumnValue(dt1.Rows[0], "CODE");
                    string name = utility.GetColumnValue(dt1.Rows[0], "NAME");
                    _DefaulterSummary = new DefaultSumCentreWise(billMonth, pCode, name, dt);
                }
            }

            return _DefaulterSummary;
        }

        public DefaultSumCentreWise GetDefaulterSummaryAmntCentreWise(string pCode, string pType, string pStatus, string pTariff)
        {
            DefaultSumCentreWise _DefaulterSummary = new DefaultSumCentreWise();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
            DataTable dt = objDbuTil.GetDefSummAmntSlabCentreWise(pCode, DateTime.Now.AddMonths(-1), pType, pStatus, pTariff);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                StringBuilder filterExp = new StringBuilder();
                filterExp.AppendFormat("LEN(CODE) = {0}", (pCode.Length).ToString());
                dv.RowFilter = filterExp.ToString();
                dv.Sort = "SRT_ORDER1";
                DataTable dt1 = dv.ToTable();

                if (dt1 != null)
                {
                    string billMonth = utility.GetColumnValue(dt1.Rows[0], "BILLMONTH");
                    string cd = utility.GetColumnValue(dt1.Rows[0], "CODE");
                    string name = utility.GetColumnValue(dt1.Rows[0], "NAME");
                    _DefaulterSummary = new DefaultSumCentreWise(billMonth, pCode, name, dt);
                }
            }

            return _DefaulterSummary;
        }

        public DefaulterSummary GetDefaulterSummaryAmntBySproc(string code, string type, string status, string tariff)
        {
            DefaulterSummary _DefaulterSummary = new DefaulterSummary();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
            DataTable dt = objDbuTil.GetDefConsSumAmntSlabsBySproc(code, type, status, tariff, code);
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
        public CreditAdjustments GetCRAdjustments(string code, string BatchFrom, char unitFlag)
        {
            CreditAdjustments _CreditAdjustments = new CreditAdjustments();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
            DataTable dt = objDbuTil.GetCRAdjustments(code, DateTime.Now.AddMonths(-1), BatchFrom, unitFlag);
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
            DB_Utility objDbuTil = new DB_Utility();
            DataTable dt = objDbuTil.GetCRAdjustmentsCentreWise(pCode, DateTime.Now.AddMonths(-1), pBatchFrom, pUnitFlag);
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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

        public List<FeederLosses> GetFeederLosses(string code)
        {
            List<FeederLosses> coll = new List<FeederLosses>();
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
            DataTable dt = objDbuTil.GetFeederLosses(code,DateTime.Now.AddMonths(-1));
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

        public Bill GetBill(string kwh, string trf, string ed_cd, string seasonchrg, string stdclsfcd,
            string gstexmtcd, string fixchrg, string mtrent, string srvrent, string dssur, string uosr, string nooftv,
            string fatapatacd, string p_tot_cons)
        {
            DataTable dt;
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
            Bill bObj;

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            dt = objDbuTil.getBillData("1", kwh, firstDayOfMonth, lastDayOfMonth, trf, ed_cd, seasonchrg, stdclsfcd, gstexmtcd, fixchrg, mtrent, srvrent, dssur, uosr, p_tot_cons, nooftv, fatapatacd);

            if (dt != null && dt.Rows.Count > 0)
            {
                return new Bill(dt.Rows[0]["ENRCHRG"].ToString(), "0", "0", dt.Rows[0]["ED"].ToString(), dt.Rows[0]["GST"].ToString(), dt.Rows[0]["PTVFEE"].ToString(),
                    dt.Rows[0]["NJSUR"].ToString(), "0", "0", "0", dt.Rows[0]["ITAX"].ToString(), dt.Rows[0]["EQSUR"].ToString());
            }

            return null;

        }

        public List<CollectCompAssMnt> GetCollVsCompAssMnt(string code)
        {
            DataTable dt;
            utility util = new utility();
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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
            DB_Utility objDbuTil = new DB_Utility();
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

        public static string GetBillingStatus(string pCode)
        {
            string ret = "Error";

            try
            {
                DB_Utility objDBUTil = new DB_Utility();
                DataTable dt = objDBUTil.getBillingStatus(pCode);
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