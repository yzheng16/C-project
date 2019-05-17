using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class SaleDTO
    {
        public DateTime SaleDate { get; set; }
        public string UserName { get; set; }
        public int EmployeeID { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotal { get; set; }
        public int? CouponID { get; set; }
        public string PaymentType { get; set; }
        public IEnumerable<SaleDetailPoco> SaleDetails { get; set; }
    }
}
