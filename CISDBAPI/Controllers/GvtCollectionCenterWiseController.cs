using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using util;
using DashBoardAPI.Models;

namespace CISDBAPI.Controllers
{
    public class GvtCollectionCenterWiseController : ApiController
    {
        public GvtVsAssmntCenterWiseBillData Get(string code, string bmonth)
        {
            return new DBoardBridge().CenterWiseDepInfo(code, bmonth);
        }
    }
}
