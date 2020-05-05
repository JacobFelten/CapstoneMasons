using CapstoneMasons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.ViewModels
{
    public class DeleteQuote
    {
        public Quote Quote { get; set; }
        public int QuoteID { get; set; }
        public string ReturnUrl { get; set; }
    }
}
