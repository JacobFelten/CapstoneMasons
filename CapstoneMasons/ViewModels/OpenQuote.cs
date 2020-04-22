using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.ViewModels
{
    public class OpenQuote
    {
        private List<Quote> quotes = new List<Quote>();
        public List<Quote> Quotes { get { return quotes; } }
        public decimal TotalCost { get; set; }
    }
}
