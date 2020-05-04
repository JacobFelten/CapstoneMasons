using CapstoneMasons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.ViewModels
{
    public class DeleteShape
    {
        public Quote Quote { get; set; }
        public int QuoteID { get; set; }
        public Shape Shape { get; set; }
        public int ShapeID { get; set; }
        public string ReturnUrl { get; set; } //This should just be the name of a method in the QuotesController
    }
}
