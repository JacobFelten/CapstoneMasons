using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.ViewModels
{
    public class OpenQuote
    {
        public List<ReviewQuote> ReviewQuotes { get; set; }
        public string SearchBar { get; set; }
        public string Sort { get; set; }
    }
}
