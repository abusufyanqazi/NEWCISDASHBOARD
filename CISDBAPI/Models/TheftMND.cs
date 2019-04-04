using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util;

namespace DashBoardAPI
{
    public class TheftMND
    {
        //BILL_MNTH, NOTE_NO, ADJ_DT, UNITS, AMOUNT,PAY_AGAINTS_DET
        public string BILL_MNTH { get; set; }
        public string NOTE_NO { get; set; }
        public string ADJ_DT { get; set; }
        public string UNITS { get; set; }
        public string AMOUNT { get; set; }
        public string PAY_AGAINTS_DET { get; set; }

        public TheftMND(string bilMon, string noteNO, string adjDt, string unit, string amnt, string payDetection)
        {
            this.BILL_MNTH = utility.GetFormatedDate(bilMon); 
            this.NOTE_NO = noteNO;
            this.ADJ_DT =  utility.GetFormatedDate(adjDt);
            this.UNITS = unit;
            this.AMOUNT = amnt;
            this.PAY_AGAINTS_DET = payDetection;
        }

    }
}