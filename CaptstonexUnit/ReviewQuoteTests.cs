using System;
using Xunit;
using CapstoneMasons.Models;
using CapstoneMasons.Repositories;
using CapstoneMasons.Controllers;
using CapstoneMasons.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CapstoneMasons.Infrastructure;
using Microsoft.CodeAnalysis.CSharp;

namespace CaptstonexUnit
{
    public class ReviewQuoteTests
    {
        private QuotesController controllerQ;
        private FakeShapeRepository repoS;
        private FakeQuoteRepository repoQ;
        private FakeFormulaRepository repoF;
        private FakeCostRepository repoC;
        private Quote quote;
        private Quote quote2;
        private Shape shape;
        private Shape shape2;
        private Shape shape3;
        private Shape shape4;
        private Leg leg1;
        private Leg leg2;
        private Leg leg3;
        private Leg leg4;
        private Leg leg5;
        private Leg leg6;
        private Leg leg7;
        private Mandrel mandrel1;
        private Mandrel mandrel2;
        private Cost cost1;
        private Cost cost2;
        private Cost cost3;
        private Cost cost4;
        private Cost cost5;
        private Cost cost6;
        private Cost cost7;
        private Cost cost8;
        private Cost cost9;
        private Cost cost10;
        private Cost cost11;
        private Cost cost12;
        private Cost cost13;
        private Cost cost14;
        private Cost cost15;
        private Cost cost16;
        private Cost cost17;
        private Cost cost18;
        private Cost cost19;
        private Cost cost20;
        private CreateShape cS;

