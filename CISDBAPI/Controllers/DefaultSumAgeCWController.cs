using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace CISDBAPI.Controllers
{
    public class DefaultSumAgeCWController : ApiController
    {
        public DefaultSumCentreWise Get(string code = "15", string age = "PRIVATE", string phase = "RUNNING DEFAULTERS", string trf = "DOMESTIC")
        {
            //token, code, type, status, tariff
            return new DBoardBridge().GetDefaulterSummaryAgeCentreWise(code, age, phase, trf);
        }
    }
}
