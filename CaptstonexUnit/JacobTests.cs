using System;
using Xunit;
using CapstoneMasons.Models;
using CapstoneMasons.Repositories;
using CapstoneMasons.Controllers;
using CapstoneMasons.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CaptstonexUnit
{
    public class JacobTests
    {
        private ShapesController controllerS;
        private QuotesController controllerQ;
        private FakeShapeRepository repoS;
        private FakeQuoteRepository repoQ;
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
            controllerQ = new QuotesController(repoQ);
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
                Qty = 60,
                NumCompleted = 0
            };
            shape3 = new Shape
            {
                ShapeID = 3,
                BarSize = 5,
                LegCount = 2,
                Legs = { leg3, leg4 },
                Qty = 30,
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

            //Act

            //Assert
        }
    }
}
