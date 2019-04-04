using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace DashBoardAPI.Controllers
{
    public class DefConsFdrCdWiseController : ApiController
    {
        public DefaultSumFdrWise Get(string code = "15")
        {
            //token, code, type, status, tariff
            return new DBoardBridge().GetDefConsFdrCdWise(code);
        }
    }
}