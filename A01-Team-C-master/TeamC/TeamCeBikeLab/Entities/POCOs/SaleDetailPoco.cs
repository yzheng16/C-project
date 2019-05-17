using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class SaleDetailPoco
    {
        public int PartID { get; set; }
        public int Quantity { get; set; }
        public decimal SellingPrice { get; set; }
        public bool Backordered { get; set; }
        public DateTime? ShippedDate { get; set; }
    }
}
