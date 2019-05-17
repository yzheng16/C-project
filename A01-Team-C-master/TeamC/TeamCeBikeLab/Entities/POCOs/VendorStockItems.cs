using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class VendorStockItems
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int QOH { get; set; }
        public int QOO { get; set; }
        public int ROL { get; set; }
        public int Buffer { get; set; }
        public decimal Price { get; set; }

        public VendorStockItems(int partID, string description, int quantityOnHand, int reorderLevel, int quantityOnOrder, int buffer, decimal purchasePrice)
        {
            ID = partID;
            Description = description;
            QOH = quantityOnHand;
            ROL = reorderLevel;
            QOO = quantityOnOrder;
            Buffer = buffer;
            Price = purchasePrice;
        }

        public VendorStockItems() { }
    }
}
