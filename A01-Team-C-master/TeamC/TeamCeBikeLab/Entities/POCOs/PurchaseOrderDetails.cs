using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class PurchaseOrderDetails
    {
        public int PurchaseOrderID { get; set; }
        public int PurchaseOrderDetailID { get; set; }
        public int VendorID { get; set; }
        public int ID { get; set; } //part ID
        public string Description { get; set; }
        public int QOH { get; set; }
        public int ROL { get; set; }
        public int QOO { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }

        public PurchaseOrderDetails(int partID, string description, int quantityOnHand, int reorderLevel, int quantityOnOrder, int quantity, decimal purchasePrice)
        {
            ID = partID;
            Description = description;
            QOH = quantityOnHand;
            ROL = reorderLevel;
            QOO = quantityOnOrder;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
        }

        public PurchaseOrderDetails() { }

    }
}
