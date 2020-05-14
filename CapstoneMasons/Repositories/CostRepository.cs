using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;
using Microsoft.EntityFrameworkCore;
using CapstoneMasons.Infrastructure;

namespace CapstoneMasons.Repositories
{
    public class CostRepository : ICostRepository
    {
        private AppDbContext context;

        public CostRepository(AppDbContext appDbContext)
        {
            context = appDbContext;
        }

        public Task<List<Cost>> Costs
        {
            get { return context.Costs.ToListAsync(); }
        }

        public Task<bool> AddCostAsync(Cost c)
        {
            bool result = false;
            if (c != null)
            {
                result = true;
                context.Costs.Add(c);
                context.SaveChanges();
            }            
            return Task.FromResult<bool>(result);
        }

        public Task<IQueryable<Cost>> GetAllCostsAsync()
        {
            return Task.FromResult<IQueryable<Cost>>(context.Costs.AsQueryable<Cost>());
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
                        context.Costs.Remove(c);
                        context.SaveChanges();
                        return true;
                    }                       
                return false;
            }
            else
            {
                return false;
            }
        }

        //Make sure that oldC and newC are not the same before doing this method
        public Task<bool> UpdateCostAsync(Cost oldC, Cost newC)
        {
            bool result = false;
            if (oldC != null && newC != null)
            {
                oldC.Price = newC.Price;
                oldC.LastChanged = TimeZoneInfo.ConvertTime(DateTime.Now,
                 TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
                context.Costs.Update(oldC);
                context.SaveChanges();
                result = true;
            }
            return Task.FromResult<bool>(result);
        }

        public Task<IQueryable<Cost>> BarCosts
        {
            get
            {
                return Task.FromResult<IQueryable<Cost>>(context.Costs.AsQueryable<Cost>()
                    .Where(c => c.Name.Contains(KnownObjects.GlobalKeyWord)));
            }
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
