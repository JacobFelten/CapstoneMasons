using System;
using Xunit;
using CapstoneMasons.Models;
using CapstoneMasons.Repositories;
using CapstoneMasons.Controllers;
using CapstoneMasons.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CaptstonexUnit
{
    public class GinoTests
    {

        private ShapesController controllerS;
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
        private Cost bar3Cost;
        private Cost bar4Cost;
        private Cost bar5Cost;
        private Cost bar6Cost;
        private Cost setUpCost;
        private Cost bar3Bend;
        private Cost bar4Bend;
        private Cost bar5Bend;
        private Cost bar6Bend;
        private Cost bar3Cut;
        private Cost bar4Cut;
        private Cost bar5Cut;
        private Cost bar6Cut;
        private Cost setUpMin;
        private CreateShape cS;

        public GinoTests()
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

            #region Costs
            bar3Cost = new Cost
            {
                CostID = 1,
                Name = "3 Bar",
                Price = 30,
                LastChanged = new DateTime()
            };
            bar6Cost = new Cost
            {
                CostID = 1,
                Name = "6 Bar",
                Price = 60,
                LastChanged = new DateTime()
            };
            bar4Cost = new Cost
            {
                CostID = 1,
                Name = "4 Bar",
                Price = 40,
                LastChanged = new DateTime()
            };
            bar4Cut = new Cost
            {
                CostID = 2,
                Name = "4 Cut",
                Price = 0.40m,
                LastChanged = new DateTime()
            };
            bar3Bend = new Cost
            {
                CostID = 3,
                Name = "3 Bend",
                Price = 0.30m,
                LastChanged = new DateTime()
            };
            bar5Bend = new Cost
            {
                CostID = 3,
                Name = "5 Bend",
                Price = 0.50m,
                LastChanged = new DateTime()
            };
            bar4Bend = new Cost
            {
                CostID = 3,
                Name = "4 Bend",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            bar5Cost = new Cost
            {
                CostID = 4,
                Name = "5 Bar",
                Price = 50,
                LastChanged = new DateTime()
            };
            bar3Cut = new Cost
            {
                CostID = 5,
                Name = "3 Cut",
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            bar5Cut = new Cost
            {
                CostID = 6,
                Name = "5 Cut",
                Price = 0.55m,
                LastChanged = new DateTime()
            };
            setUpCost = new Cost
            {
                CostID = 7,
                Name = "Setup",
                Price = 15,
                LastChanged = new DateTime()
            };
            setUpMin = new Cost
            {
                CostID = 8,
                Name = "Setup Min",
                Price = 150,
                LastChanged = new DateTime()
            };
            #endregion

            repoQ = new FakeQuoteRepository(new List<Cost> {
                bar3Cost,bar4Cost,bar5Cost,bar6Cost,setUpCost,bar3Bend,
                bar4Bend,bar5Bend,bar3Cut,bar4Cut,bar5Cut,setUpMin//,bar6Bend,bar6Cut
            });
            repoC = new FakeCostRepository(); //Added by Jacob
            repoF = new FakeFormulaRepository();
            controllerQ = new QuotesController(repoQ, repoF, repoC, null); //Added by Jacob
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
                Length = 20
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
                BarSize = 3,
                LegCount = 2,
                Legs = { leg1, leg2 },
                Qty = 2,
                NumCompleted = 0
            };
            shape3 = new Shape
            {
                ShapeID = 3,
                BarSize = 4,
                LegCount = 1,
                Legs = { leg3 },
                Qty = 3,
                NumCompleted = 0
            };
            shape4 = new Shape
            {
                ShapeID = 4,
                BarSize = 5,
                LegCount = 3,
                Legs = { leg5, leg6, leg7 },
                Qty = 4,
                NumCompleted = 0
            };
            #endregion

            quote2 = new Quote
            {
                QuoteID = 2,
                Name = "Bob's Concrete",
                OrderNum = "123456",
                Shapes = { shape2, shape3, shape4 },
                Costs = { },
                DateQuoted = DateTime.Now,
                PickedUp = false,
                Open = false
            };
        }
        /*
        [Fact]
        public async Task UpdatePriceTest()
        {
            //arrange

            //act
            var controller = await controllerQ.UpdatePrices(quote2);

            //assErt
            Assert.True(quote2.Costs.Count(c => c.Name.Contains("Bar")) == 3);
            Assert.True(quote2.Costs.Count(c => c.Name.Contains("Cut")) == 3);
            Assert.True(quote2.Costs.Count(c => c.Name.Contains("Bend")) == 2);
            Assert.True(quote2.Costs.Count(c => c.Name.Contains("Setup Min")) == 0);
            Assert.True(quote2.Costs.Count == 8);
        }*/
    }
}
