using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class TariffWiseBilling
    {
        public String BillingMonth { get; set; }
        public String CenterCode { get; set; }
        public String CenterName { get; set; }
        public List<RgnWiseTrf> RegWise { get; set; }

        public TariffWiseBilling(string billingMonth, string centerCode, string centerName, List<RgnWiseTrf> regWise)
        {
            BillingMonth = billingMonth;
            CenterCode = centerCode;
            CenterName = centerName;
            RegWise = regWise;
        }
    }
}