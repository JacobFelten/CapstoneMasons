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
    public class ShapesController : Controller
    {
        IShapeRepository repo;

        public ShapesController(IShapeRepository repository)
        {
            repo = repository;
        }

        // GET: Shapes
        public async Task<IActionResult> Index()
        {
            return View(await repo.Shapes);
        }

        // GET: Shapes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shape = await repo.GetShapeByIdAsync(id);
            if (shape == null)
            {
                return NotFound();
            }

            return View(shape);
        }

        // GET: Shapes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shapes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShapeID,BarSize,LegCount,Qty,NumCompleted")] Shape shape)
        {
            if (ModelState.IsValid)
            {
                await repo.AddShapeAsync(shape);
                return RedirectToAction(nameof(Index));
            }
            return View(shape);
        }

        // GET: Shapes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shape = await repo.GetShapeByIdAsync(id);
            if (shape == null)
            {
                return NotFound();
            }
            return View(shape);
        }

        // POST: Shapes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShapeID,BarSize,LegCount,Qty,NumCompleted")] Shape newShape)
        {
            if (id != newShape.ShapeID)
            {
                return NotFound();
            }

            Shape oldShape = null;

            if (ModelState.IsValid)
            {
                try
                {
                    oldShape = await repo.GetShapeByIdAsync(newShape.ShapeID);
                    await repo.UpdateShapesAsync(oldShape, newShape);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShapeExists(oldShape.ShapeID))
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
            return View(oldShape);
        }

        // GET: Shapes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shape = await repo.GetShapeByIdAsync(id);
            if (shape == null)
            {
                return NotFound();
            }

            return View(shape);
        }

        // POST: Shapes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shape = await repo.GetShapeByIdAsync(id);
            await repo.DeleteShapeAsync(shape);
            return RedirectToAction(nameof(Index));
        }

        private bool ShapeExists(int id)
        {
            var shapes = (IQueryable<Shape>)repo.GetAllShapesAsync();
            return shapes.Any(e => e.ShapeID == id);
        }
    }
}
