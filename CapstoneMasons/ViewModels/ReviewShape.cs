using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.ViewModels
{
    public class ReviewShape
    {
        [Required]
        public int QuoteID { get; set; }
        [Required]
        public int ShapeID { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required]
        public int BarSize { get; set; }
        public int NumOfBars { get; set; }
        public decimal CutLength { get; set; }
        public int Completed { get; set; }
        public List<CutInstruction> Instructions { get; set; }
        public List<ReviewLeg> Legs { get; set; }
        public List<Remnant> Remnants { get; set; }
        [Required]
        public bool ReviewOpen { get; set; }
        [Required]
        public bool ReviewQuote { get; set; }
    }
}
