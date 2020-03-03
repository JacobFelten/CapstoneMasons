using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.Models
{
    public class Leg
    {
        public int LegID { get; set; }
        public decimal Length { get; set; }
        public int Degree { get; set; }
        public Mandrel Mandrel { get; set; }
    }
}
