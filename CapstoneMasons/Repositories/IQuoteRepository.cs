using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.Repositories
{
    public interface IQuoteRepository
    {
        public Task<List<Quote>> Quotes { get; }
        public Task<IQueryable<Cost>> BarCosts { get; }

        public Task<bool> AddQuoteAsync(Quote q);

        public Task<bool> DeleteQuoteAsync(Quote q);

        public Task<IQueryable<Quote>> GetAllQuotesAsync();

        public Task<Quote> GetQuoteByIdAsync(int? id);

        public Task<bool> UpdateQuoteAsync(Quote newQ);

        public Task UpdateQuoteSimpleAsync(Quote q, string prop, string value);

        public Task<bool> UpdateCostAsync(Cost oldC, Cost newC);
    }
}
