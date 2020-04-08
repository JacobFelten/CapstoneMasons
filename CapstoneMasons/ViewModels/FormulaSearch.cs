using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.ViewModels
{
    public class FormulaSearch
    {
        public List<Formula> SearchResults { get; set; }
        public List<int> BarSizes { get; set; }
        public List<int> Degrees { get; set; }
        public List<Mandrel> Mandrels { get; set; }
        public int? BarSize { get; set; }
        public int? BendDegree { get; set; }
        public int? MandrelID { get; set; }
    }
}
