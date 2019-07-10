using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class GvtVsAssmntDeptWise
    {
        public String BillingMonth { get; set; }
        public String CenterCode { get; set; }
        public String CenterName { get; set; }
        public List<DeptInfo> DeptWise { get; set; }

        public GvtVsAssmntDeptWise(string billingMonth, string centerCode, string centerName, List<DeptInfo> deptWise)
        {
            BillingMonth = billingMonth;
            CenterCode = centerCode;
            CenterName = centerName;
            DeptWise = deptWise;
        }
    }
}