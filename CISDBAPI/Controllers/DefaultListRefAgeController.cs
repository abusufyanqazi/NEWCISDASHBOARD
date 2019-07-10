using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace DashBoardAPI.Controllers
{
    public class DefaultListRefAgeController : ApiController
    {
        public DefaultListRefWise Get(string code = "15", string type = "PRIVATE", string status = "RUNNING DEFAULTERS", string trf = "DOMESTIC", string slab = "1")
        {
            //token, code, type, status, tariff
            return new DBoardBridge().GetDefListRefWise(code, type, status, trf, slab, 'A');//API-7 (ii) 2-DefaulterSummaryAgeSlab_Ref Wise
        }
    }
}


