using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace   DashBoardAPI.Models
{
    public class RegWiseDfl
    {
        public String CenterCode { get; set; }
        public String CenterName { get; set; }
        public String UnpaidAmount { get; set; }
        public String DefferedAmount { get; set; }
        public String OwningAmount { get; set; }
        public String ClosingBalance { get; set; }

        public RegWiseDfl(string centerCode, string centerName, string unpaidAmount, string defferedAmount, string owningAmount, string closingBalance)
        {
            CenterCode = centerCode;
            CenterName = centerName;
            UnpaidAmount = unpaidAmount;
            DefferedAmount = defferedAmount;
            OwningAmount = owningAmount;
            ClosingBalance = closingBalance;
        }
    }
}