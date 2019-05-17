using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class JobPoco
    {
        public int? EmployeeID { get; set; }
        public int CustomerID { get; set; }
        public DateTime JobDateIn { get; set; }
        public decimal ShopRate { get; set; }
        public string StatusCode { get; set; }
        public string VehicleIdentification { get; set; }
    }
}
