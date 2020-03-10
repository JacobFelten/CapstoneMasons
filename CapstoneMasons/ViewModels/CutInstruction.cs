using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.ViewModels
{
    public class CutInstruction
    {
        public int CutQty { get; set; }
        public decimal PerLength { get; set; }
        public string PerType { get; set; }
        public int ForQty { get; set; }
    }
}
