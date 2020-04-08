using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneMasons.Models;

namespace CapstoneMasons.ViewModels
{
    public class FormulaCreate
    {
        //Input Data
        public List<Mandrel> Mandrels { get; set; }

        //Output Data
        public int BarSize { get; set; }
        [Required(ErrorMessage = "Input a value in between 0 and 360.")]
        [Range(0, 360)]
        public int Degree { get; set; }
        public int MandrelID { get; set; }
        [Required(ErrorMessage = "Input a the pin you used.")]
        public string PinNumber { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal InGained { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
