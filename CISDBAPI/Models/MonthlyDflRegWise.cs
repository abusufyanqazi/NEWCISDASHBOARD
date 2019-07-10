using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class MonthlyDflRegWise
    {
        public String AsOn { get; set; }
        public String MonthEnding { get; set; }
        public String CenterCode { get; set; }
        public String CenterName { get; set; }
        public List<RegWiseDfl> RegWise { get; set; }

        public MonthlyDflRegWise(string asOnDate, string monthEnding, string companyCode, string companyName, List<RegWiseDfl> regWiseDefaulters)
        {
            AsOn = asOnDate;
            MonthEnding = monthEnding;
            CenterCode = companyCode;
            CenterName = companyName;
            RegWise = regWiseDefaulters;
        }
    }
}