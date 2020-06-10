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
    public class FormulaCRUDTests
    {
        private FormulasController controller;
        private FakeFormulaRepository repo;
        private Mandrel noneMandrel;
        private Mandrel smallMandrel;
        private Mandrel mediumMandrel;
        private Mandrel largeMandrel;
        private Formula f1;
        private Formula f2;

        public FormulaCRUDTests()
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
        public async Task FormulaDeleteTest()
        {
            // Arrange
            // Done in the constructor

            // Act
            ViewResult view = (ViewResult)await controller.Delete(1);
            Formula f = (Formula)view.Model;

            //Assert
            Assert.Equal(4, f.BarSize);
            Assert.Equal(90, f.Degree);
            Assert.Equal(smallMandrel, f.Mandrel);
            Assert.Equal("16.5", f.PinNumber);
            Assert.Equal(1.5m, f.InGained);
        }

        [Fact]
        public async Task DeleteConfirmed()
        {
            //Arrange
            //Done in the constructor

            //Act
            await controller.DeleteConfirmed(2);

            //Assert
            Assert.Null(await repo.GetFormulaByIdAsync(2));
            Assert.Single(await repo.Formulas);
        }

        [Fact]
        public async Task Number3BarValidCreateTest()
        {
            //Arrange
            FormulaCreate ftest = new FormulaCreate { BarSize = 3, Degree = 90, MandrelID = 1, PinNumber = "15", InGained = 3.75m};
            //Act
            await controller.Create(ftest);
            List<Formula> formulas = await repo.Formulas;
            //Assert
            Assert.Equal(3, formulas.Count);
            Assert.Equal(3, formulas[2].BarSize);
            Assert.Equal(90, formulas[2].Degree);
            Assert.Equal(noneMandrel, formulas[2].Mandrel);
            Assert.Equal("15", formulas[2].PinNumber);
            Assert.Equal(3.75m, formulas[2].InGained);
        }

        [Fact]
        public async Task Number4BarValidCreateTest()
        {
            //Arrange
            FormulaCreate ftest = new FormulaCreate { BarSize = 4, Degree = 45, MandrelID = 2, PinNumber = "12", InGained = 3.75m };
            //Act
            await controller.Create(ftest);
            List<Formula> formulas = await repo.Formulas;
            //Assert
            Assert.Equal(3, formulas.Count);
            Assert.Equal(4, formulas[2].BarSize);
            Assert.Equal(45, formulas[2].Degree);
            Assert.Equal(smallMandrel, formulas[2].Mandrel);
            Assert.Equal("12", formulas[2].PinNumber);
            Assert.Equal(3.75m, formulas[2].InGained);
        }

        [Fact]
        public async Task Number4BarInvalidCreateTest()
        {
            //Arrange
            FormulaCreate ftest = new FormulaCreate { BarSize = 4, Degree = 45, MandrelID = 1, PinNumber = "12", InGained = 3.75m };
            //Act
            await controller.Create(ftest);
            List<Formula> formulas = await repo.Formulas;
            //Assert
            Assert.Equal(2, formulas.Count);
        }

        [Fact]
        public async Task Number5BarValidCreateTest()
        {
            //Arrange
            FormulaCreate ftest = new FormulaCreate { BarSize = 5, Degree = 45, MandrelID = 3, PinNumber = "12", InGained = 3.75m };
            //Act
            await controller.Create(ftest);
            List<Formula> formulas = await repo.Formulas;
            //Assert
            Assert.Equal(3, formulas.Count);
            Assert.Equal(5, formulas[2].BarSize);
            Assert.Equal(45, formulas[2].Degree);
            Assert.Equal(mediumMandrel, formulas[2].Mandrel);
            Assert.Equal("12", formulas[2].PinNumber);
            Assert.Equal(3.75m, formulas[2].InGained);
        }

        [Fact]
        public async Task Number5BarInvalidCreateTest()
        {
            //Arrange
            FormulaCreate ftest = new FormulaCreate { BarSize = 5, Degree = 45, MandrelID = 2, PinNumber = "12", InGained = 3.75m };
            //Act
            await controller.Create(ftest);
            List<Formula> formulas = await repo.Formulas;
            //Assert
            Assert.Equal(2, formulas.Count);
        }

        [Fact]
        public async Task Number6BarValidCreateTest()
        {
            //Arrange
            FormulaCreate ftest = new FormulaCreate { BarSize = 6, Degree = 90, MandrelID = 4, PinNumber = "15", InGained = 3.75m };
            //Act
            await controller.Create(ftest);
            List<Formula> formulas = await repo.Formulas;
            //Assert
            Assert.Equal(3, formulas.Count);
            Assert.Equal(6, formulas[2].BarSize);
            Assert.Equal(90, formulas[2].Degree);
            Assert.Equal(largeMandrel, formulas[2].Mandrel);
            Assert.Equal("15", formulas[2].PinNumber);
            Assert.Equal(3.75m, formulas[2].InGained);
        }

        [Fact]
        public async Task Number6BarInvalidCreateTest()
        {
            //Arrange
            FormulaCreate ftest = new FormulaCreate { BarSize = 6, Degree = 90, MandrelID = 3, PinNumber = "15", InGained = 3.75m };
            //Act
            await controller.Create(ftest);
            List<Formula> formulas = await repo.Formulas;
            //Assert
            Assert.Equal(2, formulas.Count);
        }

        [Fact]
        public async Task RetrieveByBarSizeTest()
        {
            //Arrange
            //Nothing to Arrange
            //Act
            ViewResult view = (ViewResult)await controller.SearchFormulas(4, null, null);
            FormulaSearch fs = (FormulaSearch)view.Model;
            //Assert
            Assert.Single(fs.SearchResults);
            Assert.Equal(90, fs.SearchResults[0].Degree);
            Assert.Equal(smallMandrel, fs.SearchResults[0].Mandrel);
            Assert.Equal("16.5", fs.SearchResults[0].PinNumber);
            Assert.Equal(1.5m, fs.SearchResults[0].InGained);
        }

        [Fact]
        public async Task RetrieveByDegreesTest()
        {
            //Arrange
            //Nothing to Arrange
            //Act
            ViewResult view = (ViewResult)await controller.SearchFormulas(null, 90, null);
            FormulaSearch fs = (FormulaSearch)view.Model;
            //Assert
            Assert.Equal(2, fs.SearchResults.Count);
            Assert.Equal(90, fs.SearchResults[1].Degree);
            Assert.Equal(mediumMandrel, fs.SearchResults[1].Mandrel);
            Assert.Equal("15", fs.SearchResults[1].PinNumber);
            Assert.Equal(2m, fs.SearchResults[1].InGained);
        }

        [Fact]
        public async Task RetrieveByMandrelIDTest()
        {
            //Arrange
            //Nothing to Arrange
            //Act
            ViewResult view = (ViewResult)await controller.SearchFormulas(null, null, 2);
            FormulaSearch fs = (FormulaSearch)view.Model;
            //Assert
            Assert.Single(fs.SearchResults);
            Assert.Equal(90, fs.SearchResults[0].Degree);
            Assert.Equal(smallMandrel, fs.SearchResults[0].Mandrel);
            Assert.Equal("16.5", fs.SearchResults[0].PinNumber);
            Assert.Equal(1.5m, fs.SearchResults[0].InGained);
        }

        [Fact]
        public async Task UpdateFormulaTest()
        {
            //Arrange
            //Nothing to Arrange
            //Act
            ViewResult view = (ViewResult)await controller.Edit(1);
            FormulaCreate fs = (FormulaCreate)view.Model;
            fs.InGained = 10.05m;
            fs.PinNumber = "14.5";
            await controller.Edit(fs);
            List<Formula> formulas = await repo.Formulas;
            //Assert
            Assert.Equal(2, formulas.Count);
            Assert.Equal(4, formulas[0].BarSize);
            Assert.Equal(90, formulas[0].Degree);
            Assert.Equal(smallMandrel, formulas[0].Mandrel);
            Assert.Equal("14.5", formulas[0].PinNumber);
            Assert.Equal(10.05m, formulas[0].InGained);
        }
    }
}
