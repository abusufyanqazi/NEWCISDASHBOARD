using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace DashBoardAPI.Controllers
{
    public class CreditAdjustmentController : ApiController
    {
        //string token, string code = "15",string age="1", string phase="1", string trf="01"
        //string token, string code = "15", string batchFrom="01", string batchTo="02", string unitFlag="0"
        public CreditAdjustments Get(string code = "15",string age="01", string phase="20", string trf="0")
        {
            return new DBoardBridge().GetCRAdjustments(code, age, phase,
                char.Parse(trf.Substring(0, 1)));
        }
    }
}
