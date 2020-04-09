using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string PinNumber { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal InGained { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
