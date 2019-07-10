using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class DeptInfo
    {
        public String DeptCode { get; set; }
        public String DeptName { get; set; }
        public String TotConsumers { get; set; }
        public String OpeningBalance { get; set; }
        public String CurrentAssmnt { get; set; }
        public String Payments { get; set; }
        public String ClosingBalance { get; set; }

        public DeptInfo(string deptCode, string deptName, string totConsumers, string openingBalance, string currentAssmnt, string payments, string closingBalance)
        {
            DeptCode = deptCode;
            DeptName = deptName;
            TotConsumers = totConsumers;
            OpeningBalance = openingBalance;
            CurrentAssmnt = currentAssmnt;
            Payments = payments;
            ClosingBalance = closingBalance;
        }
    }
}