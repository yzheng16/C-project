using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class PurchaseOrderInfo
    {
        public int PurchaseOrderID { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotal { get; set; }
        public int VendorID { get; set; }
        public IEnumerable<PurchaseOrderDetails> PurchaseOrderDetails { get; set; }
    }
}
