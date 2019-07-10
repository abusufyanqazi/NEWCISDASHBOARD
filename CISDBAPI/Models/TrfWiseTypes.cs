using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class TrfWiseTypes
    {
        public List<TariffWise> Domestic { get; set; }
        public List<TariffWise> Commercial { get; set; }

        public TrfWiseTypes(List<TariffWise> domestic, List<TariffWise> commercial)
        {
            Domestic = domestic;
            Commercial = commercial;
        }
    }
}