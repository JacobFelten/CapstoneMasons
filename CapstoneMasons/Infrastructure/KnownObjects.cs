using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.Infrastructure
{
    public static class KnownObjects
    {
        public static List<int> ValidRebarSizes = new List<int> { 3, 4, 5, 6 };

        public static Mandrel NoneMandrel = new Mandrel
        {
            Name = "None",
            Radius = 1
        };

        public static Mandrel SmallMandrel = new Mandrel
        {
            Name = "Small",
            Radius = 2
        };

        public static Mandrel MediumMandrel = new Mandrel
        {
            Name = "Medium",
            Radius = 3
        };

        public static Mandrel LargeMandrel = new Mandrel
        {
            Name = "Large",
            Radius = 4
        };
    }
}
