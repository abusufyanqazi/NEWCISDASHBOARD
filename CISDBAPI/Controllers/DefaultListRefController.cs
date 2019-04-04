using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace DashBoardAPI.Controllers
{
    public class DefaultListRefController : ApiController
    {
        public DefaultListRefWise Get(string code = "15", string age = "PRIVATE", string phase = "RUNNING DEFAULTERS", string trf = "DOMESTIC")
        {
            //token, code, type, status, tariff
            return new DBoardBridge().GetDefListRefWise(code, age, phase, trf);
        }
    }
}
