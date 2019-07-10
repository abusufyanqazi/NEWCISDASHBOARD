using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace DashBoardAPI.Controllers
{
    public class FeederLossesController : ApiController
    {
        public List<FeederLosses> Get()
        {
            return new DBoardBridge().GetFeederLosses();
        }
    }
}
