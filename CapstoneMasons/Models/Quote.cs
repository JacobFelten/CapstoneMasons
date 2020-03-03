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
        public string Name { get; set; }
        public string OrderNum { get; set; }
        public List<Shape> Shapes
        {
            get { return shapes; }
        }
        public List<Cost> Costs
        {
            get { return costs; }
        }
        public DateTime DateQuoted { get; set; }
        public bool PickedUp { get; set; }
        public bool Open { get; set; }
    }
}
