using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.Repositories
{
    public class FakeQuoteRepository : IQuoteRepository
    {
        private List<Quote> quotes = new List<Quote>();
        private List<Cost> barCosts = new List<Cost>();
        public FakeQuoteRepository() { }
        public FakeQuoteRepository(List<Cost> costs)
        {
            barCosts = costs;
        }
        public Task<List<Quote>> Quotes
        {
            get
            {
                return Task.FromResult<List<Quote>>(quotes);
            }
        }

        public Task<IQueryable<Cost>> BarCosts
        {
            get
            {
                return Task.FromResult<IQueryable<Cost>>(barCosts.AsQueryable<Cost>()
                    .Where(c => c.CostID < 15));
            }
        }

        public Task<bool> AddQuoteAsync(Quote q)
        {
            bool result = false;
            if (q != null)
            {
                result = true;
                quotes.Add(q);
            }
            return Task.FromResult<bool>(result);
        }

        public Task<bool> DeleteQuoteAsync(Quote q)
        {
            bool result = false;
            if (q != null)
            {
                result = true;
                quotes.Remove(q);
            }
            return Task.FromResult<bool>(result);
        }

        public Task<IQueryable<Quote>> GetAllQuotesAsync()
        {
            return Task.FromResult<IQueryable<Quote>>(quotes.AsQueryable<Quote>());
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
                case nameof(Quote.PickedUp):
                    q.PickedUp = bool.Parse(value);
                    break;
                case nameof(Quote.Open):
                    q.Open = bool.Parse(value);
                    break;
                default:
                    throw new Exception(message: "Not a valid prop");
            }
            return Task.CompletedTask;
        }
    }
}
