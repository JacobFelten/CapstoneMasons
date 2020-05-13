using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;
using Microsoft.EntityFrameworkCore;
using CapstoneMasons.Infrastructure;

namespace CapstoneMasons.Repositories
{
    public class FakeCostRepository : ICostRepository
    {
        List<Cost> costs = new List<Cost>();

        public Task<List<Cost>> Costs
        {
            get { return Task.FromResult<List<Cost>>(costs); }
        }

        public Task<IQueryable<Cost>> BarCosts
        {
            get
            {
                return Task.FromResult<IQueryable<Cost>>(costs.AsQueryable<Cost>()
                    .Where(c => c.Name.Contains(KnownObjects.GlobalKeyWord)));
            }
        }

        public Task<bool> AddCostAsync(Cost c)
        {
            bool result = false;
            if (c != null)
            {
                result = true;
                costs.Add(c);
            }
            return Task.FromResult<bool>(result);
        }

        public Task<IQueryable<Cost>> GetAllCostsAsync()
        {
            return Task.FromResult<IQueryable<Cost>>(costs.AsQueryable<Cost>());
        }

        public async Task<Cost> GetCostByIdAsync(int? id)
        {
            if (id != null)
            {
                foreach (Cost c in await Costs)
                    if (c.CostID == id)
                        return c;
                return null;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteCostByIdAsync(int? id)
        {
            if (id != null)
            {
                foreach (Cost c in await Costs)
                    if (c.CostID == id)
                    {
                        costs.Remove(c);
                        return true;
                    }
                return false;
            }
            else
            {
                return false;
            }
        }

        public Task<bool> UpdateCostAsync(Cost oldC, Cost newC)
        {
            bool result = false;
            if (oldC != null && newC != null)
            {
                oldC.Price = newC.Price;
                oldC.LastChanged = DateTime.Now;
                result = true;
            }
            return Task.FromResult<bool>(result);
        }

        public async Task<Cost> FindCostByNameAsync(string costName)
        {
            foreach (Cost c in await Costs)
            {
                if (c.Name == costName)
                {
                    return c;
                }
            }

            return null;

        }
    }
}
