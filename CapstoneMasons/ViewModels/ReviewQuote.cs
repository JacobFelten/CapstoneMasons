using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.ViewModels
{
    public class ReviewQuote
    {
        public int QuoteID { get; set; }
        public string Name { get; set; }
        public string OrderNum { get; set; }
        public int TotalCost { get; set; }
        public List<UsedBar> BarsUsed { get; set; }
        public int SetUpCharge { get; set; }
        public List<RemnantList> FinalRemnants { get; set; }
        public List<ReviewShape> Shapes { get; set; }
    }
}
