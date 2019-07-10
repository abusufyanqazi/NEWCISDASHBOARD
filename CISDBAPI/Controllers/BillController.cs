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

        public Bill Get(string kwh = "0", string trf="01", string ed_cd = "1", string stdclsfcd = "0101", string nooftv = "1", string fatapatacd = "A")
        {
            return new DBoardBridge().GetBill(kwh, trf, ed_cd, "0", stdclsfcd, "0", "0", "0", "0", "0", "0", nooftv, fatapatacd, kwh);
        }
    }
}