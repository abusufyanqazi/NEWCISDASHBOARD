using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace CISDBAPI.Controllers
{
    public class DefectiveDetailRController : ApiController
    {
        public DefectiveDetailsR Get(string code = "15", string age = "1", string trf = "01")
        {
            return new DBoardBridge().GetDefectiveMeterRegionWise(code, age, trf);
        }
    }
}
