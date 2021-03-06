﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.Models
{
    public class Leg
    {
        public int LegID { get; set; }
        public int SortOrder { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Length { get; set; }
        public int Degree { get; set; }
        public Mandrel Mandrel { get; set; }
        public bool IsRight { get; set; }
    }
}
