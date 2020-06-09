using System;
using Xunit;
using System.Collections.Generic;
using System.Text;
using CapstoneMasons.Repositories;
using CapstoneMasons.Models;
using CapstoneMasons.Controllers;
using CapstoneMasons.ViewModels;
using CapstoneMasons.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CaptstonexUnit
{
    public class QuoteManipulationTests
    {
        private QuotesController controllerQ;
        private FakeShapeRepository repoS;
        private FakeQuoteRepository repoQ;
        private FakeFormulaRepository repoF;
        private FakeCostRepository repoC;
        private Quote quote;
        private Quote quote2;
        private Quote quote3;
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
        private Cost cost21;
        private Cost cost22;
        private Cost cost23;
        private Cost cost24;
        private Cost cost25;
        private Cost cost26;
        private Cost cost27;
        private Cost cost28;

        public QuoteManipulationTests()
        {
            repoC = new FakeCostRepository();
            repoF = new FakeFormulaRepository();
            repoQ = new FakeQuoteRepository();
            repoS = new FakeShapeRepository();
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
                Degree = 40,
                Mandrel = mandrel2
            };
            leg3 = new Leg
            {
                LegID = 3,
                Length = 36,
                Degree = 25,
                Mandrel = mandrel2
            };
            #endregion
            #region Shapes
            shape = new Shape
            {
                ShapeID = 1,
                BarSize = 4,
                LegCount = 1,
                Legs = { leg1 },
                Qty = 30,
                NumCompleted = 15
            };
            shape2 = new Shape
            {
                ShapeID = 2,
                BarSize = 4,
                LegCount = 1,
                Legs = { leg2 },
                Qty = 90,
                NumCompleted = 89
            };
            shape3 = new Shape
            {
                ShapeID = 3,
                BarSize = 4,
                LegCount = 1,
                Legs = { leg3 },
                Qty = 60,
                NumCompleted = 59
            };
            #endregion
            #region Extra Costs
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
                Name = KnownObjects.Bar3GlobalCost.Name,
                Price = 10,
                LastChanged = new DateTime()
            };
            cost9 = new Cost
            {
                CostID = 9,
                Name = KnownObjects.Bar3CutCost.Name,
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost10 = new Cost
            {
                CostID = 10,
                Name = KnownObjects.Bar3BendCost.Name,
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost11 = new Cost
            {
                CostID = 11,
                Name = KnownObjects.Bar6GlobalCost.Name,
                Price = 15,
                LastChanged = new DateTime()
            };
            cost12 = new Cost
            {
                CostID = 12,
                Name = KnownObjects.Bar6CutCost.Name,
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            cost13 = new Cost
            {
                CostID = 13,
                Name = KnownObjects.Bar6BendCost.Name,
                Price = 0.33m,
                LastChanged = new DateTime(2020, 5, 1)
            };
            cost14 = new Cost
            {
                CostID = 14,
                Name = "4Bar",
                Price = 10,
                LastChanged = new DateTime(2020, 4, 20)
            };
            cost15 = new Cost
            {
                CostID = 15,
                Name = "4Cut",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost16 = new Cost
            {
                CostID = 16,
                Name = "4Bend",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost17 = new Cost
            {
                CostID = 17,
                Name = "4Bar",
                Price = 10,
                LastChanged = new DateTime(2020, 5, 5)
            };
            cost18 = new Cost
            {
                CostID = 18,
                Name = "4Cut",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost19 = new Cost
            {
                CostID = 19,
                Name = "4Bend",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost20 = new Cost
            {
                CostID = 20,
                Name = "4Bar",
                Price = 10,
                LastChanged = new DateTime(2020, 5, 22)
            };
            cost21 = new Cost
            {
                CostID = 21,
                Name = "4Cut",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost22 = new Cost
            {
                CostID = 22,
                Name = "4Bend",
                Price = 0.25m,
                LastChanged = new DateTime()
            };
            cost23 = new Cost
            {
                CostID = 23,
                Name = "5Bar",
                Price = 15,
                LastChanged = new DateTime()
            };
            cost24 = new Cost
            {
                CostID = 24,
                Name = "5Cut",
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            cost25 = new Cost
            {
                CostID = 25,
                Name = "5Bend",
                Price = 0.33m,
                LastChanged = new DateTime()
            };
            cost26 = new Cost
            {
                CostID = 26,
                Name = "Setup",
                Price = 15,
                LastChanged = new DateTime()
            };
            cost27 = new Cost
            {
                CostID = 27,
                Name = "Setup",
                Price = 15,
                LastChanged = new DateTime()
            };
            cost28 = new Cost
            {
                CostID = 28,
                Name = "Setup",
                Price = 15,
                LastChanged = new DateTime()
            };
            #endregion
            #region Quotes
            quote = new Quote
            {
                QuoteID = 2,
                Name = "Bob's Concrete",
                OrderNum = "123456",
                UseFormulas = true,
                Shapes = { shape },
                Costs = { cost14, cost15, cost16, cost26 },
                DateQuoted = new DateTime(2020, 5, 20),
                PickedUp = false,
                Open = true
            };
            quote2 = new Quote
            {
                QuoteID = 3,
                Name = "Dane's Stuff",
                OrderNum = "KDH47D",
                UseFormulas = true,
                Shapes = { shape2 },
                Costs = { cost17, cost18, cost19, cost27 },
                DateQuoted = new DateTime(2020, 5, 22),
                PickedUp = false,
                Open = true
            };
            quote3 = new Quote
            {
                QuoteID = 4,
                Name = "Acob's Stuff",
                OrderNum = "ANCDG2",
                UseFormulas = true,
                Shapes = { shape3 },
                Costs = { cost20, cost21, cost22, cost28 },
                DateQuoted = new DateTime(2020, 5, 30),
                PickedUp = false,
                Open = true
            };
            #endregion
            repoQ.AddQuoteAsync(quote);
            repoQ.AddQuoteAsync(quote2);
            repoQ.AddQuoteAsync(quote3);
            repoC.AddCostAsync(cost1);
            repoC.AddCostAsync(cost2);
            repoC.AddCostAsync(cost3);
            repoC.AddCostAsync(cost4);
            repoC.AddCostAsync(cost5);
            repoC.AddCostAsync(cost6);
            repoC.AddCostAsync(cost7);
            repoC.AddCostAsync(cost8);
            repoC.AddCostAsync(cost9);
            repoC.AddCostAsync(cost10);
            repoC.AddCostAsync(cost11);
            repoC.AddCostAsync(cost12);
            repoC.AddCostAsync(cost13);
        }

        [Fact]
        public async Task IndexSort()
        {
            //Arrange
            //Act
            ViewResult view = (ViewResult)await controllerQ.Index();
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal(3, oq.ReviewQuotes.Count);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            //Bob's Concrete prices return true as in they need to be updated
            Assert.True(oq.ReviewQuotes[1].Update);
            //Dane's Stuff prices return false meaning they are fine as is and dont need to be updated.
            Assert.False(oq.ReviewQuotes[2].Update);
        }
        #region OpenQuoteSearch
        [Fact]
        public async Task OpenSearchOnlyAtoZ()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "AtoZ";
            oqt.Sort2 = null;
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[1].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchOnlyCheapest()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Cheapest";
            oqt.Sort2 = null;
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchOnlyExpensive()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Most Expensive";
            oqt.Sort2 = null;
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchOnlyNewest()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Newest";
            oqt.Sort2 = null;
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchOnlyOldest()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Oldest";
            oqt.Sort2 = null;
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchPickedUpAtoZ()
        {
            //Arrange
            (await repoQ.Quotes)[1].PickedUp = true;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "AtoZ";
            oqt.Sort2 = "PickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchPickedUpCheapest()
        {
            //Arrange
            (await repoQ.Quotes)[1].PickedUp = true;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Cheapest";
            oqt.Sort2 = "PickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchPickedUpExpensive()
        {
            //Arrange
            (await repoQ.Quotes)[1].PickedUp = true;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Most Expensive";
            oqt.Sort2 = "PickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Dane's Stuff",oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff",oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchPickedUpNewest()
        {
            //Arrange
            (await repoQ.Quotes)[1].PickedUp = true;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Newest";
            oqt.Sort2 = "PickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchPickedUpOldest()
        {
            //Arrange
            (await repoQ.Quotes)[1].PickedUp = true;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Oldest";
            oqt.Sort2 = "PickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchNotPickedUpAtoZ()
        {
            //Arrange
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "AtoZ";
            oqt.Sort2 = "NotPickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchNotPickedUpCheapest()
        {
            //Arrange
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Cheapest";
            oqt.Sort2 = "NotPickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchNotPickedUpExpensive()
        {
            //Arrange
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Most Expensive";
            oqt.Sort2 = "NotPickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert       
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchNotPickedUpNewest()
        {
            //Arrange
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Newest";
            oqt.Sort2 = "NotPickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert       
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchNotPickedUpOldest()
        {
            //Arrange
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Oldest";
            oqt.Sort2 = "NotPickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert     
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchClosestCompletionAtoZ()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "AtoZ";
            oqt.Sort2 = "ClosestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert 
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchClosestCompletionCheapest()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Cheapest";
            oqt.Sort2 = "ClosestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert    
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchClosestCompletionExpensive()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Most Expensive";
            oqt.Sort2 = "ClosestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchClosestCompletionNewest()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Newest";
            oqt.Sort2 = "ClosestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchClosestCompletionOldest()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Oldest";
            oqt.Sort2 = "ClosestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchFarthestToCompletionAtoZ()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "AtoZ";
            oqt.Sort2 = "FarthestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchFarthestToCompletionCheapest()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Cheapest";
            oqt.Sort2 = "FarthestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchFarthestToCompletionExpensive()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Most Expensive";
            oqt.Sort2 = "FarthestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchFarthestToCompletionNewest()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Newest";
            oqt.Sort2 = "FarthestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchFarthestToCompletionOldest()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Oldest";
            oqt.Sort2 = "FarthestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task OpenSearchSearchBarName()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.SearchBarSpecific = "Name";
            oqt.SearchBar = "Dane";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal(1, oq.ReviewQuotes.Count);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
        }

        [Fact]
        public async Task OpenSearchSearchBarOrderNum()
        {
            //Arrange
            OpenQuote oqt = new OpenQuote();
            oqt.SearchBarSpecific = "Order Num";
            oqt.SearchBar = "123456";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchOpen(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal(1, oq.ReviewQuotes.Count);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
        }
        #endregion
        #region ClosedQuoteSearch
        [Fact]
        public async Task ClosedSearchOnlyAtoZ()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "AtoZ";
            oqt.Sort2 = null;
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[1].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchOnlyCheapest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Cheapest";
            oqt.Sort2 = null;
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchOnlyExpensive()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Most Expensive";
            oqt.Sort2 = null;
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchOnlyNewest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Newest";
            oqt.Sort2 = null;
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchOnlyOldest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Oldest";
            oqt.Sort2 = null;
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchPickedUpAtoZ()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            (await repoQ.Quotes)[1].PickedUp = true;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "AtoZ";
            oqt.Sort2 = "PickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchPickedUpCheapest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            (await repoQ.Quotes)[1].PickedUp = true;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Cheapest";
            oqt.Sort2 = "PickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchPickedUpExpensive()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            (await repoQ.Quotes)[1].PickedUp = true;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Most Expensive";
            oqt.Sort2 = "PickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchPickedUpNewest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            (await repoQ.Quotes)[1].PickedUp = true;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Newest";
            oqt.Sort2 = "PickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchPickedUpOldest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            (await repoQ.Quotes)[1].PickedUp = true;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Oldest";
            oqt.Sort2 = "PickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchNotPickedUpAtoZ()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "AtoZ";
            oqt.Sort2 = "NotPickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchNotPickedUpCheapest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Cheapest";
            oqt.Sort2 = "NotPickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchNotPickedUpExpensive()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Most Expensive";
            oqt.Sort2 = "NotPickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert       
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchNotPickedUpNewest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Newest";
            oqt.Sort2 = "NotPickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert       
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchNotPickedUpOldest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            (await repoQ.Quotes)[2].PickedUp = true;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Oldest";
            oqt.Sort2 = "NotPickedUp";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert     
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchClosestCompletionAtoZ()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "AtoZ";
            oqt.Sort2 = "ClosestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert 
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchClosestCompletionCheapest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Cheapest";
            oqt.Sort2 = "ClosestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert    
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchClosestCompletionExpensive()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Most Expensive";
            oqt.Sort2 = "ClosestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchClosestCompletionNewest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Newest";
            oqt.Sort2 = "ClosestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchClosestCompletionOldest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Oldest";
            oqt.Sort2 = "ClosestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchFarthestToCompletionAtoZ()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "AtoZ";
            oqt.Sort2 = "FarthestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchFarthestToCompletionCheapest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Cheapest";
            oqt.Sort2 = "FarthestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchFarthestToCompletionExpensive()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Most Expensive";
            oqt.Sort2 = "FarthestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchFarthestToCompletionNewest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Newest";
            oqt.Sort2 = "FarthestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchFarthestToCompletionOldest()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.Sort = "Oldest";
            oqt.Sort2 = "FarthestToCompletion";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[1].Name);
            Assert.Equal("Acob's Stuff", oq.ReviewQuotes[2].Name);
        }

        [Fact]
        public async Task ClosedSearchSearchBarName()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.SearchBarSpecific = "Name";
            oqt.SearchBar = "Dane";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal(1, oq.ReviewQuotes.Count);
            Assert.Equal("Dane's Stuff", oq.ReviewQuotes[0].Name);
        }

        [Fact]
        public async Task ClosedSearchSearchBarOrderNum()
        {
            //Arrange
            (await repoQ.Quotes)[0].Open = false;
            (await repoQ.Quotes)[1].Open = false;
            (await repoQ.Quotes)[2].Open = false;
            OpenQuote oqt = new OpenQuote();
            oqt.SearchBarSpecific = "Order Num";
            oqt.SearchBar = "123456";
            //Act
            ViewResult view = (ViewResult)await controllerQ.SearchClosed(oqt);
            OpenQuote oq = (OpenQuote)view.Model;
            //Assert
            Assert.Equal(1, oq.ReviewQuotes.Count);
            Assert.Equal("Bob's Concrete", oq.ReviewQuotes[0].Name);
        }
        #endregion
    }
}
