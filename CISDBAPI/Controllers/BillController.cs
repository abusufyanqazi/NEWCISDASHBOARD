using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using util;
using DashBoardAPI.Models;

namespace DashBoardAPI.Controllers
{
    public class BillController : ApiController
    {

        public Bill Get(string kwh = "0", string trf="01")
        {
            return new DBoardBridge().GetBill(kwh, trf);
        }
    }
}