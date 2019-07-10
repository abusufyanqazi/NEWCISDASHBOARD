using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace CISDBAPI.Controllers
{
    public class TrfWiseBillController : ApiController
    {
        public TariffWiseBilling Get(string code, string BMonth)
        {            
            return new DBoardBridge().TrfWiseBillData(code, BMonth);
        }
    }
}
