using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.ViewModels
{
    public class OpenQuote
    {
        public List<Quote> Quotes { get; set; }
        public decimal TotalCost { get; set; }
        public string NewOrOld { get; set; }
        public string SearchBar { get; set; }
    }
}
