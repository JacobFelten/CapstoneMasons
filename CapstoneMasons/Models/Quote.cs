using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.Models
{
    public class Quote
    {
        private List<Shape> shapes = new List<Shape>();
        private List<Cost> costs = new List<Cost>();

        public int QuoteID { get; set; }
        public string Author { get; set; }
        public string Name { get; set; } //After Review
        public string OrderNum { get; set; } //After Review
        public List<Shape> Shapes //Before Review
        {
            get { return shapes; }
        }
        public List<Cost> Costs //Before Review
        {
            get { return costs; }
        }
        public DateTime DateQuoted { get; set; } //Before Review
        public bool PickedUp { get; set; } //After Review
        public bool Open { get; set; } //After Review
        public bool? AddSetup { get; set; } //After Review
        public decimal Discount { get; set; } //After Review
        public bool UseFormulas { get; set; } //Before Review
    }
}
