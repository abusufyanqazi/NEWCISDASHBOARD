using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace DashBoardAPI.Controllers
{
    public class DefaultSumBatchController : ApiController
    {
        public DefaulterBatchSummary Get(string code = "15", string type = "PRIVATE", string status = "RUNNING DEFAULTERS", string trf = "DOMESTIC")
        {
            return new DBoardBridge().GetDefaulterSummaryBatch(code, type, status, trf);
        }
    }
}
