using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Infrastructure;
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
                    context.Shapes.Remove(s);
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

        public async Task<bool> UpdateQuoteAsync(Quote newQ)
        {
            bool result = false;
            Quote oldQ = await GetQuoteByIdAsync(newQ.QuoteID);
            var newCosts = new List<Cost>();
            foreach (Cost c in newQ.Costs)
                newCosts.Add(c);
            var newShapes = new List<Shape>();
            foreach (Shape s in newQ.Shapes)
                newShapes.Add(s);
            if (oldQ != null && newQ != null)
            {
                oldQ.OrderNum = newQ.OrderNum;
                oldQ.Name = newQ.Name;
                oldQ.DateQuoted = newQ.DateQuoted;
                oldQ.PickedUp = newQ.PickedUp;
                oldQ.Open = newQ.Open;
                oldQ.Costs.Clear();
                foreach (Cost c in newCosts)
                    oldQ.Costs.Add(c);
                oldQ.Shapes.Clear();
                foreach (Shape s in newShapes)
                    oldQ.Shapes.Add(s);
                context.Quotes.Update(oldQ);
                context.SaveChanges();
                result = true;
            }
            return result;
        }

        public Task UpdateQuoteSimpleAsync(Quote q, string prop, string value)
        {
            switch (prop)
            {
                case nameof(Quote.Name):
                    q.Name = value;
                    break;
                case nameof(Quote.OrderNum):
                    q.OrderNum = value;
                    break;
                case nameof(Quote.Discount):
                    q.Discount = decimal.Parse(value);
                    break;
                case nameof(Quote.AddSetup):
                    if (value == "null")
                        q.AddSetup = null;
                    else
                        q.AddSetup = bool.Parse(value);
                    break;
                case nameof(Quote.UseFormulas):
                    q.UseFormulas = bool.Parse(value);
                    break;
                case nameof(Quote.PickedUp):
                    q.PickedUp = bool.Parse(value);
                    break;
                case nameof(Quote.Open):
                    q.Open = bool.Parse(value);
                    break;
                default:
                    throw new Exception(message: "Not a valid prop");
            }
            context.Quotes.Update(q);
            context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
