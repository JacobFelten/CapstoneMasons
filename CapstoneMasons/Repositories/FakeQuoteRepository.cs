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

        public Task<List<Quote>> Quotes
        {
            get
            {
                return Task.FromResult<List<Quote>>(quotes);
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
                result = true;
            }
            return Task.FromResult<bool>(result);
        }
    }
}
