using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CapstoneMasons.Models;
using CapstoneMasons.Repositories;
using CapstoneMasons.ViewModels;

namespace CapstoneMasons.Controllers
{
    public class FormulasController : Controller
    {
        IFormulaRepository repo;

        public FormulasController(IFormulaRepository repository)
        {
            repo = repository;
        }

        // GET: Formulas
        public async Task<IActionResult> Index()
        {
            var tempFList = await repo.Formulas;
            IOrderedEnumerable<Formula> orderedEnumerable = tempFList.OrderBy(f => f.BarSize)
                .ThenBy(f => f.Degree);
                //.ThenBy(f => f.Mandrel.Radius);
            var fList = new List<Formula>();
            foreach (Formula f in orderedEnumerable)
                fList.Add(f);
            var barSizes = new List<int>();
            var degrees = new List<int>();
            var mandrels = new List<Mandrel>();
            FillFormulaSearchDropdowns(fList, in barSizes, in degrees, in mandrels);
            var fS = new FormulaSearch
            {
                SearchResults = fList,
                BarSizes = barSizes,
                Degrees = degrees,
                Mandrels = mandrels
            };
            return View(fS);
        }

        [HttpPost]
        public async Task<IActionResult> SearchFormulas(int? barSize, int? degree, int? mandrelID)
        {
            var tempFList = await repo.Formulas;
            IOrderedEnumerable<Formula> orderedEnumerable = tempFList.OrderBy(f => f.BarSize)
                .ThenBy(f => f.Degree);
                //.ThenBy(f => f.Mandrel.Radius);
            var fList = new List<Formula>();
            foreach (Formula f in orderedEnumerable)
                fList.Add(f);
            var m = await repo.GetMandrelByIdAsync(mandrelID);
            var resultList = new List<Formula>();
            foreach(Formula f in fList)
            {
                if ((barSize == null || f.BarSize == barSize) &&
                    (degree == null || f.Degree == degree) &&
                    (mandrelID == null || f.Mandrel == m))
                {
                    resultList.Add(f);
                }
                    
            }
            var barSizes = new List<int>();
            var degrees = new List<int>();
            var mandrels = new List<Mandrel>();
            FillFormulaSearchDropdowns(fList, in barSizes, in degrees, in mandrels);
            var fS = new FormulaSearch
            {
                SearchResults = resultList,
                BarSizes = barSizes,
                Degrees = degrees,
                Mandrels = mandrels,
                BarSize = barSize,
                BendDegree = degree,
                MandrelID = mandrelID
            };
            return View("Index", fS);
        }

        // GET: Formulas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formula = await repo.GetFormulaByIdAsync(id);

            if (formula == null)
            {
                return NotFound();
            }

            return View(formula);
        }

        // GET: Formulas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Formulas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FormulaCreate fCreate)
        {
            if (ModelState.IsValid)
            {
                Formula formula = new Formula { BarSize = fCreate.BarSize, Degree = fCreate.Degree, Mandrel = }
                await repo.AddFormulaAsync(formula);
                return RedirectToAction(nameof(Index));
            }
            return View(formula);
        }

        // GET: Formulas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formula = await repo.GetFormulaByIdAsync(id);
            if (formula == null)
            {
                return NotFound();
            }
            return View(formula);
        }

        // POST: Formulas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FormulaID,BarSize,Degree,PinNumber,InGained,LastChanged")] Formula newFormula)
        {
            if (id != newFormula.FormulaID)
            {
                return NotFound();
            }

            Formula oldFormula = null;

            if (ModelState.IsValid)
            {
                try
                {
                    oldFormula = await repo.GetFormulaByIdAsync(newFormula.FormulaID);
                    await repo.UpdateFormulaAsync(oldFormula, newFormula);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormulaExists(oldFormula.FormulaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(oldFormula);
        }

        // GET: Formulas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formula = await repo.GetFormulaByIdAsync(id);
            if (formula == null)
            {
                return NotFound();
            }

            return View(formula);
        }

        // POST: Formulas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var formula = await repo.GetFormulaByIdAsync(id);
            await repo.DeleteFormulaAsync(formula);
            return RedirectToAction(nameof(Index));
        }

        #region Other Methods
        private bool FormulaExists(int id)
        {
            var formulas = (IQueryable<Formula>)repo.GetAllFormulasAsync();
            return formulas.Any(e => e.FormulaID == id);
        }

        private void FillFormulaSearchDropdowns(List<Formula> fList, in List<int> barSizes, in List<int> degrees, in List<Mandrel> mandrels)
        {
            foreach(Formula f in fList)
            {
                if (!barSizes.Contains(f.BarSize))
                    barSizes.Add(f.BarSize);
                if (!degrees.Contains(f.Degree))
                    degrees.Add(f.Degree);
                if (f.Mandrel != null && !mandrels.Contains(f.Mandrel))
                    mandrels.Add(f.Mandrel);
            }
            barSizes.Sort();
            degrees.Sort();
            mandrels.Sort((a, b) => a.Radius.CompareTo(b.Radius));
        }
        #endregion
    }
}
