using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.ViewModels
{
    public class OpenQuote
    {
        private List<ReviewQuote> reviewquotes = new List<ReviewQuote>();
        public List<ReviewQuote> ReviewQuotes { get { return reviewquotes; } }
        public string SearchBar { get; set; }
        public string Sort { get; set; }
    }
}
