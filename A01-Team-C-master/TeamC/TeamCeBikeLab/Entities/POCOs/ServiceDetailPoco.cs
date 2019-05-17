using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class ServiceDetailPoco
    {
        public int ServiceDetailID { get; set; }
        public int JobID { get; set; }
        public string Description { get; set; }
        public decimal JobHours { get; set; }
        public int? CouponID { get; set; }
        public string CouponIDValue { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
    }
}