        public ReviewQuoteTests ()
        {
            repoS = new FakeShapeRepository();
            quote = new Quote
            {
                QuoteID = 1
            };
            repoS.Quotes.Add(quote);
            shape = new Shape
            {
                ShapeID = 1,
                BarSize = 4,
                LegCount = 3,
                Qty = 30,
                NumCompleted = 0
            };
            cS = new CreateShape
            {
                Shape = shape,
                QuoteID = quote.QuoteID
            };

            repoQ = new FakeQuoteRepository();
            repoF = new FakeFormulaRepository();
            repoC = new FakeCostRepository();
            controllerQ = new QuotesController(repoQ, repoF, repoC, repoS);
            #region Mandrels
            mandrel1 = new Mandrel
            {
                MandrelID = 1,
                Name = "Small",
                Radius = 1
            };
            mandrel2 = new Mandrel
            {
                MandrelID = 2,
                Name = "Medium",
                Radius = 1.5m
            };
            #endregion
            #region Legs
            leg1 = new Leg
            {
                LegID = 1,
                Length = 36,
                Degree = 90,
                Mandrel = mandrel2
            };
            leg2 = new Leg
            {
                LegID = 2,
                Length = 36,
                Degree = 0,
                Mandrel = null
            };
            leg3 = new Leg
            {
                LegID = 3,
                Length = 12,
                Degree = 90,
                Mandrel = mandrel2
            };
            leg4 = new Leg
            {
                LegID = 4,
                Length = 12,
                Degree = 0,
                Mandrel = null
            };
            leg5 = new Leg
            {
                LegID = 5,
                Length = 30,
                Degree = 90,
                Mandrel = mandrel1
            };
            leg6 = new Leg
            {
                LegID = 6,
                Length = 18,
                Degree = 90,
                Mandrel = mandrel1
            };
            leg7 = new Leg
            {
                LegID = 7,
                Length = 30,
                Degree = 0,
                Mandrel = null
            };
            #endregion
            #region Shapes
            shape2 = new Shape
            {
                ShapeID = 2,
                BarSize = 5,
                LegCount = 2,
                Legs = { leg1, leg2 },
                Qty = 30,
                NumCompleted = 0
            };
            shape3 = new Shape
            {
                ShapeID = 3,
                BarSize = 5,
                LegCount = 2,
                Legs = { leg3, leg4 },
                Qty = 60,
                NumCompleted = 0
            };
            shape4 = new Shape
            {
                ShapeID = 4,
                BarSize = 4,
                LegCount = 3,
                Legs = { leg5, leg6, leg7 },
                Qty = 40,
                NumCompleted = 0
            };
            #endregion
            #region Costs
            cost1 = new Cost
            {
                CostID = 1,
                Name = KnownObjects.Bar4GlobalCost.Name,
                Price = 10,
                LastChanged = new DateTime()
            };
            cost2 = new Cost
            {
                CostID = 2,
                Name = KnownObjects.Bar4CutCost.Name,
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost3 = new Cost
            {
                CostID = 3,
                Name = KnownObjects.Bar4BendCost.Name,
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost4 = new Cost
            {
                CostID = 4,
                Name = KnownObjects.Bar5GlobalCost.Name,
                Price = 15,
                LastChanged = new DateTime()
            };
            cost5 = new Cost
            {
                CostID = 5,
                Name = KnownObjects.Bar5CutCost.Name,
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            cost6 = new Cost
            {
                CostID = 6,
                Name = KnownObjects.Bar5BendCost.Name,
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            cost7 = new Cost
            {
                CostID = 7,
                Name = KnownObjects.SetupCharge.Name,
                Price = 15,
                LastChanged = new DateTime()
            };
            cost8 = new Cost
            {
                CostID = 8,
                Name = "4Bar",
                Price = 10,
                LastChanged = new DateTime()
            };
            cost9 = new Cost
            {
                CostID = 9,
                Name = "4Cut",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost10 = new Cost
            {
                CostID = 10,
                Name = "4Bend",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost11 = new Cost
            {
                CostID = 11,
                Name = "5Bar",
                Price = 15,
                LastChanged = new DateTime()
            };
            cost12 = new Cost
            {
                CostID = 12,
                Name = "5Cut",
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            cost13 = new Cost
            {
                CostID = 13,
                Name = "5Bend",
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            cost14 = new Cost
            {
                CostID = 14,
                Name = "Setup",
                Price = 15,
                LastChanged = new DateTime()
            };
            #endregion
            #region Extra Costs
            cost15 = new Cost
            {
                CostID = 15,
                Name = KnownObjects.Bar3GlobalCost.Name,
                Price = 10,
                LastChanged = new DateTime()
            };
            cost16 = new Cost
            {
                CostID = 16,
                Name = KnownObjects.Bar3CutCost.Name,
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost17 = new Cost
            {
                CostID = 17,
                Name = KnownObjects.Bar3BendCost.Name,
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost18 = new Cost
            {
                CostID = 18,
                Name = KnownObjects.Bar6GlobalCost.Name,
                Price = 15,
                LastChanged = new DateTime()
            };
            cost19 = new Cost
            {
                CostID = 19,
                Name = KnownObjects.Bar6CutCost.Name,
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            cost20 = new Cost
            {
                CostID = 20,
                Name = KnownObjects.Bar6BendCost.Name,
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            #endregion
            quote2 = new Quote
            {
                QuoteID = 2,
                Name = "Bob's Concrete",
                OrderNum = "123456",
                UseFormulas = true,
                Shapes = { shape2, shape3, shape4 },
                Costs = { cost8, cost9, cost10, cost11, cost12, cost13, cost14 },
                DateQuoted = DateTime.Now,
                PickedUp = false,
                Open = false
            };
            repoC.AddCostAsync(cost1);
            repoC.AddCostAsync(cost2);
            repoC.AddCostAsync(cost3);
            repoC.AddCostAsync(cost4);
            repoC.AddCostAsync(cost5);
            repoC.AddCostAsync(cost6);
            repoC.AddCostAsync(cost7);
            repoC.AddCostAsync(cost15);
            repoC.AddCostAsync(cost16);
            repoC.AddCostAsync(cost17);
            repoC.AddCostAsync(cost18);
            repoC.AddCostAsync(cost19);
            repoC.AddCostAsync(cost20);
        }

        [Fact]
        public async Task ReviewQuoteTest()
        {
            //Arrange
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 1,
                BarSize = 4,
                Degree = 90,
                Mandrel = mandrel1,
                PinNumber = "16.5",
                InGained = 1.5m,
                LastChanged = DateTime.Now
            });
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 2,
                BarSize = 5,
                Degree = 90,
                Mandrel = mandrel2,
                PinNumber = "15",
                InGained = 2,
                LastChanged = DateTime.Now
            });
            await repoQ.AddQuoteAsync(quote2);

            //Act
            ViewResult view = (ViewResult)await controllerQ.ReviewQuote(quote2.QuoteID);
            ReviewQuote rQ = (ReviewQuote)view.Model;

            //Assert
            Assert.Equal(2, rQ.QuoteID);

            Assert.Equal("Bob's Concrete", rQ.Name);

            Assert.Equal("123456", rQ.OrderNum);

            Assert.Equal(469.4m, rQ.TotalCost);

            Assert.Equal(2, rQ.BarsUsed.Count);
                Assert.Equal(4, rQ.BarsUsed[0].BarSize);
                Assert.Equal(14, rQ.BarsUsed[0].NumOfBars);
                Assert.Equal(10, rQ.BarsUsed[0].BarCost);
                Assert.Equal(40, rQ.BarsUsed[0].NumOfCuts);
                Assert.Equal(0.25m, rQ.BarsUsed[0].CutCost);
                Assert.Equal(80, rQ.BarsUsed[0].NumOfBends);
                Assert.Equal(0.25m, rQ.BarsUsed[0].BendCost);
                Assert.Equal(5, rQ.BarsUsed[1].BarSize);
                Assert.Equal(15, rQ.BarsUsed[1].NumOfBars);
                Assert.Equal(15, rQ.BarsUsed[1].BarCost);
                Assert.Equal(90, rQ.BarsUsed[1].NumOfCuts);
                Assert.Equal(0.33m, rQ.BarsUsed[1].CutCost);
                Assert.Equal(90, rQ.BarsUsed[1].NumOfBends);
                Assert.Equal(0.33m, rQ.BarsUsed[1].BendCost);

            Assert.Equal(15, rQ.SetUpCharge);
            
            Assert.Equal(2, rQ.FinalRemnants.Count);
                Assert.Equal(4, rQ.FinalRemnants[0].BarSize);
                Assert.Equal(2, rQ.FinalRemnants[0].Remnants.Count);
                    Assert.Equal(15, rQ.FinalRemnants[0].Remnants[0].Length);
                    Assert.Equal(13, rQ.FinalRemnants[0].Remnants[0].Qty);
                    Assert.False(rQ.FinalRemnants[0].Remnants[0].UsedAgain);
                    Assert.Equal(165, rQ.FinalRemnants[0].Remnants[1].Length);
                    Assert.Equal(1, rQ.FinalRemnants[0].Remnants[1].Qty);
                    Assert.False(rQ.FinalRemnants[0].Remnants[1].UsedAgain);
                Assert.Equal(2, rQ.FinalRemnants[1].Remnants.Count);
                    Assert.Equal(8, rQ.FinalRemnants[1].Remnants[0].Length);
                    Assert.Equal(10, rQ.FinalRemnants[1].Remnants[0].Qty);
                    Assert.False(rQ.FinalRemnants[1].Remnants[0].UsedAgain);
                    Assert.Equal(20, rQ.FinalRemnants[1].Remnants[1].Length);
                    Assert.Equal(5, rQ.FinalRemnants[1].Remnants[1].Qty);
                    Assert.False(rQ.FinalRemnants[1].Remnants[1].UsedAgain);

            Assert.Equal(3, rQ.Shapes.Count);
                Assert.Equal(4, rQ.Shapes[0].ShapeID);
                Assert.Equal(40, rQ.Shapes[0].Qty);
                Assert.Equal(4, rQ.Shapes[0].BarSize);
                Assert.Equal(14, rQ.Shapes[0].NumOfBars);
                Assert.Equal(75, rQ.Shapes[0].CutLength);
                Assert.Equal(2, rQ.Shapes[0].Instructions.Count);
                    Assert.Equal(3, rQ.Shapes[0].Instructions[0].CutQty);
                    Assert.Equal(240, rQ.Shapes[0].Instructions[0].PerLength);
                    Assert.Equal("Bar", rQ.Shapes[0].Instructions[0].PerType);
                    Assert.Equal(13, rQ.Shapes[0].Instructions[0].ForQty);
                    Assert.Equal(1, rQ.Shapes[0].Instructions[1].CutQty);
                    Assert.Equal(240, rQ.Shapes[0].Instructions[1].PerLength);
                    Assert.Equal("Bar", rQ.Shapes[0].Instructions[1].PerType);
                    Assert.Equal(1, rQ.Shapes[0].Instructions[1].ForQty);
                Assert.Equal(3, rQ.Shapes[0].Legs.Count);
                    Assert.Equal(30, rQ.Shapes[0].Legs[0].Length);
                    Assert.Equal(90, rQ.Shapes[0].Legs[0].Degree);
                    Assert.Equal("Small", rQ.Shapes[0].Legs[0].Mandrel);
                    Assert.Equal("16.5", rQ.Shapes[0].Legs[0].PinNumber);
                    Assert.Equal(1.5m, rQ.Shapes[0].Legs[0].InGained);
                    Assert.Equal(18, rQ.Shapes[0].Legs[1].Length);
                    Assert.Equal(90, rQ.Shapes[0].Legs[1].Degree);
                    Assert.Equal("Small", rQ.Shapes[0].Legs[1].Mandrel);
                    Assert.Equal("16.5", rQ.Shapes[0].Legs[1].PinNumber);
                    Assert.Equal(1.5m, rQ.Shapes[0].Legs[1].InGained);
                    Assert.Equal(30, rQ.Shapes[0].Legs[2].Length);
                    Assert.Equal(0, rQ.Shapes[0].Legs[2].Degree);
                    Assert.Equal("", rQ.Shapes[0].Legs[2].Mandrel);
                    Assert.Equal("", rQ.Shapes[0].Legs[2].PinNumber);
                    Assert.Equal(0, rQ.Shapes[0].Legs[2].InGained);
                Assert.Equal(2, rQ.Shapes[0].Remnants.Count);
                    Assert.Equal(15, rQ.Shapes[0].Remnants[0].Length);
                    Assert.Equal(13, rQ.Shapes[0].Remnants[0].Qty);
                    Assert.False(rQ.Shapes[0].Remnants[0].UsedAgain);
                    Assert.Equal(165, rQ.Shapes[0].Remnants[1].Length);
                    Assert.Equal(1, rQ.Shapes[0].Remnants[1].Qty);
                    Assert.False(rQ.Shapes[0].Remnants[1].UsedAgain);
                Assert.Equal(2, rQ.Shapes[1].ShapeID);
                Assert.Equal(30, rQ.Shapes[1].Qty);
                Assert.Equal(5, rQ.Shapes[1].BarSize);
                Assert.Equal(10, rQ.Shapes[1].NumOfBars);
                Assert.Equal(70, rQ.Shapes[1].CutLength);
                Assert.Single(rQ.Shapes[1].Instructions);
                    Assert.Equal(3, rQ.Shapes[1].Instructions[0].CutQty);
                    Assert.Equal(240, rQ.Shapes[1].Instructions[0].PerLength);
                    Assert.Equal("Bar", rQ.Shapes[1].Instructions[0].PerType);
                    Assert.Equal(10, rQ.Shapes[1].Instructions[0].ForQty);
                Assert.Equal(2, rQ.Shapes[1].Legs.Count);
                    Assert.Equal(36, rQ.Shapes[1].Legs[0].Length);
                    Assert.Equal(90, rQ.Shapes[1].Legs[0].Degree);
                    Assert.Equal("Medium", rQ.Shapes[1].Legs[0].Mandrel);
                    Assert.Equal("15", rQ.Shapes[1].Legs[0].PinNumber);
                    Assert.Equal(2, rQ.Shapes[1].Legs[0].InGained);
                    Assert.Equal(36, rQ.Shapes[1].Legs[1].Length);
                    Assert.Equal(0, rQ.Shapes[1].Legs[1].Degree);
                    Assert.Equal("", rQ.Shapes[1].Legs[1].Mandrel);
                    Assert.Equal("", rQ.Shapes[1].Legs[1].PinNumber);
                    Assert.Equal(0, rQ.Shapes[1].Legs[1].InGained);
                Assert.Single(rQ.Shapes[1].Remnants);
                    Assert.Equal(30, rQ.Shapes[1].Remnants[0].Length);
                    Assert.Equal(10, rQ.Shapes[1].Remnants[0].Qty);
                    Assert.True(rQ.Shapes[1].Remnants[0].UsedAgain);
                Assert.Equal(3, rQ.Shapes[2].ShapeID);
                Assert.Equal(60, rQ.Shapes[2].Qty);
                Assert.Equal(5, rQ.Shapes[2].BarSize);
                Assert.Equal(5, rQ.Shapes[2].NumOfBars);
                Assert.Equal(22, rQ.Shapes[2].CutLength);
                Assert.Equal(2, rQ.Shapes[2].Instructions.Count);
                    Assert.Equal(1, rQ.Shapes[2].Instructions[0].CutQty);
                    Assert.Equal(30, rQ.Shapes[2].Instructions[0].PerLength);
                    Assert.Equal("Remnant", rQ.Shapes[2].Instructions[0].PerType);
                    Assert.Equal(10, rQ.Shapes[2].Instructions[0].ForQty);
                    Assert.Equal(10, rQ.Shapes[2].Instructions[1].CutQty);
                    Assert.Equal(240, rQ.Shapes[2].Instructions[1].PerLength);
                    Assert.Equal("Bar", rQ.Shapes[2].Instructions[1].PerType);
                    Assert.Equal(5, rQ.Shapes[2].Instructions[1].ForQty);
                Assert.Equal(2, rQ.Shapes[2].Legs.Count);
                    Assert.Equal(12, rQ.Shapes[2].Legs[0].Length);
                    Assert.Equal(90, rQ.Shapes[2].Legs[0].Degree);
                    Assert.Equal("Medium", rQ.Shapes[2].Legs[0].Mandrel);
                    Assert.Equal("15", rQ.Shapes[2].Legs[0].PinNumber);
                    Assert.Equal(2, rQ.Shapes[2].Legs[0].InGained);
                    Assert.Equal(12, rQ.Shapes[2].Legs[1].Length);
                    Assert.Equal(0, rQ.Shapes[2].Legs[1].Degree);
                    Assert.Equal("", rQ.Shapes[2].Legs[1].Mandrel);
                    Assert.Equal("", rQ.Shapes[2].Legs[1].PinNumber);
                    Assert.Equal(0, rQ.Shapes[2].Legs[1].InGained);
                Assert.Equal(2, rQ.Shapes[2].Remnants.Count);
                    Assert.Equal(8, rQ.Shapes[2].Remnants[0].Length);
                    Assert.Equal(10, rQ.Shapes[2].Remnants[0].Qty);
                    Assert.False(rQ.Shapes[2].Remnants[0].UsedAgain);
                    Assert.Equal(20, rQ.Shapes[2].Remnants[1].Length);
                    Assert.Equal(5, rQ.Shapes[2].Remnants[1].Qty);
                    Assert.False(rQ.Shapes[2].Remnants[1].UsedAgain);
        }

        [Fact]
        public async Task ReviewQuotePostTest()
        {
            //Arrange
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 1,
                BarSize = 4,
                Degree = 90,
                Mandrel = mandrel1,
                PinNumber = "16.5",
                InGained = 1.5m,
                LastChanged = DateTime.Now
            });
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 2,
                BarSize = 5,
                Degree = 90,
                Mandrel = mandrel2,
                PinNumber = "15",
                InGained = 2,
                LastChanged = DateTime.Now
            });
            await repoQ.AddQuoteAsync(quote2);

            //Act
            ViewResult view = (ViewResult)await controllerQ.ReviewQuote(2, "Billy's Order", "987654", 54.4m, "false");
            ReviewQuote rQ = (ReviewQuote)view.Model;

            //Assert
            Assert.Equal(2, rQ.QuoteID);

            Assert.Equal("Billy's Order", rQ.Name);

            Assert.Equal("987654", rQ.OrderNum);

            Assert.Equal(400m, rQ.TotalCost);

            Assert.Equal(0, rQ.SetUpCharge);
            Assert.Equal(54.4m, rQ.Discount);
        }

        [Fact]
        public async Task ReviewQuoteSetupTrueTest()
        {
            //Arrange
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 1,
                BarSize = 4,
                Degree = 90,
                Mandrel = mandrel1,
                PinNumber = "16.5",
                InGained = 1.5m,
                LastChanged = DateTime.Now
            });
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 2,
                BarSize = 5,
                Degree = 90,
                Mandrel = mandrel2,
                PinNumber = "15",
                InGained = 2,
                LastChanged = DateTime.Now
            });
            await repoQ.AddQuoteAsync(quote2);

            //Act
            ViewResult view = (ViewResult)await controllerQ.ReviewQuote(2, "Billy's Order", "987654", 69.4m, "true");
            ReviewQuote rQ = (ReviewQuote)view.Model;

            //Assert
            Assert.Equal(2, rQ.QuoteID);

            Assert.Equal("Billy's Order", rQ.Name);

            Assert.Equal("987654", rQ.OrderNum);

            Assert.Equal(400m, rQ.TotalCost);

            Assert.Equal(15, rQ.SetUpCharge);
            Assert.Equal(69.4m, rQ.Discount);
        }

