using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class CartInfo
    {
        
        public IEnumerable<CartItem> CartItems { get; set; }
        public int CartItemId { get; set; }
        public string Description { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int QOH { get; internal set; }
    }
}
