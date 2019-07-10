using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace CISDBAPI.Controllers
{
    public class RegWiseDefaultersController : ApiController
    {
        /// <summary>
        /// {code}/{BMonth}
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="asOn">BMonth</param>
        /// <returns></returns>
        public MonthlyDflRegWise Get(string code, string BMonth)
        {
            DBoardBridge dbBridgeObj = new DBoardBridge();
            return dbBridgeObj.DefListRegWise(code, BMonth);
        }
    }
}
