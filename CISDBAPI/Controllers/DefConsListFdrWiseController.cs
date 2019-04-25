using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace DashBoardAPI.Controllers
{
    public class DefConsListFdrWiseController : ApiController
    {
        public DefConsListFdrWise Get(string code = "15",string fdr="")
        {
            //token, code, type, status, tariff
            return new DBoardBridge().GetDefConsListFdrCdWise(code, fdr);
        }
    }
}
