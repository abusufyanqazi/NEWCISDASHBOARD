using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class GvtVsAssmntCenterWiseBillData
    {
        public String BillingMonth { get; set; }
        public String CenterCode { get; set; }
        public String CenterName { get; set; }
        public List<CenterWiseDepts> CenterWise { get; set; }

        public GvtVsAssmntCenterWiseBillData(string billingMonth, string centerCode, string centerName, List<CenterWiseDepts> centerWise)
        {
            BillingMonth = billingMonth;
            CenterCode = centerCode;
            CenterName = centerName;
            CenterWise = centerWise;
        }
    }
}