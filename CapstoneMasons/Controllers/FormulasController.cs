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
using CapstoneMasons.Infrastructure;
using Microsoft.AspNetCore.Authorization;

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
            FormulaCreate fCreate = new FormulaCreate { Mandrels = await repo.Mandrels, Usable = true};
            return View(fCreate);
        }

        // POST: Formulas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FormulaCreate fCreate)
        {
            fCreate.Mandrels = await repo.Mandrels;
            string useMessage = await MandrelUseable(fCreate);
            if (ModelState.IsValid)
            {
                if (useMessage == "")
                {
                    Formula formula = new Formula 
                    { 
                        BarSize = fCreate.BarSize,
                        Degree = fCreate.Degree,
                        Mandrel = await repo.GetMandrelByIdAsync(fCreate.MandrelID),
                        PinNumber = fCreate.PinNumber,
                        InGained = fCreate.InGained,
                        LastChanged = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"))
                    };
                    if (! await FormulaExists(formula))
                    {
                        fCreate.Usable = true;
                        await repo.AddFormulaAsync(formula);
                        return RedirectToAction(nameof(Index));
                    }
                    fCreate.UsableMessage = "There already exists a formula with that bar size, bend degree, and mandrel.";
                    fCreate.Usable = false;
                    return View(fCreate);
                }
                fCreate.UsableMessage = useMessage;
                fCreate.Usable = false;
                return View(fCreate);
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
            FormulaCreate fCreate = new FormulaCreate 
            { 
                Mandrels = await repo.Mandrels,
                FormulaID = id,
                Usable = true,
                BarSize = formula.BarSize,
                Degree = formula.Degree,
                MandrelID = formula.Mandrel.MandrelID,
                PinNumber = formula.PinNumber,
                InGained = formula.InGained,
                LastChanged = formula.LastChanged
            };
            return View(fCreate);
        }

        // POST: Formulas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FormulaCreate fCreate)
        {
            Formula oldFormula = null;
            Formula newFormula = new Formula 
            { 
                FormulaID = (int)fCreate.FormulaID,
                BarSize = fCreate.BarSize,
                Degree = fCreate.Degree,
                Mandrel = await repo.GetMandrelByIdAsync(fCreate.MandrelID),
                PinNumber = fCreate.PinNumber,
                InGained = fCreate.InGained,
                LastChanged = fCreate.LastChanged 
            };

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
            return View(fCreate);
        }

        // GET: Formulas/Delete/5
        [Authorize(Roles = "Admins")]
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
        [Authorize(Roles = "Admins")]
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

        private async Task<string> MandrelUseable(FormulaCreate fCreate)
        {
            string usable = "";
            string error4 = "#4 rebar can only use " + KnownObjects.SmallMandrel.Name + ", " + KnownObjects.MediumMandrel.Name + ", and " +
                KnownObjects.LargeMandrel.Name + " mandrels.";
            string error5 = "#5 rebar can only use " + KnownObjects.MediumMandrel.Name + " and " +
                KnownObjects.LargeMandrel.Name + " mandrels.";
            string error6 = "#6 rebar can only use " + KnownObjects.LargeMandrel.Name + " mandrels.";
            Mandrel m = await repo.GetMandrelByIdAsync(fCreate.MandrelID);
            if(fCreate.BarSize == 3)
            {
                switch (m.Name)
                {
                    case "None":
                        usable = "";
                        break;
                    case "Small":
                        usable = "";
                        break;
                    case "Medium":
                        usable = "";
                        break;
                    case "Large":
                        usable = "";
                        break;
                }
            }
            if (fCreate.BarSize == 4)
            {
                switch (m.Name)
                {
                    case "None":
                        usable = error4;
                        break;
                    case "Small":
                        usable = "";
                        break;
                    case "Medium":
                        usable = "";
                        break;
                    case "Large":
                        usable = "";
                        break;
                }
            }
            if (fCreate.BarSize == 5)
            {
                switch (m.Name)
                {
                    case "None":
                        usable = error5;
                        break;
                    case "Small":
                        usable = error5;
                        break;
                    case "Medium":
                        usable = "";
                        break;
                    case "Large":
                        usable = "";
                        break;
                }
            }
            if (fCreate.BarSize == 6)
            {
                switch (m.Name)
                {
                    case "None":
                        usable = error6;
                        break;
                    case "Small":
                        usable = error6;
                        break;
                    case "Medium":
                        usable = error6;
                        break;
                    case "Large":
                        usable = "";
                        break;
                }
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

        private async Task<bool> FormulaExists(Formula formula)
        {
            foreach (Formula f in await repo.Formulas)
            {
                if (f.BarSize == formula.BarSize && f.Degree == formula.Degree && f.Mandrel == formula.Mandrel)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
