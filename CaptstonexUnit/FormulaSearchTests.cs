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

namespace CaptstonexUnit
{
    public class FormulaSearchTests
    {
        private FormulasController controller;
        private FakeFormulaRepository repo;
        private Mandrel noneMandrel;
        private Mandrel smallMandrel;
        private Mandrel mediumMandrel;
        private Mandrel largeMandrel;
        private Formula f1;
        private Formula f2;

        public FormulaSearchTests()
        {
            repo = new FakeFormulaRepository();
            controller = new FormulasController(repo);
            noneMandrel = new Mandrel
            {
                MandrelID = 1,
                Name = KnownObjects.NoneMandrel.Name,
                Radius = KnownObjects.NoneMandrel.Radius
            };
            repo.AddMandrelAsync(noneMandrel);
            smallMandrel = new Mandrel
            {
                MandrelID = 2,
                Name = KnownObjects.SmallMandrel.Name,
                Radius = KnownObjects.SmallMandrel.Radius
            };
            repo.AddMandrelAsync(smallMandrel);
            mediumMandrel = new Mandrel
            {
                MandrelID = 3,
                Name = KnownObjects.MediumMandrel.Name,
                Radius = KnownObjects.MediumMandrel.Radius
            };
            repo.AddMandrelAsync(mediumMandrel);
            largeMandrel = new Mandrel
            {
                MandrelID = 4,
                Name = KnownObjects.LargeMandrel.Name,
                Radius = KnownObjects.LargeMandrel.Radius
            };
            repo.AddMandrelAsync(largeMandrel);

            f1 = new Formula
            {
                FormulaID = 1,
                BarSize = 4,
                Degree = 90,
                Mandrel = smallMandrel,
                PinNumber = "16.5",
                InGained = 1.5m
            };
            repo.AddFormulaAsync(f1);
            f2 = new Formula
            {
                FormulaID = 2,
                BarSize = 5,
                Degree = 90,
                Mandrel = mediumMandrel,
                PinNumber = "15",
                InGained = 2m
            };
            repo.AddFormulaAsync(f2);
        }

        [Fact]
        public async Task IndexTest()
        {
            //// Arrange
            // Done in the constructor

            // Act
            ViewResult view = (ViewResult) await controller.Index();
            FormulaSearch fS = (FormulaSearch)view.Model;

            // Assert
            Assert.Equal(2, fS.SearchResults.Count);
            Assert.Equal(2, fS.BarSizes.Count);
            Assert.Single(fS.Degrees);
            Assert.Equal(2, fS.Mandrels.Count);
            Assert.Null(fS.BarSize);
            Assert.Null(fS.BendDegree);
            Assert.Null(fS.MandrelID);
        }

        [Fact]
        public async Task SearchFormulasTest()
        {
            //// Arrange
            // Done in the constructor

            // Act
            ViewResult view = (ViewResult)await controller.SearchFormulas(4, null, null);
            FormulaSearch fS = (FormulaSearch)view.Model;

            // Assert
            Assert.Single(fS.SearchResults);
            Assert.Equal(4, fS.SearchResults[0].BarSize);
            Assert.Equal(90, fS.SearchResults[0].Degree);
            Assert.Equal(smallMandrel, fS.SearchResults[0].Mandrel);
            Assert.Equal("16.5", fS.SearchResults[0].PinNumber);
            Assert.Equal(1.5m, fS.SearchResults[0].InGained);
        }

        [Fact]
        public async Task SearchInvalidFormulaTest()
        {
            //// Arrange
            // Done in the constructor

            // Act
            ViewResult view = (ViewResult)await controller.SearchFormulas(4, null, 3);
            FormulaSearch fS = (FormulaSearch)view.Model;

            // Assert
            Assert.Empty(fS.SearchResults);
        }
    }
}
