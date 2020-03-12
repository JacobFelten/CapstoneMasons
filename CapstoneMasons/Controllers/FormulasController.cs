using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CapstoneMasons.Models;
using CapstoneMasons.Repositories;

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
            return View(await repo.Formulas);
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
        public async Task<IActionResult> Create([Bind("FormulaID,BarSize,Degree,PinNumber,InGained,LastChanged")] Formula formula)
        {
            if (ModelState.IsValid)
            {
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

        private bool FormulaExists(int id)
        {
            var formulas = (IQueryable<Formula>)repo.GetAllFormulasAsync();
            return formulas.Any(e => e.FormulaID == id);
        }
    }
}
