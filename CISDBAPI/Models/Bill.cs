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
        public String TotAmount { get; set; }
        public Bill(string var, string tr, string fc, string ed, string gst, string ptv, string nj, string mtr,
            string srv, string slbStr)
        {
            this.VarCharges = var;
            this.TrSur = tr;
            this.FcSur = fc;
            this.ED = ed;
            this.GST = gst;
            this.PTV = ptv;
            this.NjSur = nj;
            this.MeterRent = mtr;
            this.ServiceRent = srv;
            this.SlabStr = slbStr;
            this.TotAmount =(Int32.Parse(var) + Int32.Parse(tr) + Int32.Parse(fc) + Int32.Parse(ed) + Int32.Parse(gst) +
                             Int32.Parse(ptv) + Int32.Parse(nj) +
                             Int32.Parse(mtr) + Int32.Parse(srv)).ToString();
        }
    }
}