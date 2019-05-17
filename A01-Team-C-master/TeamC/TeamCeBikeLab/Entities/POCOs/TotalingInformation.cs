using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class TotalingInformation
    {
        public double TaxAmount;
        public double SubTotal;
        public double Total; //(SubTotal * TaxAmount) + SubTotal
    }
}
