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
        public CreditAdjustments Get(string code = "15", string batch = "01", string units = "0")
        {
            return new DBoardBridge().GetCRAdjustments(code, batch, char.Parse(units.Substring(0, 1)));
        }
    }
}
