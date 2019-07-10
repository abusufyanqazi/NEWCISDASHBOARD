using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardAPI.Models
{
    public class CenterWiseDepts
    {
        public String CenterCode { get; set; }
        public String CenterName { get; set; }
        public List<DeptInfo>DeptWise { get; set; }

        public CenterWiseDepts(string centerCode, string centerName, List<DeptInfo> deptWise)
        {
            CenterCode = centerCode;
            CenterName = centerName;
            DeptWise = deptWise;
        }
    }
}