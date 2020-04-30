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
        public async Task<IActionResult> GlobalCosts()
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

            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return Json(costs);
            }

            return View(costs);
        }

        
        [HttpPost]
        public async Task<IActionResult> GlobalCosts(int bar3GlobalID, decimal bar3GlobalCost, int bend3GlobalID, decimal bar3BendCost, int cut3GlobalID, decimal bar3CutCost,
            int bar4GlobalID, decimal bar4GlobalCost, int bend4GlobalID, decimal bar4BendCost, int cut4GlobalID, decimal bar4CutCost,
            int bar5GlobalID, decimal bar5GlobalCost, int bend5GlobalID, decimal bar5BendCost, int cut5GlobalID, decimal bar5CutCost,
            int bar6GlobalID, decimal bar6GlobalCost, int bend6GlobalID, decimal bar6BendCost, int cut6GlobalID, decimal bar6CutCost,
            int setupGlobalID, decimal setupCharge, int minOrderGlobalID, decimal minimumOrderCost)
        {
            GlobalCosts globals = new GlobalCosts();
            List<Cost> globalCosts = await repo.Costs;

            globals.Bar3GlobalCost = new Cost 
            {
                CostID = bar3GlobalID,
                Name = KnownObjects.Bar3GlobalCost.Name,
                Price = bar3GlobalCost
            };
            globals.Bar3BendCost = new Cost
            {
                CostID = bend3GlobalID,
                Name = KnownObjects.Bar3BendCost.Name,
                Price = bar3BendCost
            };
            globals.Bar3CutCost = new Cost
            {
                CostID = cut3GlobalID,
                Name = KnownObjects.Bar3CutCost.Name,
                Price = bar3CutCost
            };
            globals.Bar4GlobalCost = new Cost
            {
                CostID = bar4GlobalID,
                Name = KnownObjects.Bar4GlobalCost.Name,
                Price = bar4GlobalCost
            };
            globals.Bar4BendCost = new Cost
            {
                CostID = bend4GlobalID,
                Name = KnownObjects.Bar4BendCost.Name,
                Price = bar4BendCost
            };
            globals.Bar4CutCost = new Cost
            {
                CostID = cut4GlobalID,
                Name = KnownObjects.Bar4CutCost.Name,
                Price = bar4CutCost
            };
            globals.Bar5GlobalCost = new Cost
            {
                CostID = bar5GlobalID,
                Name = KnownObjects.Bar5GlobalCost.Name,
                Price = bar5GlobalCost
            };
            globals.Bar5BendCost = new Cost
            {
                CostID = bend5GlobalID,
                Name = KnownObjects.Bar5BendCost.Name,
                Price = bar5BendCost
            };
            globals.Bar5CutCost = new Cost
            {
                CostID = cut5GlobalID,
                Name = KnownObjects.Bar5CutCost.Name,
                Price = bar5CutCost
            };

            globals.Bar6GlobalCost = new Cost
            {
                CostID = bar6GlobalID,
                Name = KnownObjects.Bar6GlobalCost.Name,
                Price = bar6GlobalCost
            };
            globals.Bar6BendCost = new Cost
            {
                CostID = bend6GlobalID,
                Name = KnownObjects.Bar6BendCost.Name,
                Price = bar6BendCost
            };
            globals.Bar6CutCost = new Cost
            {
                CostID = cut6GlobalID,
                Name = KnownObjects.Bar6CutCost.Name,
                Price = bar6CutCost
            };

            globals.SetupCharge = new Cost
            {
                CostID = setupGlobalID,
                Name = KnownObjects.SetupCharge.Name,
                Price = setupCharge
            };
            globals.MinimumOrderCost = new Cost
            {
                CostID = minOrderGlobalID,
                Name = KnownObjects.MinimumOrderCost.Name,
                Price = minimumOrderCost
            };

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

            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return Json("Prices successfully changed");
            }

            return RedirectToAction("GlobalCosts");
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
