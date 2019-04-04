using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace DashBoardAPI.Controllers
{
    public class DefaultSumCntrWiseController : ApiController
    {
        public DefaulterBatchSummary Get(string code = "15")
        {
            //token, code, type, status, tariff
            return new DBoardBridge().GetDefaulterSummaryBatch(code);
        }
    }
}
