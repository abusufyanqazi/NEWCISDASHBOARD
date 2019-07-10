using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class TentativeRefWiseList
    {
        public String AsOn { get; set; }
        public String MonthEnding { get; set; }
        public String CenterCode { get; set; }
        public String CenterName { get; set; }
        public List<RefWiseTentative> RefWise { get; set; }

        public TentativeRefWiseList(string asOn, string monthEnding, string centerCode, string centerName, List<RefWiseTentative> refWise)
        {
            AsOn = asOn;
            MonthEnding = monthEnding;
            CenterCode = centerCode;
            CenterName = centerName;
            RefWise = refWise;
        }
    }
}