using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class TariffWise
    {
        public String Category { get; set; }
        public String Connections { get; set; }
        public String UnitsBilled { get; set; }
        public String Billing { get; set; }
        public String Payment { get; set; }
        public String ClosingBalance { get; set; }
        public String SpillOver { get; set; }
        public String Percentage { get; set; }

        public TariffWise(string category, string connections, string unitsBilled, string billing, string payment, string closingBalance, string spillOver, string percentage)
        {
            Category = category;
            Connections = connections;
            UnitsBilled = unitsBilled;
            Billing = billing;
            Payment = payment;
            ClosingBalance = closingBalance;
            SpillOver = spillOver;
            Percentage = percentage;
        }
    }
}