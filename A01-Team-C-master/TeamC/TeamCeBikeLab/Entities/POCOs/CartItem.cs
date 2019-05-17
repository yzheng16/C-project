using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class CartItem
    {
        public string Description { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }

    }
}
