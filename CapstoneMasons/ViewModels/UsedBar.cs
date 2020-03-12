using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.ViewModels
{
    public class UsedBar
    {
        public int BarSize { get; set; }
        public int NumOfBars { get; set; }
        public decimal BarCost { get; set; }
        public int NumOfCuts { get; set; }
        public decimal CutCost { get; set; }
        public int NumOfBends { get; set; }
        public decimal BendCost { get; set; }
    }
}
