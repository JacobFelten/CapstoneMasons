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
        private readonly AppDbContext _context;

        public FormulasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Formulas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Formulas.ToListAsync());
        }

        // GET: Formulas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formula = await _context.Formulas
                .FirstOrDefaultAsync(m => m.FormulaID == id);
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
                _context.Add(formula);
                await _context.SaveChangesAsync();
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

            var formula = await _context.Formulas.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("FormulaID,BarSize,Degree,PinNumber,InGained,LastChanged")] Formula formula)
        {
            if (id != formula.FormulaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(formula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormulaExists(formula.FormulaID))
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
            return View(formula);
        }

        // GET: Formulas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formula = await _context.Formulas
                .FirstOrDefaultAsync(m => m.FormulaID == id);
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
            var formula = await _context.Formulas.FindAsync(id);
            _context.Formulas.Remove(formula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormulaExists(int id)
        {
            return _context.Formulas.Any(e => e.FormulaID == id);
        }
    }
}
