using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace DashBoardAPI.Controllers
{
    public class DefectiveDetailsController : ApiController
    {
        public DefectiveDetails Get(string code = "15",string age="1", string phase="1", string trf="01")
        {
            return new DBoardBridge().GetDefectiveDetails(code, age, phase,trf);
        }
    }
}
