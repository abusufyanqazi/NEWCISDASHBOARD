﻿using System;
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
        public DefaultListRefWise Get(string code = "15", string type = "PRIVATE", string status = "RUNNING DEFAULTERS", string trf = "DOMESTIC", string slab = "01--------------1000")
        {
            //token, code, type, status, tariff
            return new DBoardBridge().GetDefListRefWise(code, type, status, trf, slab, 'M'); //API-7 (ii) 1-DefaulterSummaryAmountSlab_Ref Wise
        }
    }
}
