using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace CISDBAPI.Controllers
{
    public class TentativeListRefWiseController : ApiController
    {
     /// <summary>
     /// 
     /// </summary>
        /// <param name="code">code</param>
        /// <param name="BMonth">AsOn</param>
     /// <returns></returns>
        public TentativeRefWiseList Get(string code, string BMonth)
        {
            DBoardBridge bridgeObj = new DBoardBridge();

            return bridgeObj.ListTentativeRefWise(code, BMonth);
             
        }
    }
}
