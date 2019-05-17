using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class ServiceDetailPartsPoco
    {
        public int ServiceDetailPartID { get; set; }
        public int ServiceDetailID { get; set; }
        public int PartID { get; set; }
        public string Description { get; set; }
        public short Quantity { get; set; }
    }
}
