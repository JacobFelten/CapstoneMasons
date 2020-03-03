using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.Models
{
    public class Shape
    {
        private List<Leg> legs = new List<Leg>();

        public int ShapeID { get; set; }
        public int BarSize { get; set; }
        public int LegCount { get; set; }
        public List<Leg> Legs
        {
            get { return legs; }
        }
        public int Qty { get; set; }
        public int NumCompleted { get; set; }
    }
}
