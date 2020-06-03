using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;
using Org.BouncyCastle.Asn1.Mozilla;

namespace CapstoneMasons.Infrastructure
{
    public static class KnownObjects
    {
        public static List<string> NumberPrefix = new List<string> { "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th", "10th", 
                                                                    "11th", "12th", "13th", "14th", "15th", "16th", "17th", "18th", "19th", "20th", 
                                                                    "21st", "22nd", "23rd", "24th", "25th", "26th", "27th", "28th", "29th", "30th", 
                                                                    "31st", "32nd", "33rd", "34th", "35th", "36th", "37th", "38th", "39th", "40th", 
                                                                    "41st", "42nd", "43rd", "44th", "45th", "46th", "47th", "48th", "49th", "50th", 
                                                                    "51st", "52nd", "53rd", "54th", "55th", "56th", "57th", "58th", "59th", "60th",
                                                                    "61st", "62nd", "63rd", "64th", "65th", "66th", "67th", "68th", "69th", "70th",
                                                                    "71st", "72nd", "73rd", "74th", "75th", "76th", "77th", "78th", "79th", "80th",
                                                                    "81st", "82nd", "83rd", "84th", "85th", "86th", "87th", "88th", "89th", "90th",
                                                                    "91st", "92nd", "93rd", "94th", "95th", "96th", "97th", "98th", "99th", "100th"};

        public static int Precision = 2;

        public static int RoundToNth = 4;

        public static List<int> ValidRebarSizes = new List<int> { 3, 4, 5, 6 };

        public static decimal FullBarLength = 240;

        public static string FullBar = "Bar";

        public static string Remnant = "Remnant";

        public static Mandrel NoneMandrel = new Mandrel
        {
            Name = "None",
            Radius = 0.5m
        };

        public static Mandrel SmallMandrel = new Mandrel
        {
            Name = "Small",
            Radius = 1
        };

        public static Mandrel MediumMandrel = new Mandrel
        {
            Name = "Medium",
            Radius = 1.5m
        };

        public static Mandrel LargeMandrel = new Mandrel
        {
            Name = "Large",
            Radius = 2.25m
        };

        public static string BarCost = "Bar";
        public static string CutCost = "Cut";
        public static string BendCost = "Bend";
        public static string SetupCost = "Setup";

        public static string GlobalKeyWord = "Cost";

        public static Cost Bar3GlobalCost = new Cost
        {
            Name = "Bar3Global" + GlobalKeyWord
        };
        public static Cost Bar3BendCost = new Cost
        {
            Name = "Bar3Bend" + GlobalKeyWord
        };
        public static Cost Bar3CutCost = new Cost
        {
            Name = "Bar3Cut" + GlobalKeyWord
        };


        public static Cost Bar4GlobalCost = new Cost
        {
            Name = "Bar4Global" + GlobalKeyWord
        };
        public static Cost Bar4BendCost = new Cost
        {
            Name = "Bar4Bend" + GlobalKeyWord
        };
        public static Cost Bar4CutCost = new Cost
        {
            Name = "Bar4Cut" + GlobalKeyWord
        };


        public static Cost Bar5GlobalCost = new Cost
        {
            Name = "Bar5Global" + GlobalKeyWord
        };
        public static Cost Bar5BendCost = new Cost
        {
            Name = "Bar5Bend" + GlobalKeyWord
        };
        public static Cost Bar5CutCost = new Cost
        {
            Name = "Bar5Cut" + GlobalKeyWord
        };


        public static Cost Bar6GlobalCost = new Cost
        {
            Name = "Bar6Global" + GlobalKeyWord
        };
        public static Cost Bar6BendCost = new Cost
        {
            Name = "Bar6Bend" + GlobalKeyWord
        };
        public static Cost Bar6CutCost = new Cost
        {
            Name = "Bar6Cut" + GlobalKeyWord
        };


        public static Cost SetupCharge = new Cost
        {
            Name = "SetupCharge" + GlobalKeyWord
        };
    }
}
