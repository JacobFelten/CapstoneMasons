﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneMasons.Repositories
{
    public class FakeCostRepository : ICostRepository
    {
        List<Cost> costs = new List<Cost>();

        public Task<List<Cost>> Costs
        {
            get { return Task.FromResult<List<Cost>>(costs); }
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