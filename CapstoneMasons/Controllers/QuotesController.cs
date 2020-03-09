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
    public class QuotesController : Controller
    {
        IQuoteRepository repo;

        public QuotesController(IQuoteRepository repository)
        {
            repo = repository;
        }

        // GET: Quotes
        public async Task<IActionResult> Index()
        {
            return View(await repo.Quotes);
        }

        // GET: Quotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await repo.GetQuoteByIdAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        // GET: Quotes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuoteID,Name,OrderNum,DateQuoted,PickedUp,Open")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                await repo.AddQuoteAsync(quote);
                return RedirectToAction(nameof(Index));
            }
            return View(quote);
        }

        public IActionResult ReviewQuote(Quote q)
        {
            ReviewQuote rQ = new ReviewQuote();
            return View(rQ);
        }

        // GET: Quotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await repo.GetQuoteByIdAsync(id);
            if (quote == null)
            {
                return NotFound();
            }
            return View(quote);
        }

        // POST: Quotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuoteID,Name,OrderNum,DateQuoted,PickedUp,Open")] Quote newQuote)
        {
            if (id != newQuote.QuoteID)
            {
                return NotFound();
            }

            Quote oldQuote = null;

            if (ModelState.IsValid)
            {
                try
                {
                    oldQuote = await repo.GetQuoteByIdAsync(newQuote.QuoteID);
                    await repo.UpdateQuoteAsync(oldQuote, newQuote);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuoteExists(oldQuote.QuoteID))
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
            return View(oldQuote);
        }

        // GET: Quotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await repo.GetQuoteByIdAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        // POST: Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quote = await repo.GetQuoteByIdAsync(id);
            await repo.DeleteQuoteAsync(quote);
            return RedirectToAction(nameof(Index));
        }

        private bool QuoteExists(int id)
        {
            var quotes = (IQueryable<Quote>)repo.GetAllQuotesAsync();
            return quotes.Any(e => e.QuoteID == id);
        }
    }
}
