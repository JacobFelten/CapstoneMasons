using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.ViewModels
{
    public class ReviewShape
    {
        public int ShapeID { get; set; }
        public int Qty { get; set; }
        public int BarSize { get; set; }
        public int NumOfBars { get; set; }
        public decimal CutLength { get; set; }
        public List<CutInstruction> Instructions { get; set; }
        public List<ReviewLeg> Legs { get; set; }
        public List<Remnant> Remnants { get; set; }
    }
}
