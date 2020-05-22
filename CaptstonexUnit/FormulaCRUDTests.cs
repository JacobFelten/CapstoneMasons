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
    }
}
