
//using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

/// <summary>
/// Summary description for DB_Utility
/// </summary>

namespace DAL
{
    public class DB_Utility
    {
        public string GetConnStr(string pCode)
        {
            string conStrName=pCode;
            if(pCode.Length>1)
                conStrName= pCode.Substring(0,2);

            return System.Configuration.ConfigurationManager.ConnectionStrings[conStrName].ToString();
        }
        
        public DB_Utility()
        {
        }

        public DataSet GetDepWiseInfo(string pCode, string bmonth)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            string query = "";
            query = "SELECT SDIV_NAME, SDIV_CODE FROM VW_GVT_COL_BILLING WHERE SDIV_CODE = '" + pCode + "' AND ROWNUM = 1";
            string DeptTbl = "DeptTable";
            DataSet records = new DataSet();
            records.Tables.Add("CenterInfo");
            OracleDataAdapter ad = new OracleDataAdapter();
            cmd = new OracleCommand(query, con);
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(records, "CenterInfo");
            }

            catch (Exception ex)
            {
                return null;
            }
            query = "SELECT B_PERIOD, SDIV_CODE, SDIV_NAME, TOT_ASSESS, NO_OF_CON, OP_BAL, DEPTTYPEDESC, PAYMENT, CL_BAL, 0 AS DEPTCODE " +
                    "FROM VW_GVT_COL_BILLING WHERE B_PERIOD LIKE '%" + bmonth + "%' AND SDIV_CODE LIKE '" + pCode + "%'";
            cmd = new OracleCommand(query, con);
            cmd.CommandType = CommandType.Text;

            ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                records.Tables.Add(DeptTbl);
                ad.Fill(records, DeptTbl);
            }
            catch (Exception ex)
            {
                DataTable dtErr = new DataTable();
                dtErr.Columns.Add("Desc");
                DataRow drErr = dtErr.NewRow();
                drErr["Desc"] = ex.ToString();
                dtErr.Rows.Add(drErr);
                records.Tables.Add(dtErr);
                return records;
            }

            return records;

        }
        public DataSet GetCenterWiseDeptInfo(string pCode, string bmonth)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            
            string likecode = "";
            string query = "";
            query = "SELECT SDIV_NAME, SDIV_CODE FROM VW_GVT_COL_BILLING WHERE SDIV_CODE = '" + pCode + "' AND ROWNUM = 1"; 
            string center1 = "centersrno";
            DataSet records = new DataSet();
            records.Tables.Add("CenterInfo");
            OracleDataAdapter ad = new OracleDataAdapter();
            cmd = new OracleCommand(query, con);
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(records, "CenterInfo");
            }

            catch(Exception ex) {
                return null;
            }
            ad.SelectCommand = cmd;

            for (int i = 0; i <= 9; i++)
            {
                likecode = pCode + i.ToString();
                query = "SELECT B_PERIOD, SDIV_CODE, SDIV_NAME, TOT_ASSESS, NO_OF_CON, OP_BAL, DEPTTYPEDESC, PAYMENT, CL_BAL, 0 AS DEPTCODE " +
                    "FROM VW_GVT_COL_BILLING WHERE B_PERIOD LIKE '%" + bmonth + "%' AND SDIV_CODE LIKE '" + likecode + "%'";
                likecode = "";
                cmd = new OracleCommand(query, con);
                cmd.CommandType = CommandType.Text;

                ad = new OracleDataAdapter();
                ad.SelectCommand = cmd;
                try
                {
                    records.Tables.Add((center1 + i).ToString());
                    ad.Fill(records, (center1 + i).ToString());
                }
                catch (Exception ex)
                {
                    DataTable dtErr = new DataTable();
                    dtErr.Columns.Add("Desc");
                    DataRow drErr = dtErr.NewRow();
                    drErr["Desc"] = ex.ToString();
                    dtErr.Rows.Add(drErr);
                    records.Tables.Add(dtErr);
                    return records;
                }
            }

            return records;
        }


        //VW_DEF_CONS_SUM_FDRCODE_WISE
        public DataSet GetTariffWiseBill(string pCode, string bmonth)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            DataSet records = new DataSet();
            string likecode = "";
            string query1 = "";
            string query2 = "";
            string center1 = "commercial";
            string center2 = "domestic";
            string query = "SELECT SDIV_NAME, SDIV_CODE FROM VW_TRF_WISE_UNITS WHERE SDIV_CODE = '" + pCode + "' AND ROWNUM = 1";
            records.Tables.Add("CenterInfo");
            OracleDataAdapter ad = new OracleDataAdapter();
            cmd = new OracleCommand(query, con);
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(records, "CenterInfo");
            }

            catch (Exception ex)
            {
                return null;
            }
            
            for (int i = 0; i <= 9; i++)
            {
                likecode = pCode + i.ToString();

                query1 = "SELECT B_PERIOD S, SDIV_CODE, SDIV_NAME, TARIFF_CATEGORY, CATEGORY_NAME, CONNECTIONS, UNITS, BILLING, PAYMENT, CLOSING, SPILLOVER, TOT_PERCENT" +
                " FROM VW_TRF_WISE_UNITS WHERE to_char(B_PERIOD,'MON') || '-' || to_char(B_PERIOD,'YY') = " + " '" +  bmonth.ToUpper() + "'" + " AND SDIV_CODE LIKE '" + likecode + "%" + "' AND TARIFF_CATEGORY IN " +
                "(SELECT TRFCD FROM DISCO.tbltariff where category='D')";

                query2 = "SELECT B_PERIOD S, SDIV_CODE, SDIV_NAME, TARIFF_CATEGORY, CATEGORY_NAME, CONNECTIONS, UNITS, BILLING, PAYMENT, CLOSING, SPILLOVER, TOT_PERCENT" +
                " FROM VW_TRF_WISE_UNITS WHERE to_char(B_PERIOD,'MON') || '-' || to_char(B_PERIOD,'YY') = " + " '" +  bmonth.ToUpper() + "' AND SDIV_CODE LIKE '" + likecode + "%" + "' AND TARIFF_CATEGORY IN " +
                "(SELECT TRFCD FROM DISCO.tbltariff where category='C')";

                likecode = "";
                cmd = new OracleCommand(query1, con);
                cmd.CommandType = CommandType.Text;

                ad = new OracleDataAdapter();
                ad.SelectCommand = cmd;
                try
                {
                    records.Tables.Add((center1 + i).ToString());
                    ad.Fill(records, (center1 + i).ToString());
                }
                catch (Exception ex)
                {
                    DataTable dtErr = new DataTable();
                    dtErr.Columns.Add("Desc");
                    DataRow drErr = dtErr.NewRow();
                    drErr["Desc"] = ex.ToString();
                    dtErr.Rows.Add(drErr);
                    records.Tables.Add(dtErr);
                    return records;
                }
                cmd = new OracleCommand(query2, con);
                cmd.CommandType = CommandType.Text;
                ad.SelectCommand = cmd;
                try
                {
                    records.Tables.Add((center2 + i).ToString());
                    ad.Fill(records, (center2 + i).ToString());
                }
                catch (Exception ex)
                {
                    DataTable dtErr = new DataTable();
                    dtErr.Columns.Add("Desc");
                    DataRow drErr = dtErr.NewRow();
                    drErr["Desc"] = ex.ToString();
                    dtErr.Rows.Add(drErr);
                    records.Tables.Add(dtErr);
                    return records;
                }
            }

            return records;
        }
        public DataTable GetRefWiseTentative(string pCode, string ason)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string query = "SELECT ROWNUM AS SRNO, RGNCODE, RGNNAME, B_PERIOD, REF_NO, TARIFF_ACTIVE, CONSUMER_NAME, " +
                "(ADDR_1||''||ADDR_2) AS ADDRESS, 0 AS ER0NO, 0 AS ERODATE, DFL_UNPAID_AGE AS AGE, UNPAID_AMOUNT, 0 AS DEFERREDAMOUNT, DFL_OWNING_AMNT, CL_TOT_AMNT" +
                " FROM VW_114_TENT_DSH WHERE B_PERIOD = (SELECT MAX(B_PERIOD) FROM VW_114_TENT_DSH) AND SDIVCODE LIKE '" + pCode + "%' AND DFL_ISU_DT <= '" + ason + "'";
            cmd = new OracleCommand(query, con);
            cmd.CommandType = CommandType.Text;
            DataSet records = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(records);
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

            if (records.Tables[0].Rows.Count > 0)
            {
                return records.Tables[0];
            }
            return null;
        }
        public DataTable GetRegWiseDefaulters(string pCode, string asOn)
        {
            string feederName = string.Empty;
            OracleConnection con = null;
            //DateTime asOnDate = Convert.ToDateTime(asOn);
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            //OracleParameter todate = new OracleParameter("@ason", OracleDbType.Varchar2);
            //todate.Value = asOnDate;
            string query = "SELECT SDIVCODE AS CenterCode, RGNCODE, SDIVNAME as CenterName, RGNNAME,B_PERIOD , SUM(UNPAID_AMOUNT)" +
                " AS UnpaidAmount, SUM(0) AS DefferedAmount, SUM(DFL_OWNING_AMNT) AS OwningAmount, SUM(CL_TOT_AMNT) AS ClosingAmount " +
                "FROM VW_114_TENT_DSH WHERE B_PERIOD = (SELECT MAX(B_PERIOD) FROM VW_114_TENT_DSH) AND SDIVCODE LIKE '" + pCode + "%'" +
                " AND DFL_ISU_DT <= '" + asOn + "' GROUP BY SDIVCODE, SDIVNAME, RGNNAME, B_PERIOD, RGNCODE";


            cmd = new OracleCommand(query, con);
            cmd.CommandType = CommandType.Text;
            DataSet records = new DataSet();
            OracleDataAdapter ad = new OracleDataAdapter();
            ad.SelectCommand = cmd;
            try
            {
                ad.Fill(records);
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

            if (records.Tables[0].Rows.Count > 0)
            {
                return records.Tables[0];
            }
            return null;
        }
        public string GetFeederNameByCode(string pCode, string pFdrCode)
        {
            string feederName = string.Empty;
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sql = @"select FDRNAME  " +
                         "from VW_DEF_CONS_SUM_FDRCODE_WISE " +
                         "WHERE rownum=1 ";

            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " AND SDIVCODE = '" + pCode + "'";
            }

            if (!string.IsNullOrEmpty(pFdrCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " AND FDRCODE = '" + pFdrCode + "'";
            }

            cmd = new OracleCommand(sql, con);
            cmd.CommandType = CommandType.Text;

            try
            {
                feederName = cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                feederName = ex.ToString();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return feederName;
        }
        public DataTable GetDefConsFdrCdWise(string pCode, DateTime pBillMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

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
                //sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
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
        public DataTable GetDefListRefWise(string pCode, DateTime pBillMon, string pType, string pStatus, string pTariff, string pSlab, char flagAgeAmnt)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sql = @" SELECT NULL SLAB, ROWNUM SrNO, SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME, BPERIOD BILLMONTH, MAINDT, REFNO, TARRIFCODE, DEF_TYPE, DEF_STATUS,  " +
                         "TARIFF_CAT, NAMEADD, DCN_NO, DCN_DATE, MTR_NO, AGE, AMOUNT " +
                         "FROM VW_DEFAULTER_LIST " +
                         "WHERE 1=1 ";
            //" WHERE BPERIOD=(SELECT MAX(BPERIOD) FROM VW_DEFAULTER_LIST)";

            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
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

            if (!string.IsNullOrEmpty(pSlab))
            {
                //string slabFrom = pSlab.Substring(0, pSlab.IndexOf("--------------"));
                //string slabTo = pSlab.Substring(pSlab.LastIndexOf("--------------") + "--------------".Length);
                if (flagAgeAmnt == 'A')
                {
                    sql += " AND AGE_SLAB = " + pSlab;
                    //sql += " AND AGE between " + slabFrom + " and " + slabTo;
                }
                else
                {
                    sql += " AND AMOUNT_SLAB = " + pSlab;
                    //sql += " AND AMOUNT between " + slabFrom + " and " + slabTo;
                }
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

        //FDR CD wise
        public DataTable GetDefListFeederWise(string pCode, DateTime pBillMon, string fdrCD)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sql = @" SELECT  ROWNUM SrNO, SDIVCODE CODE, SDIV_NAME NAME, BPERIOD BILLMONTH, REFNO, TARRIFCODE, DEF_STATUS STATUS,  " +
                         "NAMEADD, DCN_NO, DCN_DATE, MTR_NO, AGE, AMOUNT, SDIV_NAME FEEDER_NAME " +
                         "FROM VW_DEFAULTER_LIST " +
                         "WHERE 1=1";
            //" WHERE BPERIOD=(SELECT MAX(BPERIOD) FROM VW_DEFAULTER_LIST)";

            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " AND SDIVCODE = '" + pCode + "'";
            }

            if (!string.IsNullOrEmpty(fdrCD))
            {
                sql += " AND FEEDER_CD = '" + fdrCD + "'";
            }

            sql += " ORDER BY SDIVCODE, REFNO";

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
            con = new OracleConnection(GetConnStr(pCode));
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
                //sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
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
            con = new OracleConnection(GetConnStr(pCode));
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

            switch (pCode.Length)
            {
                case 2:
                    {
                        //sortorder = "IN('3','5')";
                        sortorder = "'" + pCode + "_11%'";
                        break;
                    }
                case 3:
                    {
                        //sortorder = "IN('2','3')";
                        sortorder = "'" + pCode + "_1%'";
                        break;
                    }
                case 4:
                    {
                        sortorder = "'" + pCode + "_%'";
                        break;
                    }
                default:
                    {
                        //sortorder = "IN('3','5')";
                        sortorder = "'" + pCode + "_11%'";
                        break;
                    }


            }

            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME, BPERIOD BILLMONTH, count(refno) CONSUMERS, sum(AMOUNT) AMOUNT  " +
                         "FROM VW_DEFAULTER_LIST " +
                         " WHERE 1=1 ";
            //sql += " AND SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder + filter;
            sql += " AND SDIVCODE LIKE " + sortorder + filter;
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
            con = new OracleConnection(GetConnStr(pCode));
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
                         " WHERE 1=1 ";
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
            con = new OracleConnection(GetConnStr(pCode));
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

            string sql = @"SELECT SDIVCODE CODE, SDIV_NAME NAME, BILLMONTH BILLMONTH, SUM(TOT_CONS) CONSUMERS, SUM(AMOUNT) AMOUNT  " +
                         "FROM VW_DEF_CONS_SUM_BATCH_WISE " +
                         " WHERE 1=1 ";
            sql += " AND SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder + filter;
            sql += " GROUP BY SDIVCODE, SDIV_NAME, B_PERIOD";

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
        public DataTable GetDefConsSumBatch(string pCode, DateTime pBillMon, string pAge, string pPvtGvt, string pRundisc, string pTrf)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));
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
                         " WHERE 1=1 ";
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
            con = new OracleConnection(GetConnStr(pCode));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string groupBy = " SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIV_NAME, B_PERIOD, SLAB_ID, SLAB_NAME ";
            string sql = @" SELECT ROWNUM SrNO, SRT_ORDER2, SRT_ORDER1, CODE, NAME , BILLMONTH, SLAB_NAME, SLAB_ID, CONSUMERS, AMOUNT FROM (";
            sql += @" SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME " +
                         ", B_PERIOD BILLMONTH, SLAB_NAME, SLAB_ID, SUM(CONSUMERS) CONSUMERS, SUM(AMOUNT) AMOUNT " +
                         "from VW_DEF_CONS_SUM_AGE_SLABS " +
            " WHERE 1=1";
            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
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

        /// <summary>
        /// Centre wise
        /// </summary>
        /// <param name="pCode"></param>
        /// <param name="pBillMon"></param>
        /// <param name="pType"></param>
        /// <param name="pStatus"></param>
        /// <param name="pTariff"></param>
        /// <returns></returns>
        public DataTable GetDefSummAgeSlabCentreWise(string pCode, DateTime pBillMon, string pType, string pStatus, string pTariff)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string groupBy = " SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIV_NAME, B_PERIOD ";
            string sql = @" SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME " +
                         ", B_PERIOD BILLMONTH, SUM(CONSUMERS) CONSUMERS, SUM(AMOUNT) AMOUNT " +
                         "from VW_DEF_CONS_SUM_AGE_SLABS " +
                        " WHERE 1=1";

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
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " AND SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
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
        /// Centre wise
        /// </summary>
        /// <param name="pCode"></param>
        /// <param name="pBillMon"></param>
        /// <param name="pType"></param>
        /// <param name="pStatus"></param>
        /// <param name="pTariff"></param>
        /// <returns></returns>
        public DataTable GetDefSummAmntSlabCentreWise(string pCode, DateTime pBillMon, string pType, string pStatus, string pTariff)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string groupBy = " SRT_ORDER2, SRT_ORDER1, SDIVCODE , SDIV_NAME, B_PERIOD ";
            string sql = @" SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME , B_PERIOD BILLMONTH, " +
                         " Sum(CONSUMERS) CONSUMERS, Sum(AMOUNT) AMOUNT " +
                         "from VW_DEF_CONS_SUM_AMNT_SLABS " +
                          " WHERE 1=1 ";

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

            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " and SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
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

        public DataTable GetDefSummAmntSlab(string pCode, DateTime pBillMon, string pType, string pStatus, string pTariff)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string groupBy = " SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIV_NAME, B_PERIOD, SLAB_ID, SLAB_NAME ";
            string sql = @" SELECT ROWNUM SrNO, SRT_ORDER2, SRT_ORDER1, CODE, NAME , BILLMONTH, SLAB_ID, SLAB_NAME, CONSUMERS, AMOUNT FROM (" +
                " SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME , B_PERIOD BILLMONTH , SLAB_ID, SLAB_NAME,  " +
                         " Sum(CONSUMERS) CONSUMERS, Sum(AMOUNT) AMOUNT " +
                         "from VW_DEF_CONS_SUM_AMNT_SLABS " +
                          " WHERE 1=1 ";

            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
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
        public DataTable GetCRAdjustments(string pCode, DateTime pBillMon, string pBatchFrom, char pUnit)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));
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
                //sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
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
            con = new OracleConnection(GetConnStr(pCode));
            string fromBatch = pBatchFrom;
            string toBatch = pBatchFrom;

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
                fromBatch = pBatchFrom.Split('-')[0];
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
        public DataTable GetDefectiveMeterRegionWise(string pCode, DateTime billMon, string age, string trf)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string fromAge = age;
            string toAge = age;
            string sortorder = "";

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

            if (age.IndexOf("-") > 0)
            {
                fromAge = age.Split('-')[0];
                toAge = age.Split('-')[1];
            }

            string sql = "SELECT CODE, NAME, BILL_MONTH, SUM(TotSinglePhase) TotSinglePhase, SUM(TotThreePhase) TotThreePhase, SUM(TotDefCons) TotDefCons " +
                            "FROM ( " +
                            "select SDIVCODE CODE, SDIV_NAME NAME, BILL_MONTH, " +
                            "case METER_TYPE when 'SINGLE PHASE' then 1 " +
                            "    else 0 end TotSinglePhase, " +
                "case METER_TYPE when 'THREE PHASE' then 1 " +
                 "   else 0 end TotThreePhase, " +
                "1 TotDefCons " +
                         "from VW_DEFECTIVE_METERS_PROG ";

            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
                //sql += " AND BILL_MONTH = (SELECT MAX(BILL_MONTH) FROM VW_DEFECTIVE_METERS_PROG WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder + ")";
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

        public DataTable GetDefectiveMeterDetails(string pCode, DateTime billMon, string age, string phase, string trf)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string fromAge = age;
            string toAge = age;

            if (age.IndexOf("-") > 0)
            {
                fromAge = age.Split('-')[0];
                toAge = age.Split('-')[1];
            }

            string sql = @"select ROWNUM SRNO, SRT_ORDER2, SRT_ORDER1, SDIVCODE CODE, SDIV_NAME NAME, METER_TYPE PHASE, TARIFF_CAT CATEGORY, CKEY REFNO, NAME_ADDRESS, TRF_CD, SAN_LOAD, CAT, DEFECT_AGE, STATUS, BILL_MONTH " +
                         "from VW_DEFECTIVE_METERS_PROG ";

            if (!string.IsNullOrEmpty(pCode))
            {
                //sql += " WHERE SDIVpCode LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder;
                sql += " WHERE SDIVCODE = '" + pCode + "'";
                //sql += " AND BILL_MONTH = (SELECT MAX(BILL_MONTH) FROM VW_DEFECTIVE_METERS_PROG WHERE SDIVCODE = '" + pCode + "')";
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

        public DataTable GetDefectMeterSumMonWise(string pCode, DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

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

            string sql = @"SELECT ROWNUM SRNO, SRT_ORDER2, SRT_ORDER1, BILLMONTH, SDIVCODE CODE, SDIVNAME NAME, ONE_MONTH, TWO_TO_3_MONTH, FOUR_TO_6_MONTH, MORE_THEN_6_MONTH " +
                         "FROM VW_DEFECTIVE_METER_SUM ";

            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " WHERE SDIVCODE LIKE '" + pCode + "%' AND SRT_ORDER2 " + sortorder
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

        public DataTable GetDefectMeterSumTrfWise(string pCode, DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

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

            string sql = @"SELECT ROWNUM SRNO, SRT_ORDER2, SRT_ORDER1, BILLMONTH, SDIVCODE CODE, SDIVNAME NAME, DOMESTIC, COMMERCIAL, INDUSTRIAL, AGRICULTURE, OTHERS, TOTAL " +
                         "FROM VW_DEFECTIVE_METER_TRFWISE ";

            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " WHERE SDIVCODE LIKE  '" + pCode + "%' AND SRT_ORDER2 " + sortorder
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

        public DataTable GetExtraHeaveyBill(string pCode, DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sortorder = "SRT_ORDER1";



            string sql = @"SELECT B_PERIOD BillingMonth, rownum SrNo, BMONTH, SRT_ORDER2,SRT_ORDER1,SDIVCODE CODE, SDIV_NAME NAME, REFNO, TRF, SLOAD, UNITS,AMOUNT "
                            + " FROM VE_EXTRA_HEAVY_BILLS ";
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " WHERE SDIVCODE = '" + pCode + "'" //" AND SRT_ORDER2 " + sortorder
                                                          ////+ " AND BILLMONTH='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'" 
                       + " AND B_PERIOD=(SELECT MAX(B_PERIOD) FROM VE_EXTRA_HEAVY_BILLS WHERE SDIVCODE = '" + pCode + "')"
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

        public DataTable GetExtraHeaveyBillRegion(string pCode, DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

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
            string sql = @"SELECT B_PERIOD BillingMonth, SDIVCODE CODE, SDIV_NAME NAME, sum(UNITS) UNITS, sum(AMOUNT) AMOUNT"
                            + " FROM VE_EXTRA_HEAVY_BILLS ";
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " WHERE SDIVCODE LIKE '" + pCode + "%'  AND SRT_ORDER2 " + sortorder
                    + " AND B_PERIOD=(SELECT MAX(B_PERIOD) FROM VE_EXTRA_HEAVY_BILLS WHERE SDIVCODE like '" + pCode + "%'  AND SRT_ORDER2 " + sortorder + ")"
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
        public DataTable GetCashCollSummary(string pCode, DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

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

            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, BILLMONTH, MAINDATE, MAINDATEC, sdiv_code CODE, sdiv_name NAME, DAILY_STUBS, ONLINE_STUBS, NORMAL_CASH_COLLECTED, ONLINE_CASH_COLLECTED, NORMAL_CASH_POSTED, ONLINE_CASH_POSTED, TOTAL_CASH_POSTED, RCO_FEE, ADV_CASH, UNIDENTIFIED_CASH, P_DISC_PAYMENT, GOVT_PAYMENT, TUBEWELL_PAYMENT
                                    FROM VW_CASH_COLL_SUMMARY";
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " WHERE sdiv_code LIKE '" + pCode + "%' AND SRT_ORDER2 " + sortorder
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
        public DataTable GetFeederLosses(string pCode, DateTime billMon)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sql =
                @"select ""DIV_CIR"",""DIVNAME"",""0 or below 0"" ZERO_BELOW,""0-10"" ZERO_TEN,""10 20"" TEN_TWENTY,""20 30"" TWENTY_THIRTY,
                            ""30 40"" THIRTY_FOURTY,""40 50"" FOURTY_FIFTY,""Above 50"" ABOVE_FIFTY, ""TOTAL"",""BPERIOD"",""CC_CODE"" 
                            from VW_FEEDER_LINE_LOSS";
            //sql += " WHERE BPERIOD='01-" + billMon.ToString("MMM") + "-" + billMon.ToString("yyyy") + "'" 
            //BPERIOD=(SELECT MAX(BPERIOD) FROM VW_FEEDER_LINE_LOSS)
            sql += " WHERE 1=1 "
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
        public DataTable getBillingStatus(string pCode)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));
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
        public DataTable GetCollVsCompAssmnt(string pCode)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));//
            string sortorder = "";

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

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            string sql =
                @"SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIVNAME, PVT_COMP_ASSES, GVT_COMP_ASSES, COMP_ASSES, PVT_COLL, GVT_COLL, TOT_COLL, to_char(B_PERIOD, 'DD-MON-YY') B_PERIOD, CC_CODE, PVT_PERCENT, GVT_PERCENT, TOT_PERCENT
                                      FROM  VW_COLL_VS_COMP_ASS
                                      WHERE 1=1 "; //B_PERIOD=(SELECT MAX(B_PERIOD) FROM VW_COLL_VS_COMP_ASS)
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " AND SDIVCODE LIKE '" + pCode + "%' AND SRT_ORDER2 " + sortorder;

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
        public DataTable GetCollVsBilling(string pCode)
        {
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));//
            string sortorder = "";

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
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            string sql =
                @"SELECT SRT_ORDER2, SRT_ORDER1, SDIVCODE, SDIVNAME, PVT_ASSESS, GVT_ASSESS, TOT_ASSESS, PVT_COLL, GVT_COLL, TOT_COLL, B_PERIOD, CC_CODE, PVT_PERCENT, GVT_PERCENT, TOT_PERCENT
                                      FROM VW_COLL_VS_BILL
                                       WHERE B_PERIOD=(SELECT MAX(B_PERIOD) FROM VW_COLL_VS_BILL)";
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " AND SDIVCODE LIKE '" + pCode + "%' AND SRT_ORDER2 " + sortorder;
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
        public DataTable getReceiveables(string pCode, DateTime billMon)
        {
            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, sdiv_code CODE, sdiv_name NAME, PVT_REC, GVT_REC, TOT_REC, PVT_SPILL, 
                                      GVT_SPILL, TOT_SPILL, PVT_ARREAR, GVT_ARREAR, TOT_ARREAR, CC_CODE, B_PERIOD
                                      FROM VW_RECIEVABLES";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            string sortorder = "";

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
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " WHERE SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " AND sdiv_code LIKE '" + pCode + "%'";
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
        public DataTable getMonLosses(string pCode, DateTime billMon)
        {
            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, SDIV, SDIVNAME, to_char(b_period, 'DD-MON-YY') B_PERIOD, RCV, BIL, LOS, PCT, " +
                          " to_char(PRV_PERIOD, 'DD-MON-YY') PRV_PERIOD, PRV_RCV, PRV_BIL, PRV_LOS, PRV_PCT, VAR_INCDEC, ATC, PRV_ATC " +
                           " FROM VW_MONTHLY_LINE_LOSS ";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            string sortorder = "";

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
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " WHERE SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " AND SDIV LIKE '" + pCode + "%'";
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
        public DataTable getPrgsLosses(string pCode, DateTime billMon)
        {
            string sql = @"SELECT 0 ATC , SRT_ORDER2, SRT_ORDER1, SDIV, SDIVNAME, B_PERIOD, RCV, BIL, LOS, PCT, PRV_PERIOD, PRV_RCV, PRV_BIL, PRV_LOS, PRV_PCT, VAR_INCDEC
                            FROM VW_PROG_LINE_LOSSES";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            string sortorder = "";

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
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " WHERE SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " AND SDIV LIKE '" + pCode + "%'";
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
        public DataTable getBillingStatsBatchWise(string pCode, DateTime billMon)
        {
            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, MONTH, BATCH, SDIV_CODE CODE, SDIV_NAME NAME, TNOCONSUMERS, NOUNBILLEDCASES, NOSTSREADING, NODISCASES, NORECCASES, NOMCOCASES, NODEFMETERS, LOCKCASES, NONEWCONN, CREDBALCONSM, NOHEAVYBCASES, CREDBALAMT "
                         + " from VW_BILLING_STATS_BATCHWISE "
                         + " where MONTH = (select max(MONTH) from VW_BILLING_STATS_BATCHWISE)";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            string sortorder = "";

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
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " AND SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " AND SDIV_CODE LIKE '" + pCode + "%'";
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

        public DataTable getAssesmentBatchWise(string pCode, DateTime billMon)
        {
            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, MONTH, BATCH, SDIV_CODE, SDIV_NAME, NOBILLSISSUED, OPB, CURASSESS, GOVTASSESS, NET, UNITBILLED, RURALUNITBILLED, URBANUNITBILLED, NOADJUSTM, UNITADJ, AMTADJ, NODETADJ, DETADJUNITS, DETADJAMT,"
                         + " ASSESS_DOM, ASSESS_COM, ASSESS_IND, ASSESS_AGRI"
                         + " from VW_ASSESMENT_BATCHWISE "
                         + " where MONTH = (select max(MONTH) from VW_ASSESMENT_BATCHWISE)";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            string sortorder = "";

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
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " AND SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " AND SDIV_CODE LIKE '" + pCode + "%'";
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

        public DataTable getBillingStatsDaily(string pCode, DateTime billMon)
        {
            string sql = @"SELECT SRT_ORDER2, SRT_ORDER1, MONTH, SDIV_CODE CODE, SDIV_NAME NAME, TNOCONSUMERS, NOUNBILLEDCASES, NOSTSREADING, NODISCASES, NORECCASES, NOMCOCASES, NODEFMETERS, LOCKCASES, NONEWCONN, CREDBALCONSM, NOHEAVYBCASES, CREDBALAMT "
                         + " from VW_BILLING_STATS_DAILY "
                         + " where MONTH = (select max(MONTH) from VW_BILLING_STATS_DAILY)";
            OracleConnection con = null;
            OracleCommand cmd;
            con = new OracleConnection(GetConnStr(pCode));

            string sortorder = "";

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
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            sql += " AND SRT_ORDER2 " + sortorder;
            if (!string.IsNullOrEmpty(pCode))
            {
                sql += " AND SDIV_CODE LIKE '" + pCode + "%'";
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
            string mndConStr = System.Configuration.ConfigurationManager.ConnectionStrings["MNDGetConnStr(pCode)"].ToString();
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
        public DataTable getBillData(string ts, string kwh, DateTime strtPeriod, DateTime endPeriod, string trf, string ed_cd,
            string seasonchrg, string stdclsfcd, string gstexmtcd, string fixchrg, string mtrent, string srvrent, string dssur, string uosr,
            string p_tot_cons, string nooftv, string fatapatacd)
        {

            string startMonth = strtPeriod.ToString("dd") + "-" + strtPeriod.ToString("MMM") + "-" +
                                strtPeriod.ToString("yy");
            string endMonth = endPeriod.ToString("dd") + "-" + endPeriod.ToString("MMM") + "-" +
                              endPeriod.ToString("yy");
            string sql = "DISCO.PROC_BILL4API";
            OracleConnection con = null;
            con = new OracleConnection(GetConnStr("15"));
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

                OracleParameter opKWH = new OracleParameter("KWH", OracleDbType.Decimal);
                //opKWH.Size = 200;
                opKWH.Value = decimal.Parse(kwh);
                cmd.Parameters.Add(opKWH);


                OracleParameter opSperiod = new OracleParameter("S_PERIOD", OracleDbType.Varchar2);
                opSperiod.Value = startMonth.ToUpper();
                cmd.Parameters.Add(opSperiod);

                OracleParameter opEperiod = new OracleParameter("E_PERIOD", OracleDbType.Varchar2);
                opEperiod.Value = endMonth.ToUpper();
                cmd.Parameters.Add(opEperiod);

                OracleParameter opTRF = new OracleParameter("TRF", OracleDbType.Varchar2);
                opTRF.Value = trf;
                cmd.Parameters.Add(opTRF);

                OracleParameter ed_Cd = new OracleParameter("ED_CD", OracleDbType.Varchar2);
                ed_Cd.Value = ed_cd;
                cmd.Parameters.Add(ed_Cd);

                OracleParameter seasonCharge = new OracleParameter("SEASON_CHRG", OracleDbType.Decimal);
                seasonCharge.Value = float.Parse(seasonchrg);
                cmd.Parameters.Add(seasonCharge);

                OracleParameter stdClfCd = new OracleParameter("STD_CLSF_CD", OracleDbType.Varchar2);
                stdClfCd.Value = stdclsfcd;
                cmd.Parameters.Add(stdClfCd);

                OracleParameter gstExmtCd = new OracleParameter("GST_EXMT_CD", OracleDbType.Varchar2);
                gstExmtCd.Value = gstexmtcd;
                cmd.Parameters.Add(gstExmtCd);


                OracleParameter fixCharge = new OracleParameter("FIX_CHARGE", OracleDbType.Decimal);
                fixCharge.Value = float.Parse(fixchrg);
                cmd.Parameters.Add(fixCharge);

                OracleParameter meterRent = new OracleParameter("METER_RENT", OracleDbType.Decimal);
                meterRent.Value = float.Parse(mtrent);
                cmd.Parameters.Add(meterRent);

                OracleParameter srvRent = new OracleParameter("SRV_RENT", OracleDbType.Decimal);
                srvRent.Value = float.Parse(srvrent);
                cmd.Parameters.Add(srvRent);

                OracleParameter dssUr = new OracleParameter("DSSUR", OracleDbType.Decimal);
                dssUr.Value = float.Parse(dssur);
                cmd.Parameters.Add(dssUr);

                OracleParameter uoSur = new OracleParameter("UOSUR", OracleDbType.Decimal);
                uoSur.Value = float.Parse(uosr);
                cmd.Parameters.Add(uoSur);

                OracleParameter noOfTv = new OracleParameter("NO_OF_TV", OracleDbType.Decimal);
                noOfTv.Value = float.Parse(nooftv);
                cmd.Parameters.Add(noOfTv);

                OracleParameter fata_pata_cd = new OracleParameter("FATA_PATA_CD", OracleDbType.Varchar2);
                fata_pata_cd.Value = fatapatacd;
                cmd.Parameters.Add(fata_pata_cd);

                OracleParameter p_Tot_cons = new OracleParameter("P_TOT_CONS", OracleDbType.Decimal);
                p_Tot_cons.Value = float.Parse(p_tot_cons);
                cmd.Parameters.Add(p_Tot_cons);

                OracleParameter edres = new OracleParameter("V_ED", OracleDbType.Decimal);
                edres.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(edres);

                OracleParameter energyRes = new OracleParameter("V_Energy", OracleDbType.Decimal);
                energyRes.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(energyRes);

                OracleParameter gstRes = new OracleParameter("V_GST", OracleDbType.Decimal);
                gstRes.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(gstRes);

                OracleParameter itaxRes = new OracleParameter("V_ITAX", OracleDbType.Decimal);
                itaxRes.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(itaxRes);

                OracleParameter ptvfee = new OracleParameter("V_PTVFEE", OracleDbType.Decimal);
                ptvfee.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(ptvfee);

                OracleParameter eqSur = new OracleParameter("V_EQSUR", OracleDbType.Decimal);
                eqSur.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(eqSur);

                OracleParameter njSur = new OracleParameter("V_NJSUR", OracleDbType.Decimal);
                njSur.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(njSur);


                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                OracleDataAdapter da = new OracleDataAdapter(cmd);


                dt.Columns.Add("ENRCHRG");
                dt.Columns.Add("GST");
                dt.Columns.Add("ED");
                dt.Columns.Add("ITAX");
                dt.Columns.Add("PTVFEE");
                dt.Columns.Add("EQSUR");
                dt.Columns.Add("NJSUR");
                DataRow data = dt.NewRow();
                data["ENRCHRG"] = cmd.Parameters["V_ENERGY"].Value.ToString();
                data["GST"] = cmd.Parameters["V_GST"].Value.ToString();
                data["ED"] = cmd.Parameters["V_ED"].Value.ToString();
                data["ITAX"] = cmd.Parameters["V_ITAX"].Value.ToString();
                data["PTVFEE"] = cmd.Parameters["V_PTVFEE"].Value.ToString();
                data["EQSUR"] = cmd.Parameters["V_EQSUR"].Value.ToString();
                data["NJSUR"] = cmd.Parameters["V_NJSUR"].Value.ToString();
                dt.Rows.Add(data);



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


        public DataTable GetDefConsSumAmntSlabsBySproc(string pCode, string pDefType, string pDefStatus, string pTariffCat, string pSortOrder)
        {
            DataTable dt = new DataTable();
            using (OracleConnection cn = new OracleConnection(GetConnStr(pCode)))
            {
                try
                {


                    string sql = "PKG_DSH_VIEWS.proc_DEF_CONS_SUM_AMNT_SLABS";
                    OracleCommand cmd = new OracleCommand(sql, cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.InitialLONGFetchSize = 1000;
                    //cmd.CommandText = "disco_dsh."+"PKG_DSH_VIEWS.proc_DEF_CONS_SUM_AMNT_SLABS";
                    OracleParameter cur_dsh = new OracleParameter("cur_dsh", OracleDbType.RefCursor);
                    cur_dsh.Direction = ParameterDirection.InputOutput;
                    cmd.Parameters.Add(cur_dsh);

                    OracleParameter SRTORDER2 = new OracleParameter("P_SRTORDER2", OracleDbType.Varchar2);
                    SRTORDER2.Value = pSortOrder;
                    SRTORDER2.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(SRTORDER2);

                    OracleParameter SDIVCODE = new OracleParameter("P_SDIVCODE", OracleDbType.Varchar2);
                    SDIVCODE.Value = pSortOrder;
                    SDIVCODE.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(SDIVCODE);

                    OracleParameter DEF_TYPE = new OracleParameter("P_DEF_TYPE", OracleDbType.Varchar2);
                    DEF_TYPE.Value = pDefType;
                    DEF_TYPE.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(DEF_TYPE);


                    OracleParameter DEF_STATUS = new OracleParameter("P_DEF_STATUS", OracleDbType.Varchar2);
                    DEF_STATUS.Value = pDefStatus;
                    DEF_STATUS.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(DEF_STATUS);

                    OracleParameter TARIFF_CAT = new OracleParameter("P_TARIFF_CAT", OracleDbType.Varchar2);
                    TARIFF_CAT.Value = pDefStatus;
                    TARIFF_CAT.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(TARIFF_CAT);

                    if (cn.State != ConnectionState.Open)
                    {
                        cn.Open();
                    }

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
                    dt = dtErr;
                }
                finally
                {
                    if (cn != null && cn.State == ConnectionState.Open)
                    {
                        cn.Close();

                    }
                }
                return dt;
            }
        }
    }
}