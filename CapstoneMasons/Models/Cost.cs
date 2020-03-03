using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.Models
{
    public class Cost
    {
        public int CostID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
