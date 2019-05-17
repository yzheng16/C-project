using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class CategoryInfo
    {
        public int CategoryID { get; set; }
        public string Description { get; set; }
        public int AmountOfParts { get; set; }
        public int TotalParts { get; set; }
    }
}
