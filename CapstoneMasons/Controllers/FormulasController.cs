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
            var fList = SortFormulas(await repo.Formulas);
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
            var fList = SortFormulas(await repo.Formulas);
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
        public async Task<IActionResult> Create()
        {
            FormulaCreate fCreate = new FormulaCreate { Mandrels = await repo.Mandrels };
            return View(fCreate);
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

                Formula formula = new Formula { BarSize = fCreate.BarSize, Degree = fCreate.Degree, Mandrel = await repo.GetMandrelByIdAsync(fCreate.MandrelID), PinNumber = fCreate.PinNumber, InGained = fCreate.InGained, LastChanged = System.DateTime.Now};
                await repo.AddFormulaAsync(formula);
                return RedirectToAction(nameof(Index));
            }
            return View(fCreate);
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

        private bool MandrilUseable(FormulaCreate fCreate)
        {
            bool usable = true;
            if(fCreate.BarSize == 3)
            {
                switch()
            }
            

            
            return usable;
        }
        //This method takes a list of formulas and fills lists of bar sizes, degrees, and mandrels with all the
        //unique occurrences of those things in the list of formulas. If the current formula in the loop is of bar size 5
        //for example, the code checks to make sure the list of bar sizes doesn't allready have 5 before adding it. These
        //lists are then sorted and used to fill the dropdowns on the Formulas Index page.
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

        //This method sorts a list of formulas first by bar size, then by degrees, then by mandrel size.
        private List<Formula> SortFormulas(List<Formula> tempFList)
        {
            IOrderedEnumerable<Formula> orderedEnumerable = tempFList.OrderBy(f => f.BarSize)
                .ThenBy(f => f.Degree)
                .ThenBy(f => f.Mandrel.Radius);
            var fList = new List<Formula>();
            foreach (Formula f in orderedEnumerable)
                fList.Add(f);
            return fList;
        }
        #endregion
    }
}
