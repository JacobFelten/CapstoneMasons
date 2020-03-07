using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.Models
{
    public class Mandrel
    {
        public int MandrelID { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Radius { get; set; }
    }
}
