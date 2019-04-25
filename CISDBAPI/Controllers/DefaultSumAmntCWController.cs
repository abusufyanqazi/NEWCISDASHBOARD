using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace CISDBAPI.Controllers
{

    public class DefaultSumAmntCWController : ApiController
    {
        public DefaultSumCentreWise Get(string code = "15", string type = "PRIVATE", string status = "RUNNING DEFAULTERS", string trf = "DOMESTIC")
        {
            return new DBoardBridge().GetDefaulterSummaryAmntCentreWise(code, type, status, trf);
        }
    }
}
