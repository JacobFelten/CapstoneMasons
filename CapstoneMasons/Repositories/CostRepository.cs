﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;
using Microsoft.EntityFrameworkCore;

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
            get { return context.Costs.Include(c => c.Price).ToListAsync(); }
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

        //Make sure that oldC and newC are not the same before doing this method
        public Task<bool> UpdateCostAsync(Cost oldC, Cost newC)
        {
            bool result = false;
            if (oldC != null && newC != null)
            {
                oldC.Price = newC.Price;
                context.Costs.Update(oldC);
                context.SaveChanges();
                result = true;
            }
            return Task.FromResult<bool>(result);
        }
    }
}
