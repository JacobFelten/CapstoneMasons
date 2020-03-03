using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.Models
{
    public class Formula
    {
        public int FormulaID { get; set; }
        public int BarSize { get; set; }
        public int Degree { get; set; }
        public Mandrel Mandrel { get; set; }
        public decimal PinNumber { get; set; }
        public decimal InGained { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
