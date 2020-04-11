using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.Repositories
{
    public interface ICostRepository
    {
        public Task<List<Cost>> Costs { get; }

        public Task<bool> AddCostAsync(Cost c);

        public Task<IQueryable<Cost>> GetAllCostsAsync();

        public Task<Cost> GetCostByIdAsync(int? id);

        public Task<bool> UpdateCostAsync(Cost oldC, Cost newC);

    }
}
