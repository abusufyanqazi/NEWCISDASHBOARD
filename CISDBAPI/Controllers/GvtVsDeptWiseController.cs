using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace CISDBAPI.Controllers
{
    public class GvtVsDeptWiseController : ApiController
    {
        public GvtVsAssmntDeptWise Get(string code, string bmonth)
        {
            return new DBoardBridge().DepWiseInfo(code, bmonth);
        }
    }
}
