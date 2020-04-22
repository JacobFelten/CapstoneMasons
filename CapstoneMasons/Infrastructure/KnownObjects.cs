using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.Infrastructure
{
    public static class KnownObjects
    {
        public static List<string> NumberPrefix = new List<string> { "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th", "10th", "11th", "12th", "13th" };

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


        public static Cost Bar3GlobalCost = new Cost
        {
            Name = "Bar3GlobalCost"
        };
        public static Cost Bar3BendCost = new Cost
        {
            Name = "Bar3BendCost"
        };
        public static Cost Bar3CutCost = new Cost
        {
            Name = "Bar3CutCost"
        };


        public static Cost Bar4GlobalCost = new Cost
        {
            Name = "Bar4GlobalCost"
        };
        public static Cost Bar4BendCost = new Cost
        {
            Name = "Bar4BendCost"
        };
        public static Cost Bar4CutCost = new Cost
        {
            Name = "Bar4CutCost"
        };


        public static Cost Bar5GlobalCost = new Cost
        {
            Name = "Bar5GlobalCost"
        };
        public static Cost Bar5BendCost = new Cost
        {
            Name = "Bar5BendCost"
        };
        public static Cost Bar5CutCost = new Cost
        {
            Name = "Bar5CutCost"
        };


        public static Cost Bar6GlobalCost = new Cost
        {
            Name = "Bar6GlobalCost"
        };
        public static Cost Bar6BendCost = new Cost
        {
            Name = "Bar6BendCost"
        };
        public static Cost Bar6CutCost = new Cost
        {
            Name = "Bar6CutCost"
        };


        public static Cost SetupCharge = new Cost
        {
            Name = "SetupCharge"
        };
        public static Cost MinimumOrderCost = new Cost
        {
            Name = "MinimumOrderCost"
        };
    }
}
