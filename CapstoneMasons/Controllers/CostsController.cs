using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CapstoneMasons.Models;
using CapstoneMasons.Repositories;
using CapstoneMasons.ViewModels;
using CapstoneMasons.Infrastructure;

namespace CapstoneMasons.Controllers
{
    public class CostsController : Controller
    {
        ICostRepository repo;

        public CostsController(ICostRepository repository)
        {
            repo = repository;
        }

        [HttpGet]
        public async Task<ViewResult> GlobalCosts()
        {
            List<Cost> globalCosts = await repo.Costs;

            GlobalCosts costs = new GlobalCosts()
            {
                //For some reason globalCosts[0] refers to Bar4GlobalCost
                //and globalCosts[3] refers to Bar3GlobalCost? We might need to figure out why later
                Bar3GlobalCost = await repo.FindCostByNameAsync(KnownObjects.Bar3GlobalCost.Name),
                Bar3BendCost = await repo.FindCostByNameAsync(KnownObjects.Bar3BendCost.Name),
                Bar3CutCost = await repo.FindCostByNameAsync(KnownObjects.Bar3CutCost.Name),
                Bar4GlobalCost = await repo.FindCostByNameAsync(KnownObjects.Bar4GlobalCost.Name),
                Bar4BendCost = await repo.FindCostByNameAsync(KnownObjects.Bar4BendCost.Name),
                Bar4CutCost = await repo.FindCostByNameAsync(KnownObjects.Bar4CutCost.Name),
                Bar5GlobalCost = await repo.FindCostByNameAsync(KnownObjects.Bar5GlobalCost.Name),
                Bar5BendCost = await repo.FindCostByNameAsync(KnownObjects.Bar5BendCost.Name),
                Bar5CutCost = await repo.FindCostByNameAsync(KnownObjects.Bar5CutCost.Name),
                Bar6GlobalCost = await repo.FindCostByNameAsync(KnownObjects.Bar6GlobalCost.Name),
                Bar6BendCost = await repo.FindCostByNameAsync(KnownObjects.Bar6BendCost.Name),
                Bar6CutCost = await repo.FindCostByNameAsync(KnownObjects.Bar6CutCost.Name),
                SetupCharge = await repo.FindCostByNameAsync(KnownObjects.SetupCharge.Name),
                MinimumOrderCost = await repo.FindCostByNameAsync(KnownObjects.MinimumOrderCost.Name)
            };

            return View(costs);
        }

        
        [HttpPost]
        public async Task<ViewResult> GlobalCosts(GlobalCosts globals)
        {
            List<Cost> globalCosts = await repo.Costs;

            await CheckCosts(globals.Bar3GlobalCost);
            await CheckCosts(globals.Bar3BendCost);
            await CheckCosts(globals.Bar3CutCost);
            await CheckCosts(globals.Bar4GlobalCost);
            await CheckCosts(globals.Bar4BendCost);
            await CheckCosts(globals.Bar4CutCost);
            await CheckCosts(globals.Bar5GlobalCost);
            await CheckCosts(globals.Bar5BendCost);
            await CheckCosts(globals.Bar5CutCost);
            await CheckCosts(globals.Bar6GlobalCost);
            await CheckCosts(globals.Bar6BendCost);
            await CheckCosts(globals.Bar6CutCost);
            await CheckCosts(globals.SetupCharge);
            await CheckCosts(globals.MinimumOrderCost);

            return View(globals);
        }

        private async Task CheckCosts(Cost cost)
        {
            Cost oldCost = await repo.FindCostByNameAsync(cost.Name);
            if (oldCost.Price != cost.Price)
            {
                await repo.UpdateCostAsync(oldCost, cost);
            }
        }
        
        
    }
}
