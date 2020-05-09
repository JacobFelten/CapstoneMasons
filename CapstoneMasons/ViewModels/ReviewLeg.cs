using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.ViewModels
{
    public class ReviewLeg
    {
        public decimal Length { get; set; }
        public int Degree { get; set; }
        public string Mandrel { get; set; }
        public string PinNumber { get; set; }
        public decimal InGained { get; set; }
        public bool IsRight { get; set; }
    }
}
