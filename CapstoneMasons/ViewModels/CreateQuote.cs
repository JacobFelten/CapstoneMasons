using CapstoneMasons.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneMasons.ViewModels
{
    public class CreateQuote
    {
        [Required(ErrorMessage = "The name of the Quote is required")]
        public string Name { get; set; }
        //[Required] ask
        public string OrderNum { get; set; }
        //[Required] ask
        public string Author { get; set; }
        [Required(ErrorMessage = "The number of Shapes is required")]
        public int ShapesCount { get; set; }
        [Required(ErrorMessage = "Incomplete Leg Numbers")]
        public List<int> LegsInShapes { get; set; }
        [Required]
        public bool UseFormulas { get; set; }
        public DateTime DateQuoted { get; set; }
    }
}
