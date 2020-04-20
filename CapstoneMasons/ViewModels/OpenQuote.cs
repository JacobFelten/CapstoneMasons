using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.ViewModels
{
    public class OpenQuote
    {
        private List<Shape> shapes = new List<Shape>();
        private List<Cost> costs = new List<Cost>();

        public int QuoteID { get; set; }
        public string Name { get; set; } //After Review
        public string OrderNum { get; set; } //After Review
        public List<Shape> Shapes //Before Review
        {
            get { return shapes; }
        }
        public decimal TotalCost { get; set; }
        public List<Cost> Costs //Before Review
        {
            get { return costs; }
        }
        public DateTime DateQuoted { get; set; } //Before Review
        public bool PickedUp { get; set; } //After Review
        public bool Open { get; set; } //After Review
        public bool? AddSetup { get; set; } //After Review
        public decimal Discount { get; set; } //After Review
    }
}