        [Fact]
        public async Task DeleteShapeTest()
        {
            //Arrange
            await repoQ.AddQuoteAsync(quote2);
            (await repoS.Shapes).Add(shape2);

            //Act
            ViewResult view = (ViewResult)await controllerQ.DeleteShape(2, 2, "ReviewOpen");
            DeleteShape dS = (DeleteShape)view.Model;

            //Assert
            Assert.Equal(quote2, dS.Quote);
            Assert.Equal(shape2, dS.Shape);
            Assert.Equal("ReviewOpen", dS.ReturnUrl);
        }

        [Fact]
        public async Task DeleteShapePostTest()
        {
            //Arrange
            DeleteShape dS = new DeleteShape
            {
                QuoteID = 2,
                ShapeID = 2,
                ReturnUrl = "ReviewOpen"
            };
            await repoQ.AddQuoteAsync(quote2);
            (await repoS.Shapes).Add(shape2);
            int shapeAmt = (await repoS.Shapes).Count;

            //Act
            await controllerQ.DeleteShape(dS);

            //Assert
            Assert.Equal(shapeAmt - 1, (await repoS.Shapes).Count);
        }

        [Fact]
        public async Task ReviewOpenTest()
        {
            //Arrange
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 1,
                BarSize = 4,
                Degree = 90,
                Mandrel = mandrel1,
                PinNumber = "16.5",
                InGained = 1.5m,
                LastChanged = DateTime.Now
            });
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 2,
                BarSize = 5,
                Degree = 90,
                Mandrel = mandrel2,
                PinNumber = "15",
                InGained = 2,
                LastChanged = DateTime.Now
            });
            await repoQ.AddQuoteAsync(quote2);

            //Act
            ViewResult view = (ViewResult)await controllerQ.ReviewOpen(quote2.QuoteID);
            ReviewOpen rO = (ReviewOpen)view.Model;
            ReviewQuote rQ = rO.ReviewQuote;

            //Assert
            Assert.Equal(2, rQ.QuoteID);

            Assert.Equal("Bob's Concrete", rQ.Name);

            Assert.Equal("123456", rQ.OrderNum);

            Assert.Equal(469.4m, rQ.TotalCost);

            Assert.Equal(2, rQ.BarsUsed.Count);
            Assert.Equal(4, rQ.BarsUsed[0].BarSize);
            Assert.Equal(14, rQ.BarsUsed[0].NumOfBars);
            Assert.Equal(10, rQ.BarsUsed[0].BarCost);
            Assert.Equal(40, rQ.BarsUsed[0].NumOfCuts);
            Assert.Equal(0.25m, rQ.BarsUsed[0].CutCost);
            Assert.Equal(80, rQ.BarsUsed[0].NumOfBends);
            Assert.Equal(0.25m, rQ.BarsUsed[0].BendCost);
            Assert.Equal(5, rQ.BarsUsed[1].BarSize);
            Assert.Equal(15, rQ.BarsUsed[1].NumOfBars);
            Assert.Equal(15, rQ.BarsUsed[1].BarCost);
            Assert.Equal(90, rQ.BarsUsed[1].NumOfCuts);
            Assert.Equal(0.33m, rQ.BarsUsed[1].CutCost);
            Assert.Equal(90, rQ.BarsUsed[1].NumOfBends);
            Assert.Equal(0.33m, rQ.BarsUsed[1].BendCost);

            Assert.Equal(15, rQ.SetUpCharge);

            Assert.Equal(2, rQ.FinalRemnants.Count);
            Assert.Equal(4, rQ.FinalRemnants[0].BarSize);
            Assert.Equal(2, rQ.FinalRemnants[0].Remnants.Count);
            Assert.Equal(15, rQ.FinalRemnants[0].Remnants[0].Length);
            Assert.Equal(13, rQ.FinalRemnants[0].Remnants[0].Qty);
            Assert.False(rQ.FinalRemnants[0].Remnants[0].UsedAgain);
            Assert.Equal(165, rQ.FinalRemnants[0].Remnants[1].Length);
            Assert.Equal(1, rQ.FinalRemnants[0].Remnants[1].Qty);
            Assert.False(rQ.FinalRemnants[0].Remnants[1].UsedAgain);
            Assert.Equal(2, rQ.FinalRemnants[1].Remnants.Count);
            Assert.Equal(8, rQ.FinalRemnants[1].Remnants[0].Length);
            Assert.Equal(10, rQ.FinalRemnants[1].Remnants[0].Qty);
            Assert.False(rQ.FinalRemnants[1].Remnants[0].UsedAgain);
            Assert.Equal(20, rQ.FinalRemnants[1].Remnants[1].Length);
            Assert.Equal(5, rQ.FinalRemnants[1].Remnants[1].Qty);
            Assert.False(rQ.FinalRemnants[1].Remnants[1].UsedAgain);

            Assert.Equal(3, rQ.Shapes.Count);
            Assert.Equal(4, rQ.Shapes[0].ShapeID);
            Assert.Equal(40, rQ.Shapes[0].Qty);
            Assert.Equal(4, rQ.Shapes[0].BarSize);
            Assert.Equal(14, rQ.Shapes[0].NumOfBars);
            Assert.Equal(75, rQ.Shapes[0].CutLength);
            Assert.Equal(2, rQ.Shapes[0].Instructions.Count);
            Assert.Equal(3, rQ.Shapes[0].Instructions[0].CutQty);
            Assert.Equal(240, rQ.Shapes[0].Instructions[0].PerLength);
            Assert.Equal("Bar", rQ.Shapes[0].Instructions[0].PerType);
            Assert.Equal(13, rQ.Shapes[0].Instructions[0].ForQty);
            Assert.Equal(1, rQ.Shapes[0].Instructions[1].CutQty);
            Assert.Equal(240, rQ.Shapes[0].Instructions[1].PerLength);
            Assert.Equal("Bar", rQ.Shapes[0].Instructions[1].PerType);
            Assert.Equal(1, rQ.Shapes[0].Instructions[1].ForQty);
            Assert.Equal(3, rQ.Shapes[0].Legs.Count);
            Assert.Equal(30, rQ.Shapes[0].Legs[0].Length);
            Assert.Equal(90, rQ.Shapes[0].Legs[0].Degree);
            Assert.Equal("Small", rQ.Shapes[0].Legs[0].Mandrel);
            Assert.Equal("16.5", rQ.Shapes[0].Legs[0].PinNumber);
            Assert.Equal(1.5m, rQ.Shapes[0].Legs[0].InGained);
            Assert.Equal(18, rQ.Shapes[0].Legs[1].Length);
            Assert.Equal(90, rQ.Shapes[0].Legs[1].Degree);
            Assert.Equal("Small", rQ.Shapes[0].Legs[1].Mandrel);
            Assert.Equal("16.5", rQ.Shapes[0].Legs[1].PinNumber);
            Assert.Equal(1.5m, rQ.Shapes[0].Legs[1].InGained);
            Assert.Equal(30, rQ.Shapes[0].Legs[2].Length);
            Assert.Equal(0, rQ.Shapes[0].Legs[2].Degree);
            Assert.Equal("", rQ.Shapes[0].Legs[2].Mandrel);
            Assert.Equal("", rQ.Shapes[0].Legs[2].PinNumber);
            Assert.Equal(0, rQ.Shapes[0].Legs[2].InGained);
            Assert.Equal(2, rQ.Shapes[0].Remnants.Count);
            Assert.Equal(15, rQ.Shapes[0].Remnants[0].Length);
            Assert.Equal(13, rQ.Shapes[0].Remnants[0].Qty);
            Assert.False(rQ.Shapes[0].Remnants[0].UsedAgain);
            Assert.Equal(165, rQ.Shapes[0].Remnants[1].Length);
            Assert.Equal(1, rQ.Shapes[0].Remnants[1].Qty);
            Assert.False(rQ.Shapes[0].Remnants[1].UsedAgain);
            Assert.Equal(2, rQ.Shapes[1].ShapeID);
            Assert.Equal(30, rQ.Shapes[1].Qty);
            Assert.Equal(5, rQ.Shapes[1].BarSize);
            Assert.Equal(10, rQ.Shapes[1].NumOfBars);
            Assert.Equal(70, rQ.Shapes[1].CutLength);
            Assert.Single(rQ.Shapes[1].Instructions);
            Assert.Equal(3, rQ.Shapes[1].Instructions[0].CutQty);
            Assert.Equal(240, rQ.Shapes[1].Instructions[0].PerLength);
            Assert.Equal("Bar", rQ.Shapes[1].Instructions[0].PerType);
            Assert.Equal(10, rQ.Shapes[1].Instructions[0].ForQty);
            Assert.Equal(2, rQ.Shapes[1].Legs.Count);
            Assert.Equal(36, rQ.Shapes[1].Legs[0].Length);
            Assert.Equal(90, rQ.Shapes[1].Legs[0].Degree);
            Assert.Equal("Medium", rQ.Shapes[1].Legs[0].Mandrel);
            Assert.Equal("15", rQ.Shapes[1].Legs[0].PinNumber);
            Assert.Equal(2, rQ.Shapes[1].Legs[0].InGained);
            Assert.Equal(36, rQ.Shapes[1].Legs[1].Length);
            Assert.Equal(0, rQ.Shapes[1].Legs[1].Degree);
            Assert.Equal("", rQ.Shapes[1].Legs[1].Mandrel);
            Assert.Equal("", rQ.Shapes[1].Legs[1].PinNumber);
            Assert.Equal(0, rQ.Shapes[1].Legs[1].InGained);
            Assert.Single(rQ.Shapes[1].Remnants);
            Assert.Equal(30, rQ.Shapes[1].Remnants[0].Length);
            Assert.Equal(10, rQ.Shapes[1].Remnants[0].Qty);
            Assert.True(rQ.Shapes[1].Remnants[0].UsedAgain);
            Assert.Equal(3, rQ.Shapes[2].ShapeID);
            Assert.Equal(60, rQ.Shapes[2].Qty);
            Assert.Equal(5, rQ.Shapes[2].BarSize);
            Assert.Equal(5, rQ.Shapes[2].NumOfBars);
            Assert.Equal(22, rQ.Shapes[2].CutLength);
            Assert.Equal(2, rQ.Shapes[2].Instructions.Count);
            Assert.Equal(1, rQ.Shapes[2].Instructions[0].CutQty);
            Assert.Equal(30, rQ.Shapes[2].Instructions[0].PerLength);
            Assert.Equal("Remnant", rQ.Shapes[2].Instructions[0].PerType);
            Assert.Equal(10, rQ.Shapes[2].Instructions[0].ForQty);
            Assert.Equal(10, rQ.Shapes[2].Instructions[1].CutQty);
            Assert.Equal(240, rQ.Shapes[2].Instructions[1].PerLength);
            Assert.Equal("Bar", rQ.Shapes[2].Instructions[1].PerType);
            Assert.Equal(5, rQ.Shapes[2].Instructions[1].ForQty);
            Assert.Equal(2, rQ.Shapes[2].Legs.Count);
            Assert.Equal(12, rQ.Shapes[2].Legs[0].Length);
            Assert.Equal(90, rQ.Shapes[2].Legs[0].Degree);
            Assert.Equal("Medium", rQ.Shapes[2].Legs[0].Mandrel);
            Assert.Equal("15", rQ.Shapes[2].Legs[0].PinNumber);
            Assert.Equal(2, rQ.Shapes[2].Legs[0].InGained);
            Assert.Equal(12, rQ.Shapes[2].Legs[1].Length);
            Assert.Equal(0, rQ.Shapes[2].Legs[1].Degree);
            Assert.Equal("", rQ.Shapes[2].Legs[1].Mandrel);
            Assert.Equal("", rQ.Shapes[2].Legs[1].PinNumber);
            Assert.Equal(0, rQ.Shapes[2].Legs[1].InGained);
            Assert.Equal(2, rQ.Shapes[2].Remnants.Count);
            Assert.Equal(8, rQ.Shapes[2].Remnants[0].Length);
            Assert.Equal(10, rQ.Shapes[2].Remnants[0].Qty);
            Assert.False(rQ.Shapes[2].Remnants[0].UsedAgain);
            Assert.Equal(20, rQ.Shapes[2].Remnants[1].Length);
            Assert.Equal(5, rQ.Shapes[2].Remnants[1].Qty);
            Assert.False(rQ.Shapes[2].Remnants[1].UsedAgain);
        }

        [Fact]
        public async Task ReviewOpenPostTest()
        {
            //Arrange
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 1,
                BarSize = 4,
                Degree = 90,
                Mandrel = mandrel1,
                PinNumber = "16.5",
                InGained = 1.5m,
                LastChanged = DateTime.Now
            });
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 2,
                BarSize = 5,
                Degree = 90,
                Mandrel = mandrel2,
                PinNumber = "15",
                InGained = 2,
                LastChanged = DateTime.Now
            });
            await repoQ.AddQuoteAsync(quote2);

            ReviewOpen rO = new ReviewOpen
            {
                QuoteID = 2,
                Name = "Billy's Order",
                OrderNumber = "987654",
                Discount = 69.4m,
                Setup = "true",
                Completed =  new int[3] { 20, 30, 40 }
            };

            (await repoS.Shapes).Add(shape2);
            (await repoS.Shapes).Add(shape3);
            (await repoS.Shapes).Add(shape4);

            //Act
            ViewResult view = (ViewResult)await controllerQ.ReviewOpen(rO, "true");
            ReviewOpen rQ = (ReviewOpen)view.Model;

            //Assert
            Assert.Equal(2, rO.ReviewQuote.QuoteID);
            Assert.Equal("Billy's Order", rO.ReviewQuote.Name);
            Assert.Equal("987654", rO.ReviewQuote.OrderNum);
            Assert.Equal(400m, rO.ReviewQuote.TotalCost);
            Assert.Equal(15, rO.ReviewQuote.SetUpCharge);
            Assert.Equal(69.4m, rO.ReviewQuote.Discount);
            Assert.Equal(20, rO.ReviewQuote.Shapes[0].Completed);
            Assert.Equal(30, rO.ReviewQuote.Shapes[1].Completed);
            Assert.Equal(40, rO.ReviewQuote.Shapes[2].Completed);
        }

        [Fact]
        public async Task UpdateQuotePricesTest()
        {
            //Arrange
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 1,
                BarSize = 4,
                Degree = 90,
                Mandrel = mandrel1,
                PinNumber = "16.5",
                InGained = 1.5m,
                LastChanged = DateTime.Now
            });
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 2,
                BarSize = 5,
                Degree = 90,
                Mandrel = mandrel2,
                PinNumber = "15",
                InGained = 2,
                LastChanged = DateTime.Now
            });
            await repoQ.AddQuoteAsync(quote2);
            cost1.Price = 20;
            cost2.Price = 0.5m;
            cost3.Price = 0.5m;
            cost4.Price = 30;
            cost5.Price = 0.66m;
            cost6.Price = 0.66m;
            cost7.Price = 30;

            //Act
            ViewResult view = (ViewResult)await controllerQ.UpdateQuotePrices(quote2.QuoteID);
            ReviewOpen rO = (ReviewOpen)view.Model;
            Quote q = await repoQ.GetQuoteByIdAsync(rO.ReviewQuote.QuoteID);

            //Assert
            Assert.Equal(13, q.Costs.Count);
            Assert.Equal(938.8m, rO.ReviewQuote.TotalCost);
            Assert.Equal(20, rO.ReviewQuote.BarsUsed[0].BarCost);
            Assert.Equal(0.5m, rO.ReviewQuote.BarsUsed[0].CutCost);
            Assert.Equal(0.5m, rO.ReviewQuote.BarsUsed[0].BendCost);
            Assert.Equal(30, rO.ReviewQuote.BarsUsed[1].BarCost);
            Assert.Equal(0.66m, rO.ReviewQuote.BarsUsed[1].CutCost);
            Assert.Equal(0.66m, rO.ReviewQuote.BarsUsed[1].BendCost);
            Assert.Equal(30, rO.ReviewQuote.SetUpCharge);
        }

        [Fact]
        public async Task CloseOpenQuoteTest()
        {
            //Arrange
            quote2.Open = true;
            await repoQ.AddQuoteAsync(quote2);

            //Act
            await controllerQ.CloseOpenQuote(2);

            //Assert
            Assert.False(quote2.Open);
        }

        [Fact]
        public async Task DeleteQuoteTest()
        {
            //Arrange 
            await repoQ.AddQuoteAsync(quote2);

            //Act
            ViewResult view = (ViewResult)await controllerQ.DeleteQuote(2, "Index");
            DeleteQuote dQ = (DeleteQuote)view.Model;

            //Assert
            Assert.Equal(quote2, dQ.Quote);
            Assert.Equal("Index", dQ.ReturnUrl);
        }

        [Fact]
        public async Task DeleteQuotePostTest()
        {
            //Arrange
            DeleteQuote dQ = new DeleteQuote
            {
                QuoteID = 2,
                ReturnUrl = "OpenQuote"
            };
            await repoQ.AddQuoteAsync(quote2);

            //Act
            await controllerQ.DeleteQuote(dQ);

            //Assert
            Assert.Null(await repoQ.GetQuoteByIdAsync(2));
        }

        [Fact]
        public async Task ReviewClosedTest()
        {
            //Arrange
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 1,
                BarSize = 4,
                Degree = 90,
                Mandrel = mandrel1,
                PinNumber = "16.5",
                InGained = 1.5m,
                LastChanged = DateTime.Now
            });
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 2,
                BarSize = 5,
                Degree = 90,
                Mandrel = mandrel2,
                PinNumber = "15",
                InGained = 2,
                LastChanged = DateTime.Now
            });
            await repoQ.AddQuoteAsync(quote2);

            //Act
            ViewResult view = (ViewResult)await controllerQ.ReviewClosed(quote2.QuoteID);
            ReviewQuote rQ = (ReviewQuote)view.Model;

            //Assert
            Assert.Equal(2, rQ.QuoteID);

            Assert.Equal("Bob's Concrete", rQ.Name);

            Assert.Equal("123456", rQ.OrderNum);

            Assert.Equal(469.4m, rQ.TotalCost);

            Assert.Equal(2, rQ.BarsUsed.Count);
            Assert.Equal(4, rQ.BarsUsed[0].BarSize);
            Assert.Equal(14, rQ.BarsUsed[0].NumOfBars);
            Assert.Equal(10, rQ.BarsUsed[0].BarCost);
            Assert.Equal(40, rQ.BarsUsed[0].NumOfCuts);
            Assert.Equal(0.25m, rQ.BarsUsed[0].CutCost);
            Assert.Equal(80, rQ.BarsUsed[0].NumOfBends);
            Assert.Equal(0.25m, rQ.BarsUsed[0].BendCost);
            Assert.Equal(5, rQ.BarsUsed[1].BarSize);
            Assert.Equal(15, rQ.BarsUsed[1].NumOfBars);
            Assert.Equal(15, rQ.BarsUsed[1].BarCost);
            Assert.Equal(90, rQ.BarsUsed[1].NumOfCuts);
            Assert.Equal(0.33m, rQ.BarsUsed[1].CutCost);
            Assert.Equal(90, rQ.BarsUsed[1].NumOfBends);
            Assert.Equal(0.33m, rQ.BarsUsed[1].BendCost);

            Assert.Equal(15, rQ.SetUpCharge);

            Assert.Equal(2, rQ.FinalRemnants.Count);
            Assert.Equal(4, rQ.FinalRemnants[0].BarSize);
            Assert.Equal(2, rQ.FinalRemnants[0].Remnants.Count);
            Assert.Equal(15, rQ.FinalRemnants[0].Remnants[0].Length);
            Assert.Equal(13, rQ.FinalRemnants[0].Remnants[0].Qty);
            Assert.False(rQ.FinalRemnants[0].Remnants[0].UsedAgain);
            Assert.Equal(165, rQ.FinalRemnants[0].Remnants[1].Length);
            Assert.Equal(1, rQ.FinalRemnants[0].Remnants[1].Qty);
            Assert.False(rQ.FinalRemnants[0].Remnants[1].UsedAgain);
            Assert.Equal(2, rQ.FinalRemnants[1].Remnants.Count);
            Assert.Equal(8, rQ.FinalRemnants[1].Remnants[0].Length);
            Assert.Equal(10, rQ.FinalRemnants[1].Remnants[0].Qty);
            Assert.False(rQ.FinalRemnants[1].Remnants[0].UsedAgain);
            Assert.Equal(20, rQ.FinalRemnants[1].Remnants[1].Length);
            Assert.Equal(5, rQ.FinalRemnants[1].Remnants[1].Qty);
            Assert.False(rQ.FinalRemnants[1].Remnants[1].UsedAgain);

            Assert.Equal(3, rQ.Shapes.Count);
            Assert.Equal(4, rQ.Shapes[0].ShapeID);
            Assert.Equal(40, rQ.Shapes[0].Qty);
            Assert.Equal(4, rQ.Shapes[0].BarSize);
            Assert.Equal(14, rQ.Shapes[0].NumOfBars);
            Assert.Equal(75, rQ.Shapes[0].CutLength);
            Assert.Equal(2, rQ.Shapes[0].Instructions.Count);
            Assert.Equal(3, rQ.Shapes[0].Instructions[0].CutQty);
            Assert.Equal(240, rQ.Shapes[0].Instructions[0].PerLength);
            Assert.Equal("Bar", rQ.Shapes[0].Instructions[0].PerType);
            Assert.Equal(13, rQ.Shapes[0].Instructions[0].ForQty);
            Assert.Equal(1, rQ.Shapes[0].Instructions[1].CutQty);
            Assert.Equal(240, rQ.Shapes[0].Instructions[1].PerLength);
            Assert.Equal("Bar", rQ.Shapes[0].Instructions[1].PerType);
            Assert.Equal(1, rQ.Shapes[0].Instructions[1].ForQty);
            Assert.Equal(3, rQ.Shapes[0].Legs.Count);
            Assert.Equal(30, rQ.Shapes[0].Legs[0].Length);
            Assert.Equal(90, rQ.Shapes[0].Legs[0].Degree);
            Assert.Equal("Small", rQ.Shapes[0].Legs[0].Mandrel);
            Assert.Equal("16.5", rQ.Shapes[0].Legs[0].PinNumber);
            Assert.Equal(1.5m, rQ.Shapes[0].Legs[0].InGained);
            Assert.Equal(18, rQ.Shapes[0].Legs[1].Length);
            Assert.Equal(90, rQ.Shapes[0].Legs[1].Degree);
            Assert.Equal("Small", rQ.Shapes[0].Legs[1].Mandrel);
            Assert.Equal("16.5", rQ.Shapes[0].Legs[1].PinNumber);
            Assert.Equal(1.5m, rQ.Shapes[0].Legs[1].InGained);
            Assert.Equal(30, rQ.Shapes[0].Legs[2].Length);
            Assert.Equal(0, rQ.Shapes[0].Legs[2].Degree);
            Assert.Equal("", rQ.Shapes[0].Legs[2].Mandrel);
            Assert.Equal("", rQ.Shapes[0].Legs[2].PinNumber);
            Assert.Equal(0, rQ.Shapes[0].Legs[2].InGained);
            Assert.Equal(2, rQ.Shapes[0].Remnants.Count);
            Assert.Equal(15, rQ.Shapes[0].Remnants[0].Length);
            Assert.Equal(13, rQ.Shapes[0].Remnants[0].Qty);
            Assert.False(rQ.Shapes[0].Remnants[0].UsedAgain);
            Assert.Equal(165, rQ.Shapes[0].Remnants[1].Length);
            Assert.Equal(1, rQ.Shapes[0].Remnants[1].Qty);
            Assert.False(rQ.Shapes[0].Remnants[1].UsedAgain);
            Assert.Equal(2, rQ.Shapes[1].ShapeID);
            Assert.Equal(30, rQ.Shapes[1].Qty);
            Assert.Equal(5, rQ.Shapes[1].BarSize);
            Assert.Equal(10, rQ.Shapes[1].NumOfBars);
            Assert.Equal(70, rQ.Shapes[1].CutLength);
            Assert.Single(rQ.Shapes[1].Instructions);
            Assert.Equal(3, rQ.Shapes[1].Instructions[0].CutQty);
            Assert.Equal(240, rQ.Shapes[1].Instructions[0].PerLength);
            Assert.Equal("Bar", rQ.Shapes[1].Instructions[0].PerType);
            Assert.Equal(10, rQ.Shapes[1].Instructions[0].ForQty);
            Assert.Equal(2, rQ.Shapes[1].Legs.Count);
            Assert.Equal(36, rQ.Shapes[1].Legs[0].Length);
            Assert.Equal(90, rQ.Shapes[1].Legs[0].Degree);
            Assert.Equal("Medium", rQ.Shapes[1].Legs[0].Mandrel);
            Assert.Equal("15", rQ.Shapes[1].Legs[0].PinNumber);
            Assert.Equal(2, rQ.Shapes[1].Legs[0].InGained);
            Assert.Equal(36, rQ.Shapes[1].Legs[1].Length);
            Assert.Equal(0, rQ.Shapes[1].Legs[1].Degree);
            Assert.Equal("", rQ.Shapes[1].Legs[1].Mandrel);
            Assert.Equal("", rQ.Shapes[1].Legs[1].PinNumber);
            Assert.Equal(0, rQ.Shapes[1].Legs[1].InGained);
            Assert.Single(rQ.Shapes[1].Remnants);
            Assert.Equal(30, rQ.Shapes[1].Remnants[0].Length);
            Assert.Equal(10, rQ.Shapes[1].Remnants[0].Qty);
            Assert.True(rQ.Shapes[1].Remnants[0].UsedAgain);
            Assert.Equal(3, rQ.Shapes[2].ShapeID);
            Assert.Equal(60, rQ.Shapes[2].Qty);
            Assert.Equal(5, rQ.Shapes[2].BarSize);
            Assert.Equal(5, rQ.Shapes[2].NumOfBars);
            Assert.Equal(22, rQ.Shapes[2].CutLength);
            Assert.Equal(2, rQ.Shapes[2].Instructions.Count);
            Assert.Equal(1, rQ.Shapes[2].Instructions[0].CutQty);
            Assert.Equal(30, rQ.Shapes[2].Instructions[0].PerLength);
            Assert.Equal("Remnant", rQ.Shapes[2].Instructions[0].PerType);
            Assert.Equal(10, rQ.Shapes[2].Instructions[0].ForQty);
            Assert.Equal(10, rQ.Shapes[2].Instructions[1].CutQty);
            Assert.Equal(240, rQ.Shapes[2].Instructions[1].PerLength);
            Assert.Equal("Bar", rQ.Shapes[2].Instructions[1].PerType);
            Assert.Equal(5, rQ.Shapes[2].Instructions[1].ForQty);
            Assert.Equal(2, rQ.Shapes[2].Legs.Count);
            Assert.Equal(12, rQ.Shapes[2].Legs[0].Length);
            Assert.Equal(90, rQ.Shapes[2].Legs[0].Degree);
            Assert.Equal("Medium", rQ.Shapes[2].Legs[0].Mandrel);
            Assert.Equal("15", rQ.Shapes[2].Legs[0].PinNumber);
            Assert.Equal(2, rQ.Shapes[2].Legs[0].InGained);
            Assert.Equal(12, rQ.Shapes[2].Legs[1].Length);
            Assert.Equal(0, rQ.Shapes[2].Legs[1].Degree);
            Assert.Equal("", rQ.Shapes[2].Legs[1].Mandrel);
            Assert.Equal("", rQ.Shapes[2].Legs[1].PinNumber);
            Assert.Equal(0, rQ.Shapes[2].Legs[1].InGained);
            Assert.Equal(2, rQ.Shapes[2].Remnants.Count);
            Assert.Equal(8, rQ.Shapes[2].Remnants[0].Length);
            Assert.Equal(10, rQ.Shapes[2].Remnants[0].Qty);
            Assert.False(rQ.Shapes[2].Remnants[0].UsedAgain);
            Assert.Equal(20, rQ.Shapes[2].Remnants[1].Length);
            Assert.Equal(5, rQ.Shapes[2].Remnants[1].Qty);
            Assert.False(rQ.Shapes[2].Remnants[1].UsedAgain);
        }

        [Fact]
        public async Task OpenClosedQuoteTest()
        {
            //Arrange
            quote2.Open = false;
            await repoQ.AddQuoteAsync(quote2);

            //Act
            await controllerQ.OpenClosedQuote(2);

            //Assert
            Assert.True(quote2.Open);
        }
    }
}
