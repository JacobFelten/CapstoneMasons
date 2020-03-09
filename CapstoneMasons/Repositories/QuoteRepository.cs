using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneMasons.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private AppDbContext context;

        public QuoteRepository(AppDbContext appDbContext)
        {
            context = appDbContext;
        }

        public Task<List<Quote>> Quotes
        {
            get 
            { 
                return context.Quotes
                    .Include(q => q.Costs)
                    .Include(q => q.Shapes)
                        .ThenInclude(s => s.Legs)
                            .ThenInclude(l => l.Mandrel)
                    .ToListAsync(); 
            }
        }

        public Task<bool> AddQuoteAsync(Quote q)
        {
            bool result = false;
            if (q != null)
            {
                result = true;
                context.Quotes.Add(q);
                context.SaveChanges();
            }
            return Task.FromResult<bool>(result);
        }

        public Task<bool> DeleteQuoteAsync(Quote q)
        {
            bool result = false;
            if (q != null)
            {
                result = true;
                foreach (Cost c in q.Costs)
                    context.Costs.Remove(c);
                foreach (Shape s in q.Shapes)
                {
                    foreach (Leg l in s.Legs)
                        context.Legs.Remove(l);
                    context.Shapes.Remove(s)
                }               
                context.Quotes.Remove(q);
                context.SaveChanges();
            }
            return Task.FromResult<bool>(result);
        }

        public Task<IQueryable<Quote>> GetAllQuotesAsync()
        {
            return Task.FromResult<IQueryable<Quote>>(context.Quotes.AsQueryable<Quote>());
        }

        public async Task<Quote> GetQuoteByIdAsync(int? id)
        {
            if (id != null)
            {
                foreach (Quote q in await Quotes)
                    if (q.QuoteID == id)
                        return q;
                return null;
            }
            else
            {
                return null;
            }

        }

        public Task<bool> UpdateQuoteAsync(Quote oldQ, Quote newQ)
        {
            bool result = false;
            if (oldQ != null && newQ != null)
            {
                oldQ.OrderNum = newQ.OrderNum;
                oldQ.Name = newQ.Name;
                oldQ.DateQuoted = newQ.DateQuoted;
                oldQ.PickedUp = newQ.PickedUp;
                oldQ.Open = newQ.Open;
                oldQ.Costs.Clear();
                foreach (Cost c in newQ.Costs)
                    oldQ.Costs.Add(c);
                foreach (Shape s in newQ.Shapes)
                    oldQ.Shapes.Add(s);
                context.Quotes.Update(oldQ);
                context.SaveChanges();
                result = true;
            }
            return Task.FromResult<bool>(result);
        }
    }
}
