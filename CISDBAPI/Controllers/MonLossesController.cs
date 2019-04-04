﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace DashBoardAPI.Controllers
{
    public class MonLossesController : ApiController
    {
        public List<MonLosses> Get(string code = "15")
        {
            return new DBoardBridge().GetMonLosses(code);
        }
    }
}
