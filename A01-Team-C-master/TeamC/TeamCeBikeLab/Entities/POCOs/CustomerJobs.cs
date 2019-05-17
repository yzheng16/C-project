using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCeBikeLab.Entities.POCOs
{
    public class CustomerJobs
    {
        public int JobID { get; set; }
        public DateTime JobDateIn { get; set; }
        public DateTime? JobDateStarted { get; set; }
        public DateTime? JobDateDone { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
    }
}
