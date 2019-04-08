using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAPI.Models;

namespace CISDBAPI.Controllers
{
    public class DefaultListCntrWiseController : ApiController
    {
        public DefaulterBatchSummary Get(string rs, string age, string batch, string pgvt, string rundc, string trf, string srt)
        {
            // {rs}/{age}/{batch}/{pgvt}/{rundc}/{trf}/{srt}
            return new DBoardBridge().GetDefaultListCntrWise("15",rs, age, batch, pgvt, rundc, trf, srt);
        }
    }
}
