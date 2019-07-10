using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class RgnWiseTrf
    {
        public String CenterCode { get; set; }
        public String CenterName { get; set; }
        
        public TrfWiseTypes   TariffWise{ get; set; }

        public RgnWiseTrf(string centerCode, string centerName, TrfWiseTypes tariffWise)
        {
            CenterCode = centerCode;
            CenterName = centerName;
            TariffWise = tariffWise;
        }
    }
}