using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class RefWiseTentative
    {
        public String SrNo { get; set; }
        public String RefNo { get; set; }
        public String Tariff { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public String EroNo { get; set; }
        public String EroDate { get; set; }

        public String Age { get; set; }
        public String UnpaidAmount { get; set; }
        public String DefferedAmount { get; set; }
        public String OwningAmount { get; set; }
        public String ClosingAmount { get; set; }

        public RefWiseTentative(string srNo, string refNo, string tariff, string name, string address, string eroNo, string eroDate, string age, string unpaidAmount, string defferedAmount, string owningAmount, string closingAmount)
        {
            SrNo = srNo;
            RefNo = refNo;
            Tariff = tariff;
            Name = name;
            Address = address;
            EroNo = eroNo;
            EroDate = eroDate;
            Age = age;
            UnpaidAmount = unpaidAmount;
            DefferedAmount = defferedAmount;
            OwningAmount = owningAmount;
            ClosingAmount = closingAmount;
        }
    }
}