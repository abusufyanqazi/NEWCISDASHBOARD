using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class Bill
    {
        public String VarCharges { get; set; }
        public String TrSur { get; set; }
        public String FcSur { get; set; }
        public String ED { get; set; }
        public String GST { get; set; }
        public String PTV { get; set; }
        public String NjSur { get; set; }
        public String MeterRent { get; set; }
        public String ServiceRent { get; set; }
        public String SlabStr { get; set; }
        public String Itax { get; set; }
        public String EqSur { get; set; }
        public String TotAmount { get; set; }


        public Bill(string var, string tr, string fc, string ed, string gst, string ptv, string nj, string mtr,
            string srv, string slbStr, string it, string eq)
        {
            this.VarCharges = (var == "null") ? "0" : var;
            this.TrSur = (tr == "null") ? "0" : tr;
            this.FcSur = (fc == "null") ? "0" : fc;
            this.ED = (ed == "null") ? "0" : ed;
            this.GST = (gst == "null") ? "0" : gst;
            this.PTV = (ptv == "null") ? "0" : ptv;
            this.NjSur = (nj == "null") ? "0" : nj;
            this.MeterRent = (mtr == "null") ? "0" : mtr;
            this.ServiceRent = (srv == "null") ? "0" : srv;
            this.Itax = (it == "null") ? "0" : it;
            this.SlabStr = (slbStr == "null") ? "0" : slbStr;
            this.EqSur = (eq == "null") ? "0" : eq;
            this.TotAmount = (float.Parse(this.VarCharges) + float.Parse(this.TrSur) + float.Parse(this.FcSur) + float.Parse(this.ED) + float.Parse(this.GST) +
                             float.Parse(this.PTV) + float.Parse(this.NjSur) + float.Parse(this.Itax) + float.Parse(this.EqSur) +
                             float.Parse(this.MeterRent) + float.Parse(this.ServiceRent)).ToString();
        }
    }

}

