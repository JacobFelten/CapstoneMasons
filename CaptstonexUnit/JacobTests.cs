using System;
using Xunit;
using CapstoneMasons.Models;
using CapstoneMasons.Repositories;
using CapstoneMasons.Controllers;
using CapstoneMasons.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CaptstonexUnit
{
    public class JacobTests
    {
        private ShapesController controllerS;
        private QuotesController controllerQ;
        private FakeShapeRepository repoS;
        private FakeQuoteRepository repoQ;
        private FakeFormulaRepository repoF;
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
        private CreateShape cS;

        public JacobTests ()
        {
            repoS = new FakeShapeRepository();
            controllerS = new ShapesController(repoS);
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
            controllerQ = new QuotesController(repoQ, repoF);
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
                Name = "4 Bar",
                Price = 10,
                LastChanged = new DateTime()
            };
            cost2 = new Cost
            {
                CostID = 2,
                Name = "4 Cut",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost3 = new Cost
            {
                CostID = 3,
                Name = "4 Bend",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost4 = new Cost
            {
                CostID = 4,
                Name = "5 Bar",
                Price = 15,
                LastChanged = new DateTime()
            };
            cost5 = new Cost
            {
                CostID = 5,
                Name = "5 Cut",
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            cost6 = new Cost
            {
                CostID = 6,
                Name = "5 Bend",
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            cost7 = new Cost
            {
                CostID = 7,
                Name = "Setup",
                Price = 15,
                LastChanged = new DateTime()
            };
            cost8 = new Cost
            {
                CostID = 8,
                Name = "Setup Min",
                Price = 150,
                LastChanged = new DateTime()
            };
            #endregion
            quote2 = new Quote
            {
                QuoteID = 2,
                Name = "Bob's Concrete",
                OrderNum = "123456",
                Shapes = { shape2, shape3, shape4 },
                Costs = { cost1, cost2, cost3, cost4, cost5, cost6, cost7, cost8 },
                DateQuoted = DateTime.Now,
                PickedUp = false,
                Open = false
            };
        }

        [Fact]
        public async Task CreateShapeTest()
        {
            // Arrange
            // Done in the constructor

            // Act
            await controllerS.Create(cS);
            List<Shape> shapes = await repoS.Shapes;

            // Assert
            Assert.Single(shapes);
            Assert.Single(quote.Shapes);
            Assert.Equal(3, shape.Legs.Count);
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
                PinNumber = 16.5m,
                InGained = 1.5m,
                LastChanged = DateTime.Now
            });
            await repoF.AddFormulaAsync(new Formula
            {
                FormulaID = 2,
                BarSize = 5,
                Degree = 90,
                Mandrel = mandrel2,
                PinNumber = 15,
                InGained = 2,
                LastChanged = DateTime.Now
            });

            //Act
            ViewResult view = (ViewResult)await controllerQ.ReviewQuote(quote2);
            ReviewQuote rQ = (ReviewQuote)view.Model;

            //Assert
            Assert.Equal(2, rQ.QuoteID);

            Assert.Equal("Bob's Concrete", rQ.Name);

            Assert.Equal("123456", rQ.OrderNum);

            Assert.Equal(454.4m, rQ.TotalCost);

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

            Assert.Equal(0, rQ.SetUpCharge);
            
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
                    Assert.Equal(16.5m, rQ.Shapes[0].Legs[0].PinNumber);
                    Assert.Equal(1.5m, rQ.Shapes[0].Legs[0].InGained);
                    Assert.Equal(18, rQ.Shapes[0].Legs[1].Length);
                    Assert.Equal(90, rQ.Shapes[0].Legs[1].Degree);
                    Assert.Equal("Small", rQ.Shapes[0].Legs[1].Mandrel);
                    Assert.Equal(16.5m, rQ.Shapes[0].Legs[1].PinNumber);
                    Assert.Equal(1.5m, rQ.Shapes[0].Legs[1].InGained);
                    Assert.Equal(30, rQ.Shapes[0].Legs[2].Length);
                    Assert.Equal(0, rQ.Shapes[0].Legs[2].Degree);
                    Assert.Null(rQ.Shapes[0].Legs[2].Mandrel);
                    Assert.Equal(0, rQ.Shapes[0].Legs[2].PinNumber);
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
                    Assert.Equal(15, rQ.Shapes[1].Legs[0].PinNumber);
                    Assert.Equal(2, rQ.Shapes[1].Legs[0].InGained);
                    Assert.Equal(36, rQ.Shapes[1].Legs[1].Length);
                    Assert.Equal(0, rQ.Shapes[1].Legs[1].Degree);
                    Assert.Null(rQ.Shapes[1].Legs[1].Mandrel);
                    Assert.Equal(0, rQ.Shapes[1].Legs[1].PinNumber);
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
                    Assert.Equal(15, rQ.Shapes[2].Legs[0].PinNumber);
                    Assert.Equal(2, rQ.Shapes[2].Legs[0].InGained);
                    Assert.Equal(12, rQ.Shapes[2].Legs[1].Length);
                    Assert.Equal(0, rQ.Shapes[2].Legs[1].Degree);
                    Assert.Null(rQ.Shapes[2].Legs[1].Mandrel);
                    Assert.Equal(0, rQ.Shapes[2].Legs[1].PinNumber);
                    Assert.Equal(0, rQ.Shapes[2].Legs[1].InGained);
                Assert.Equal(2, rQ.Shapes[2].Remnants.Count);
                    Assert.Equal(8, rQ.Shapes[2].Remnants[0].Length);
                    Assert.Equal(10, rQ.Shapes[2].Remnants[0].Qty);
                    Assert.False(rQ.Shapes[2].Remnants[0].UsedAgain);
                    Assert.Equal(20, rQ.Shapes[2].Remnants[1].Length);
                    Assert.Equal(5, rQ.Shapes[2].Remnants[1].Qty);
                    Assert.False(rQ.Shapes[2].Remnants[1].UsedAgain);
        }
    }
}
