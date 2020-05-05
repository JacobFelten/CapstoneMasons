using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.ViewModels
{
    public class ReviewOpen
    {
        public int QuoteID { get; set; }
        public string Name { get; set; }
        public string OrderNumber { get; set; }
        public decimal Discount { get; set; }
        public string Setup { get; set; }
        public ReviewQuote ReviewQuote { get; set; }
        public int[] Completed { get; set; }
    }
}
