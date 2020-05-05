using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.ViewModels
{
    public class GlobalCosts
    {
        /* Costs pertaining to Bar 3 */
        public Cost Bar3GlobalCost { get; set; }
        public Cost Bar3BendCost { get; set; }
        public Cost Bar3CutCost { get; set; }

        /* Costs pertaining to Bar 4 */
        public Cost Bar4GlobalCost { get; set; }
        public Cost Bar4BendCost { get; set; }
        public Cost Bar4CutCost { get; set; }

        /* Costs pertaining to Bar 5 */
        public Cost Bar5GlobalCost { get; set; }
        public Cost Bar5BendCost { get; set; }
        public Cost Bar5CutCost { get; set; }

        /* Costs pertaining to Bar 6 */
        public Cost Bar6GlobalCost { get; set; }
        public Cost Bar6BendCost { get; set; }
        public Cost Bar6CutCost { get; set; }

        /*Costs pertaining to setup fees and minimum cost amounts */
        public Cost SetupCharge { get; set; }
    }
}
