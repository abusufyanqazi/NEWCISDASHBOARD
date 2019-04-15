using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
//using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;

/// <summary>
/// Summary description for DB_Utility
/// </summary>

namespace DAL
{
    public class DB_Utility
    {
        private string _constr;
        public DB_Utility(string conStr)
        {
            //
            // TODO: Add constructor logic here
            //
            _constr = conStr;
        }
        
        //VW_DEF_CONS_SUM_FDRCODE_WISE
        
        public DataTable GetDefConsFdrCdWise(string pCode, DateTime pBillMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sql = @" SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME, FDRCODE, FDRNAME, MAINDT, BILLMONTH, TOT_CONS, AMOUNT  " +
                         "FROM VW_DEF_CONS_SUM_FDRCODE_WISE " +
                         "WHERE 1=1";
            //" WHERE BPERIOD=(SELECT MAX(BPERIOD) FROM VW_DEFAULTER_LIST)";

            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + code + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " AND SDIVCODE like '" + pCode + "%'";
            }

            sql += " ORDER BY SRT_ORDER1, FDRCODE";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }


        //VW_DEFAULTER_LIST
        //TODO: NULL SLAB
        public DataTable GetDefListRefWise(string pCode, DateTime pBillMon, string pType, string pStatus, string pTariff, string pSlab)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sql = @" SELECT NULL SLAB, ROWNUM SrNO, SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME, BPERIOD BILLMONTH, MAINDT, REFNO, TARRIFCODE, DEF_TYPE, DEF_STATUS,  " +
                         "TARIFF_CAT, NAMEADD, DCN_NO, DCN_DATE, MTR_NO, AGE, AMOUNT " +
                         "FROM VW_DEFAULTER_LIST " +
                         "WHERE 1=1";
            //" WHERE BPERIOD=(SELECT MAX(BPERIOD) FROM VW_DEFAULTER_LIST)";

            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + code + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " AND SDIVCODE like '" + pCode + "%'";
            }

            if (!string.IsNullOrEmpty(pType))
            {
                sql += " AND DEF_TYPE = '" + pType + "'";
            }

            if (!string.IsNullOrEmpty(pStatus))
            {
                sql += " AND DEF_STATUS = '" + pStatus + "'";

            }

            if (!string.IsNullOrEmpty(pTariff))
            {
                sql += " AND TARIFF_CAT = '" + pTariff + "'";

            }
            //ToDo Column missing in view
            //if (!string.IsNullOrEmpty(pSlab))
            //{
            //    sql += " AND SLAB_NAME = '" + pSlab + "'";

            //}

            sql += " ORDER BY SRT_ORDER1, REFNO";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public DataTable GetDefListRefWise(string pCode, DateTime pBillMon, string pRs, string pAge, string pBatch, string pPvtGvt, string pRundisc, string pTrf, string pSrtBy)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);
            string sortorder = "";
            string fromRs = pRs;
            string toRs = pRs;
            string filter = string.Empty;

            if (pRs.IndexOf("-") > 0)
            {
                fromRs = pRs.Split('-')[0];
                toRs = pRs.Split('-')[1];
            }

            filter += " AND AMOUNT BETWEEN " + fromRs + " AND " + toRs;

            if (!string.Empty.Equals(pTrf) && !pTrf.ToUpper().Equals("ALL"))
                filter += " AND  TARIFF_CAT LIKE '" + pTrf + "'";

            if (!string.Empty.Equals(pBatch) && !pBatch.ToUpper().Equals("ALL"))
                filter += " AND REFNO LIKE '" + pAge.PadLeft(2, '0') + "%'";

            if (!string.Empty.Equals(pAge))
                filter += " AND AGE >= " + pAge;

            if (!string.Empty.Equals(pPvtGvt))
                filter += " AND DEF_TYPE LIKE '" + pPvtGvt + "'";

            if (!string.Empty.Equals(pRundisc))
                filter += " AND DEF_STATUS LIKE '" + pRundisc + "'";

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sql = @" SELECT NULL SLAB, ROWNUM SrNO, SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME, BPERIOD BILLMONTH, MAINDT, REFNO, TARRIFCODE, DEF_TYPE, DEF_STATUS,  " +
                         "TARIFF_CAT, NAMEADD, DCN_NO, DCN_DATE, MTR_NO, AGE, AMOUNT " +
                         "FROM VW_DEFAULTER_LIST " +
                         "WHERE 1=1";
            //" WHERE BPERIOD=(SELECT MAX(BPERIOD) FROM VW_DEFAULTER_LIST)";

            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + code + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " AND SDIVCODE like '" + pCode + "%'";
            }

            sql += " ORDER BY SRT_ORDER1, REFNO";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        //
        public DataTable GetDefConsListCenterWise(string pCode, DateTime pBillMon, string pRs, string pAge, string pBatch, string pPvtGvt, string pRundisc, string pTrf, string pSrtBy)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);
            string sortorder = "";
            string fromRs = pRs;
            string toRs = pRs;
            string filter = string.Empty;

            if (pRs.IndexOf("-") > 0)
            {
                fromRs = pRs.Split('-')[0];
                toRs = pRs.Split('-')[1];
            }
            
            filter += " AND AMOUNT BETWEEN " + fromRs   + " AND " + toRs;

            if (!string.Empty.Equals(pTrf) && !pTrf.ToUpper().Equals("ALL"))
                filter += " AND  TARIFF_CAT LIKE '" + pTrf + "'";
            
            if (!string.Empty.Equals(pBatch) && ! pBatch.ToUpper().Equals("ALL"))
                filter += " AND REFNO LIKE '" + pAge.PadLeft(2,'0') + "%'";

            if (!string.Empty.Equals(pAge))
                filter += " AND AGE >= " + pAge;

            if (!string.Empty.Equals(pPvtGvt))
                filter += " AND DEF_TYPE LIKE '" + pPvtGvt + "'";
            
            if (!string.Empty.Equals(pRundisc))
                filter += " AND DEF_STATUS LIKE '" + pRundisc + "'";

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            switch (pCode.Length)
            {
                case 2:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }
                case 3:
                    {
                        sortorder = "IN('2','3')";
                        break;
                    }
                case 4:
                    {
                        sortorder = "IN('1','2')";
                        break;
                    }
                default:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }


            }

            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME, BPERIOD BILLMONTH, count(refno) CONSUMERS, sum(AMOUNT) AMOUNT  " +
                         "FROM VW_DEFAULTER_LIST " +
                         " WHERE BPERIOD=(SELECT MAX(BPERIOD) FROM VW_DEFAULTER_LIST)";
            sql += " AND SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder + filter;
            sql += " GROUP BY SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIV_NAME, BPERIOD";
            //sql += " ORDER BY SRT_ORDER1";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }
        
        //VW_DEF_CONS_SUM_BATCH_WISE
        public DataTable GetDefConsSumBatch(string pCode, DateTime pBillMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);
            string sortorder = string.Empty;
            string filter = string.Empty;

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            switch (pCode.Length)
            {
                case 2:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }
                case 3:
                    {
                        sortorder = "IN('2','3')";
                        break;
                    }
                case 4:
                    {
                        sortorder = "IN('1','2')";
                        break;
                    }
                default:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }


            }

            string sql = @"SELECT SDIVCODE CODE, SDIV_NAME NAME, BILLMONTH, sum(TOT_CONS) CONSUMERS, sum(AMOUNT) AMOUNT " +
                         "FROM VW_DEF_CONS_SUM_BATCH_WISE " +
                         " WHERE BILLMONTH=(SELECT MAX(BILLMONTH) FROM VW_DEF_CONS_SUM_BATCH_WISE)";
            sql += " AND SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
            sql += " GROUP BY SDIVCODE, SDIV_NAME, BILLMONTH";
            //sql += " ORDER BY SRT_ORDER1";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }
        public DataTable GetDefConsSumBatch(string pCode, DateTime pBillMon, string pPvtGvt, string pRundisc, string pTrf)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);
            string sortorder = string.Empty;
            string filter = string.Empty;

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            switch (pCode.Length)
            {
                case 2:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }
                case 3:
                    {
                        sortorder = "IN('2','3')";
                        break;
                    }
                case 4:
                    {
                        sortorder = "IN('1','2')";
                        break;
                    }
                default:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }


            }


            if (!string.Empty.Equals(pPvtGvt))
                filter += " AND DEF_TYPE LIKE '" + pPvtGvt + "'";

            if (!string.Empty.Equals(pRundisc))
                filter += " AND DEF_STATUS LIKE '" + pRundisc + "'";

            string sql = @"SELECT SDIVCODE CODE, SDIV_NAME NAME, BPERIOD BILLMONTH, count(refno) CONSUMERS, sum(AMOUNT) AMOUNT  " +
                         "FROM VW_DEFAULTER_LIST " +
                         " WHERE BPERIOD=(SELECT MAX(BPERIOD) FROM VW_DEFAULTER_LIST)";
            sql += " AND SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder + filter;
            sql += " GROUP BY SDIVCODE, SDIV_NAME, BPERIOD";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }
        public DataTable GetDefConsSumBatch(string pCode, DateTime pBillMon, string pAge,string pPvtGvt, string pRundisc, string pTrf)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);
            string sortorder = string.Empty;
            string filter = string.Empty;

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            switch (pCode.Length)
            {
                case 2:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }
                case 3:
                    {
                        sortorder = "IN('2','3')";
                        break;
                    }
                case 4:
                    {
                        sortorder = "IN('1','2')";
                        break;
                    }
                default:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }


            }

            if (!string.Empty.Equals(pAge))
                filter += " AND AGE >= " + pAge;

            if (!string.Empty.Equals(pPvtGvt))
                filter += " AND DEF_TYPE LIKE '" + pPvtGvt + "'";

            if (!string.Empty.Equals(pRundisc))
                filter += " AND DEF_STATUS LIKE '" + pRundisc + "'";

            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME, BILLMONTH, sum(TOT_CONS) CONSUMERS, sum(AMOUNT) AMOUNT " +
                         "FROM VW_DEF_CONS_SUM_BATCH_WISE " +
                         " WHERE BILLMONTH=(SELECT MAX(BILLMONTH) FROM VW_DEF_CONS_SUM_BATCH_WISE)";
            sql += " AND SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder + filter;
            sql += " GROUP BY SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIV_NAME, BILLMONTH";
            sql += " ORDER BY SRT_ORDER1";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }
        
        public DataTable GetDefSummAgeSlab(string pCode, DateTime pBillMon, string pType, string pStatus, string pTariff)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string groupBy = " SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIV_NAME, B_PERIOD, SLAB_ID, SLAB_NAME ";
            string sql = @" SELECT ROWNUM SrNO, SRT_ORDER2, SRT_ORDER1, CODE, NAME , BILLMONTH, SLAB_NAME, CONSUMERS, AMOUNT FROM (";
            sql += @" SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME " +
                         ", B_PERIOD BILLMONTH, SLAB_NAME, SUM(CONSUMERS) CONSUMERS, SUM(AMOUNT) AMOUNT " +
                         "from VW_DEF_CONS_SUM_AGE_SLABS "+
            " WHERE B_PERIOD=(SELECT MAX(B_PERIOD) FROM VW_DEF_CONS_SUM_AGE_SLABS)";
            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + code + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " AND SDIVCODE = '" + pCode + "'";
            }

            if (!string.IsNullOrEmpty(pType))
            {
                sql += " AND DEF_TYPE = '" + pType + "'";
            }

            if (!string.IsNullOrEmpty(pStatus))
            {
                sql += " AND DEF_STATUS = '" + pStatus + "'";

            }

            if (!string.IsNullOrEmpty(pTariff))
            {
                sql += " AND TARIFF_CAT = '" + pTariff + "'";

            }

            sql += " GROUP BY " + groupBy;
            sql += " ORDER BY SRT_ORDER1, slab_id)";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public DataTable GetDefSummAmntSlab(string pCode, DateTime pBillMon, string pType, string pStatus, string pTariff)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string groupBy = " SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIV_NAME, B_PERIOD, SLAB_ID, SLAB_NAME ";
            string sql = @" SELECT ROWNUM SrNO, SRT_ORDER2, SRT_ORDER1, CODE, NAME , BILLMONTH, SLAB_NAME, CONSUMERS, AMOUNT FROM (" +
                " SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME , B_PERIOD BILLMONTH , SLAB_ID, SLAB_NAME,  " +
                         " Sum(CONSUMERS) CONSUMERS, Sum(AMOUNT) AMOUNT " +
                         "from VW_DEF_CONS_SUM_AMNT_SLABS " +
                          " WHERE B_PERIOD=(SELECT MAX(B_PERIOD) FROM VW_DEF_CONS_SUM_AMNT_SLABS)";

            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + code + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " AND SDIVCODE = '" + pCode + "'";
            }

            if (!string.IsNullOrEmpty(pType))
            {
                sql += " AND DEF_TYPE = '" + pType + "'";
            }

            if (!string.IsNullOrEmpty(pStatus))
            {
                sql += " AND DEF_STATUS = '" + pStatus + "'";

            }

            if (!string.IsNullOrEmpty(pTariff))
            {
                sql += " AND TARIFF_CAT = '" + pTariff + "'";

            }

            sql += " GROUP BY " + groupBy;
            sql += " ORDER BY SRT_ORDER1, SLAB_ID)";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCode">office code</param>
        /// <param name="pBillMon">billing month</param>
        /// <param name="pBatchFrom">starting batch no.</param>
        /// <param name="pBatchTo">ending batch no.</param>
        /// <param name="pUnit">with & without units(0), with unit(1), without units(2) </param>
        /// <returns></returns>
        public DataTable GetCRAdjustments(string pCode, DateTime pBillMon, string pBatchFrom,  char pUnit)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);
            string fromBatch = pBatchFrom;
            string toBatch = pBatchFrom;

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sortorder = "SRT_ORDER1";

            string sql = @"SELECT rownum SrNO, SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME, BATCH, REF_NO, NAME CONS_NAME, ADJ_NO, RO_ADJ_DATE, " +
                         "MAIN_DATE, POSTING_DATE, B_PERIOD BILLMONTH, UNITS_ADJ, AMOUNT_ADJ " +
                         "from VW_CR_ADJMS ";

            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + code + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " WHERE SDIVCODE = '" + pCode + "'";
            }
            if (pBatchFrom.IndexOf("-") > 0)
            {
                fromBatch = pBatchFrom.Split('-')[0];
                toBatch = pBatchFrom.Split('-')[1];
            }

            if (!string.IsNullOrEmpty(fromBatch) && !string.IsNullOrEmpty(toBatch))
            {
                sql += " AND BATCH BETWEEN '" + fromBatch + "' and '" + toBatch + "'";
            }

            if (pUnit!=' ' && pUnit!='0')
            {
                if(pUnit=='1')
                {
                    sql += " AND nvl(UNITS_ADJ,0) != 0 ";
                }
                else
                {
                    sql += " AND nvl(UNITS_ADJ,0) = 0 ";

                }
            }

           
            sql += " ORDER BY SRT_ORDER1";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }
        //Credit Adj. Details Centerwise
        public DataTable GetCRAdjustmentsCentreWise(string pCode, DateTime pBillMon, string pBatchFrom, char pUnit)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);
            string fromBatch = pBatchFrom;
            string toBatch=pBatchFrom;

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sortorder = "SRT_ORDER1";
            switch (pCode.Length)
            {
                case 2:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }
                case 3:
                    {
                        sortorder = "IN('2','3')";
                        break;
                    }
                case 4:
                    {
                        sortorder = "IN('1','2')";
                        break;
                    }
                default:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }


            }

            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME, count(REF_NO) TOTREFFNOS,  " +
                         " B_PERIOD BILLMONTH, sum(UNITS_ADJ) UNITS, SUM(AMOUNT_ADJ) AMOUNT" +
                         " from VW_CR_ADJMS ";
            
            if (pBatchFrom.IndexOf("-") > 0)
            {
                fromBatch= pBatchFrom.Split('-')[0];
                toBatch = pBatchFrom.Split('-')[1];
            }

            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
            }

            if (!string.IsNullOrEmpty(fromBatch) && !string.IsNullOrEmpty(toBatch))
            {
                sql += " AND BATCH BETWEEN '" + fromBatch + "' and '" + toBatch + "'";
            }

            if (pUnit != ' ' && pUnit != '0')
            {
                if (pUnit == '1')
                {
                    sql += " AND nvl(UNITS_ADJ,0) != 0 ";
                }
                else
                {
                    sql += " AND nvl(UNITS_ADJ,0) = 0 ";

                }
            }

            sql += " GROUP BY SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIV_NAME, B_PERIOD ";
            sql += " ORDER BY SRT_ORDER1";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="billMon"></param>
        /// <param name="age"></param>
        /// <param name="trf"></param>
        /// <returns></returns>
        public DataTable GetDefectiveMeterRegionWise(string code, DateTime billMon, string age, string trf)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string fromAge = age;
            string toAge = age;
            string sortorder = "";

            switch (code.Length)
            {
                case 2:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }
                case 3:
                    {
                        sortorder = "IN('2','3')";
                        break;
                    }
                case 4:
                    {
                        sortorder = "IN('1','2')";
                        break;
                    }
                default:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }
            }

            if (age.IndexOf("-") > 0)
            {
                fromAge = age.Split('-')[0];
                toAge = age.Split('-')[1];
            }

            string sql = "SELECT CODE, NAME, BILL_MONTH, SUM(TotSinglePhase) TotSinglePhase, SUM(TotThreePhase) TotThreePhase, SUM(TotDefCons) TotDefCons " +
                            "FROM ( "+
                            "select SDIVCODE CODE, SDIV_NAME NAME, BILL_MONTH, " +
                            "case METER_TYPE when 'SINGLE PHASE' then 1 "+
                            "    else 0 end TotSinglePhase, "+
                "case METER_TYPE when 'THREE PHASE' then 1 "+
                 "   else 0 end TotThreePhase, "+
                "1 TotDefCons " +
                         "from VW_DEFECTIVE_METERS_PROG ";

            if (!string.IsNullOrEmpty(code))
            {
                //sql += " WHERE SDIVCODE LIKE '" + code + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " WHERE SDIVCODE LIKE '" + code + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " AND BILL_MONTH = (SELECT MAX(BILL_MONTH) FROM VW_DEFECTIVE_METERS_PROG WHERE SDIVCODE LIKE '" + code + "%'  AND SRT_ORDER2 " + sortorder + ")";
            }

            if (!string.IsNullOrEmpty(age))
            {
                sql += " AND DEFECT_AGE between " + fromAge + " and " + toAge;
            }


            if (!string.IsNullOrEmpty(trf))
            {
                sql += " AND TRF_CD = '" + trf + "'";
            }
            sql += ") group by CODE, NAME, BILL_MONTH " +
                " order by CODE desc";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public DataTable GetDefectiveMeterDetails(string code, DateTime billMon, string age, string phase, string trf)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string fromAge = age;
            string toAge = age;

            if(age.IndexOf("-")>0)
            {
                fromAge = age.Split('-')[0];
                toAge = age.Split('-')[1];
            }
            
            string sql = @"select ROWNUM SRNO, SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME, METER_TYPE PHASE, TARIFF_CAT CATEGORY, CKEY REFNO, NAME_ADDRESS, TRF_CD, SAN_LOAD, CAT, DEFECT_AGE, STATUS, BILL_MONTH " +
                         "from VW_DEFECTIVE_METERS_PROG ";

            if (!string.IsNullOrEmpty(code))
            {
                //sql += " WHERE SDIVCODE LIKE '" + code + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " WHERE SDIVCODE = '" + code + "'";
                sql += " AND BILL_MONTH = (SELECT MAX(BILL_MONTH) FROM VW_DEFECTIVE_METERS_PROG WHERE SDIVCODE = '" + code + "')";
            }

            if (!string.IsNullOrEmpty(age))
            {
                sql += " AND DEFECT_AGE between " + fromAge + " and " + toAge;
            }

            if (!string.IsNullOrEmpty(phase))
            {
                sql += " AND METER_TYPE = '" + phase + "'";
            }

            if (!string.IsNullOrEmpty(trf))
            {
                sql += " AND TRF_CD = '" + trf + "'";
            }
            sql += " ORDER BY SRT_ORDER1";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public DataTable GetDefectMeterSumMonWise(string code, DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sortorder = "SRT_ORDER1";

            switch (code.Length)
            {
                case 2:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
                case 3:
                {
                    sortorder = "IN('2','3')";
                    break;
                }
                case 4:
                {
                    sortorder = "IN('1','2')";
                    break;
                }
                default:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
            }

            string sql = @"SELECT ROWNUM SRNO, SRT_ORDER2, SRT_ORDER1, BILLMONTH, SDIVCODE CODE, SDIVNAME NAME, ONE_MONTH, TWO_TO_3_MONTH, FOUR_TO_6_MONTH, MORE_THEN_6_MONTH " +
                         "FROM VW_DEFECTIVE_METER_SUM ";

            if (!string.IsNullOrEmpty(code))
            {
                sql += " WHERE SDIVCODE LIKE '" + code + "%' AND SRT_ORDER2 " + sortorder
                       + " ORDER BY SRT_ORDER1";
            }
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public DataTable GetDefectMeterSumTrfWise(string code, DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sortorder = "SRT_ORDER1";

            switch (code.Length)
            {
                case 2:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
                case 3:
                {
                    sortorder = "IN('2','3')";
                    break;
                }
                case 4:
                {
                    sortorder = "IN('1','2')";
                    break;
                }
                default:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
            }

            string sql = @"SELECT ROWNUM SRNO, SRT_ORDER2, SRT_ORDER1, BILLMONTH, SDIVCODE CODE, SDIVNAME NAME, DOMESTIC, COMMERCIAL, INDUSTRIAL, AGRICULTURE, OTHERS, TOTAL " +
                         "FROM VW_DEFECTIVE_METER_TRFWISE ";

            if (!string.IsNullOrEmpty(code))
            {
                sql += " WHERE SDIVCODE LIKE  '" + code + "%' AND SRT_ORDER2 " + sortorder
                       + " ORDER BY SRT_ORDER1";
            }
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public DataTable GetExtraHeaveyBill(string code, DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sortorder = "SRT_ORDER1";



            string sql = @"SELECT B_PERIOD BillingMonth, rownum SrNo, BMONTH, SRT_ORDER2,SRT_ORDER1,SDIVCODE CODE, SDIV_NAME NAME, REFNO, TRF, SLOAD, UNITS,AMOUNT "
                            + " FROM VE_EXTRA_HEAVY_BILLS ";
            if (!string.IsNullOrEmpty(code))
            {
                sql += " WHERE SDIVCODE = '" + code + "'" //" AND SRT_ORDER2 " + sortorder
                    ////+ " AND BILLMONTH='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'" 
                       + " AND B_PERIOD=(SELECT MAX(B_PERIOD) FROM VE_EXTRA_HEAVY_BILLS WHERE SDIVCODE = '" + code + "')"
                       + " ORDER BY SRT_ORDER1";
            }
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }
     
        public DataTable GetExtraHeaveyBillRegion(string code, DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sortorder = "SRT_ORDER1";

            switch (code.Length)
            {
                case 2:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
                case 3:
                {
                    sortorder = "IN('2','3')";
                    break;
                }
                case 4:
                {
                    sortorder = "IN('1','2')";
                    break;
                }
                default:
                {
                    sortorder = "IN('3','5')";
                    break;
                }

            }
            string sql = @"SELECT B_PERIOD BillingMonth, SDIVCODE CODE, SDIV_NAME NAME, sum(UNITS) UNITS, sum(AMOUNT) AMOUNT"
                            + " FROM VE_EXTRA_HEAVY_BILLS ";
            if (!string.IsNullOrEmpty(code))
            {
                sql += " WHERE SDIVCODE LIKE '" + code + "%'  AND SRT_ORDER2 " + sortorder
                    + " AND B_PERIOD=(SELECT MAX(B_PERIOD) FROM VE_EXTRA_HEAVY_BILLS WHERE SDIVCODE like '" + code + "%'  AND SRT_ORDER2 " + sortorder + ")"
                    ////+ " AND BILLMONTH='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'"
                    + " GROUP BY B_PERIOD , SDIVCODE , SDIV_NAME"
                       + " ORDER BY SDIVCODE";
            }
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);

            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }
        public DataTable GetCashCollSummary(string code, DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sortorder = "SRT_ORDER1";

            switch (code.Length)
            {
                case 2:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }
                case 3:
                    {
                        sortorder = "IN('2','3')";
                        break;
                    }
                case 4:
                    {
                        sortorder = "IN('1','2')";
                        break;
                    }
                default:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }


            }

            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, BILLMONTH, MAINDATE, MAINDATEC, sdiv_code CODE, sdiv_name NAME, DAILY_STUBS, ONLINE_STUBS, NORMAL_CASH_COLLECTED, ONLINE_CASH_COLLECTED, NORMAL_CASH_POSTED, ONLINE_CASH_POSTED, TOTAL_CASH_POSTED, RCO_FEE, ADV_CASH, UNIDENTIFIED_CASH, P_DISC_PAYMENT, GOVT_PAYMENT, TUBEWELL_PAYMENT
                                    FROM VW_CASH_COLL_SUMMARY";
            if (!string.IsNullOrEmpty(code))
            {
                sql += " WHERE sdiv_code LIKE '" + code + "%' AND SRT_ORDER2 " + sortorder
                       //+ " AND BILLMONTH='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'" 
                       + " AND BILLMONTH=(SELECT MAX(BILLMONTH) FROM VW_CASH_COLL_SUMMARY)"
                       + " ORDER BY SRT_ORDER1";
            }
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
                
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }

            return null;
        }
        public DataTable GetFeederLosses(DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sql =
                @"select ""DIV_CIR"",""DIVNAME"",""0 or below 0"" ZERO_BELOW,""0-10"" ZERO_TEN,""10 20"" TEN_TWENTY,""20 30"" TWENTY_THIRTY,
                            ""30 40"" THIRTY_FOURTY,""40 50"" FOURTY_FIFTY,""Above 50"" ABOVE_FIFTY, ""TOTAL"",""BPERIOD"",""CC_CODE"" 
                            from VW_FEEDER_LINE_LOSS";
            //sql += " WHERE BPERIOD='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'" 
            sql += " WHERE BPERIOD=(SELECT MAX(BPERIOD) FROM VW_FEEDER_LINE_LOSS)"
                   + " ORDER BY DIV_CIR";
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
                
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            return null;
        }
        public DataTable getBillingStatus()
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);//
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            cmd = new OracleCommand(@"SELECT BILLING_MONTH, DIV_NAME, BATCH_DUE, BATCH_RECEIVED, BATCH_BILLED, CC_CODE, RECEIVED_DATE, POSTED_DATE
                                FROM BILLING_STATUS_DIV
                                where BILLING_MONTH=(SELECT MAX(BILLING_MONTH) FROM BILLING_STATUS_DIV)", con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
               
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
             if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            return null;
        }
        public DataTable GetCollVsCompAssmnt(string code)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);//
            string sortorder = "";

            switch (code.Length)
            {
                case 2:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
                case 3:
                {
                    sortorder = "IN('2','3')";
                    break;
                }
                case 4:
                {
                    sortorder = "IN('1','2')";
                    break;
                }
                default:
                {
                    sortorder = "IN('3','5')";
                    break;
                }


            }

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            string sql =
                @"SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIVNAME, PVT_COMP_ASSES, GVT_COMP_ASSES, COMP_ASSES, PVT_COLL, GVT_COLL, TOT_COLL, to_char(B_PERIOD, 'DD-MON-YY') B_PERIOD, CC_CODE, PVT_PERCENT, GVT_PERCENT, TOT_PERCENT
                                      FROM  VW_COLL_VS_COMP_ASS
                                      WHERE B_PERIOD=(SELECT MAX(B_PERIOD) FROM VW_COLL_VS_COMP_ASS)";
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND SDIVCODE LIKE '" + code + "%' AND SRT_ORDER2 " + sortorder;

            }
            sql += " ORDER BY SRT_ORDER1";
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
               
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
             if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            return null;
        }
        //COLLECTION VS BILLING 
        public DataTable GetCollVsBilling(string code)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);//
            string sortorder = "";

            switch (code.Length)
            {
                case 2:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
                case 3:
                {
                    sortorder = "IN('2','3')";
                    break;
                }
                case 4:
                {
                    sortorder = "IN('1','2')";
                    break;
                }
                default:
                {
                    sortorder = "IN('3','5')";
                    break;
                }


            }
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sql =
                @"SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIVNAME, PVT_ASSESS, GVT_ASSESS, TOT_ASSESS, PVT_COLL, GVT_COLL, TOT_COLL, B_PERIOD, CC_CODE, PVT_PERCENT, GVT_PERCENT, TOT_PERCENT
                                      FROM VW_COLL_VS_BILL
                                       WHERE B_PERIOD=(SELECT MAX(B_PERIOD) FROM VW_COLL_VS_BILL)";
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND SDIVCODE LIKE '" + code + "%' AND SRT_ORDER2 " + sortorder;
            }

            sql += " ORDER BY SRT_ORDER1";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
               
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
             if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            return null;
        }
        public DataTable getReceiveables(string code, DateTime billMon)
        {
            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, sdiv_code CODE, sdiv_name NAME, PVT_REC, GVT_REC, TOT_REC, PVT_SPILL, 
                                      GVT_SPILL, TOT_SPILL, PVT_ARREAR, GVT_ARREAR, TOT_ARREAR, CC_CODE, B_PERIOD
                                      FROM VW_RECIEVABLES";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            string sortorder = "";

            switch (code.Length)
            {
                case 2:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
                case 3:
                {
                    sortorder = "IN('2','3')";
                    break;
                }
                case 4:
                {
                    sortorder = "IN('1','2')";
                    break;
                }
                default:
                {
                    sortorder = "IN('3','5')";
                    break;
                }


            }
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " WHERE SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND sdiv_code LIKE '" + code + "%'";
            }

            //sql += " AND B_PERIOD='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'";
            sql += " AND B_PERIOD=(SELECT MAX(B_PERIOD) FROM VW_RECIEVABLES)";
            sql += " ORDER BY SRT_ORDER1";

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
               
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
             if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            return null;
        }
        public DataTable getMonLosses(string code, DateTime billMon)
        {
            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, SDIV, SDIVNAME, to_char(b_period, 'DD-MON-YY') B_PERIOD, RCV, BIL, LOS, PCT, to_char(PRV_PERIOD, 'DD-MON-YY') PRV_PERIOD, PRV_RCV, PRV_BIL, PRV_LOS, PRV_PCT, VAR_INCDEC, ATC
                           FROM VW_MONTHLY_LINE_LOSS";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            string sortorder = "";

            switch (code.Length)
            {
                case 2:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
                case 3:
                {
                    sortorder = "IN('2','3')";
                    break;
                }
                case 4:
                {
                    sortorder = "IN('1','2')";
                    break;
                }
                default:
                {
                    sortorder = "IN('3','5')";
                    break;
                }


            }
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " WHERE SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND SDIV LIKE '" + code + "%'";
            }
            sql += " AND B_PERIOD=(SELECT MAX(B_PERIOD) FROM VW_MONTHLY_LINE_LOSS)";
            //sql += " AND B_PERIOD='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'";
            sql += " ORDER BY SRT_ORDER1";
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
               
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
             if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            return null;
        }
        public DataTable getPrgsLosses(string code, DateTime billMon)
        {
            string sql = @"SELECT 0 ATC , SRT_ORDER2, SRT_ORDER1, SDIV, SDIVNAME, B_PERIOD, RCV, BIL, LOS, PCT, PRV_PERIOD, PRV_RCV, PRV_BIL, PRV_LOS, PRV_PCT, VAR_INCDEC
                            FROM VW_PROG_LINE_LOSSES";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            string sortorder = "";

            switch (code.Length)
            {
                case 2:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
                case 3:
                {
                    sortorder = "IN('2','3')";
                    break;
                }
                case 4:
                {
                    sortorder = "IN('1','2')";
                    break;
                }
                default:
                {
                    sortorder = "IN('3','5')";
                    break;
                }


            }
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " WHERE SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND SDIV LIKE '" + code + "%'";
            }

            //sql += " AND B_PERIOD='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'";
            sql += " ORDER BY SRT_ORDER1";
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
               
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
             if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            return null;
        }
        public DataTable getBillingStatsBatchWise(string code, DateTime billMon)
        {
            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, MONTH, BATCH, SDIV_CODE CODE, SDIV_NAME NAME, TNOCONSUMERS, NOUNBILLEDCASES, NOSTSREADING, NODISCASES, NORECCASES, NOMCOCASES, NODEFMETERS, LOCKCASES, NONEWCONN, CREDBALCONSM, NOHEAVYBCASES, CREDBALAMT "
                         + " from VW_BILLING_STATS_BATCHWISE "
                         + " where MONTH = (select max(MONTH) from VW_BILLING_STATS_BATCHWISE)";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            string sortorder = "";

            switch (code.Length)
            {
                case 2:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
                case 3:
                {
                    sortorder = "IN('2','3')";
                    break;
                }
                case 4:
                {
                    sortorder = "IN('1','2')";
                    break;
                }
                default:
                {
                    sortorder = "IN('3','5')";
                    break;
                }


            }
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " AND SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND SDIV_CODE LIKE '" + code + "%'";
            }

            //sql += " AND B_PERIOD='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'";
            sql += " ORDER BY SRT_ORDER1";
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
               
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
             if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            return null;
        }

        public DataTable getAssesmentBatchWise(string code, DateTime billMon)
        {
            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, MONTH, BATCH, SDIV_CODE, SDIV_NAME, NOBILLSISSUED, OPB, CURASSESS, GOVTASSESS, NET, UNITBILLED, RURALUNITBILLED, URBANUNITBILLED, NOADJUSTM, UNITADJ, AMTADJ, NODETADJ, DETADJUNITS, DETADJAMT,"
                         +" ASSESS_DOM, ASSESS_COM, ASSESS_IND, ASSESS_AGRI"
                         + " from VW_ASSESMENT_BATCHWISE "
                         + " where MONTH = (select max(MONTH) from VW_ASSESMENT_BATCHWISE)";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            string sortorder = "";

            switch (code.Length)
            {
                case 2:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }
                case 3:
                    {
                        sortorder = "IN('2','3')";
                        break;
                    }
                case 4:
                    {
                        sortorder = "IN('1','2')";
                        break;
                    }
                default:
                    {
                        sortorder = "IN('3','5')";
                        break;
                    }


            }
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " AND SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND SDIV_CODE LIKE '" + code + "%'";
            }

            //sql += " AND B_PERIOD='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'";
            sql += " ORDER BY SRT_ORDER1";
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
                
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            return null;
        }

        public DataTable getBillingStatsDaily(string code, DateTime billMon)
        {
            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, MONTH, SDIV_CODE CODE, SDIV_NAME NAME, TNOCONSUMERS, NOUNBILLEDCASES, NOSTSREADING, NODISCASES, NORECCASES, NOMCOCASES, NODEFMETERS, LOCKCASES, NONEWCONN, CREDBALCONSM, NOHEAVYBCASES, CREDBALAMT "
                         + " from VW_BILLING_STATS_DAILY "
                         + " where MONTH = (select max(MONTH) from VW_BILLING_STATS_DAILY)";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(_constr);

            string sortorder = "";

            switch (code.Length)
            {
                case 2:
                {
                    sortorder = "IN('3','5')";
                    break;
                }
                case 3:
                {
                    sortorder = "IN('2','3')";
                    break;
                }
                case 4:
                {
                    sortorder = "IN('1','2')";
                    break;
                }
                default:
                {
                    sortorder = "IN('3','5')";
                    break;
                }


            }
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " AND SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND SDIV_CODE LIKE '" + code + "%'";
            }

            //sql += " AND B_PERIOD='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'";
            sql += " ORDER BY SRT_ORDER1";
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
               
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
             if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            return null;
        }

        public DataTable getTheftData(string refNo)
        {
            string sql = @"select BILL_MNTH, NOTE_NO, ADJ_DT, UNITS, AMOUNT,PAY_AGAINTS_DET"
                         + " from vw_theft_dt_portal "
                         //+ " where BATCH||SDIV|| CONS_NO ='" + refNo + "'";
                         + " WHERE  APPLICATION_NO = (SELECT GET_APP_BY_REF_CUR_MONTH('" + refNo + "') FROM DUAL)";
            OracleConnection con = null;
            OracleCommand cmd;
            string mndConStr = System.Configuration.ConfigurationManager.ConnectionStrings["MND_CONSTR"].ToString();
            con = new OracleConnection(mndConStr);

            
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
           
            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(ds);
               
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
             if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            return null;
        }


        public DataTable getBillData(string ts, string kwh, DateTime strtPeriod, DateTime endPeriod, string trf)
        {
            string startMonth = strtPeriod.ToString("dd") + "-" + strtPeriod.ToString("MMM") + "-" +
                                strtPeriod.ToString("yyyy");
            string endMonth = endPeriod.ToString("dd") + "-" + endPeriod.ToString("MMM") + "-" +
                              endPeriod.ToString("yyyy");
            string sql = "PROC_BILL4API";
            OracleConnection con = null;
            string cisConStr = System.Configuration.ConfigurationManager.ConnectionStrings["MEPCO2.6"].ToString();
            con = new OracleConnection(cisConStr);
            DataTable dt = new DataTable();

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

               OracleCommand cmd =
                    new OracleCommand(sql, con);

                OracleParameter opTS = new OracleParameter("TS", OracleDbType.Varchar2);
                opTS.Value = ts;
                cmd.Parameters.Add(opTS);

                OracleParameter opKWH = new OracleParameter("KWH", OracleDbType.Long);
                opKWH.Value = Int32.Parse(kwh);
                cmd.Parameters.Add(opKWH);
                OracleParameter opSperiod = new OracleParameter("S_PERIOD", OracleDbType.Date);
                opSperiod.Value = startMonth.ToUpper();
                cmd.Parameters.Add(opSperiod);

                OracleParameter opEperiod = new OracleParameter("E_PERIOD", OracleDbType.Date);
                opEperiod.Value = endMonth.ToUpper();
                cmd.Parameters.Add(opEperiod);

                OracleParameter opTRF = new OracleParameter("TRF", OracleDbType.Varchar2);
                opTRF.Value = trf;
                cmd.Parameters.Add(opTRF);
                OracleParameter opResult = new OracleParameter("RESULT", OracleDbType.RefCursor);
                opResult.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(opResult);

                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(dt);
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                return dtErr;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return dt;
        }
    }
}   