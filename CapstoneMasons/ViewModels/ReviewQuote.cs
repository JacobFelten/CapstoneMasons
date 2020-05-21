using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.ViewModels
{
    public class ReviewQuote
    {
        public int QuoteID { get; set; }
        public bool Update { get; set; }
        public string Author { get; set; }
        public DateTime DateQuoted { get; set; }
        public string Name { get; set; }
        public string OrderNum { get; set; }
        public bool? AddSetup { get; set; }
        public bool UseFormulas { get; set; }
        public bool PickedUp { get; set; }
        public decimal QtyLeft { get; set; }
        public List<Formula> NeededFormulas { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalCost { get; set; }
        public List<UsedBar> BarsUsed { get; set; }
        public decimal SetUpCharge { get; set; }
        public List<RemnantList> FinalRemnants { get; set; }
        public List<ReviewShape> Shapes { get; set; }
    }
}
