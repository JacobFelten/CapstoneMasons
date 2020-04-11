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
                Bar3GlobalCost = globalCosts[3],
                Bar3BendCost = globalCosts[1],
                Bar3CutCost = globalCosts[2],
                Bar4GlobalCost = globalCosts[0],
                Bar4BendCost = globalCosts[4],
                Bar4CutCost = globalCosts[5],
                Bar5GlobalCost = globalCosts[6],
                Bar5BendCost = globalCosts[7],
                Bar5CutCost = globalCosts[8],
                Bar6GlobalCost = globalCosts[9],
                Bar6BendCost = globalCosts[10],
                Bar6CutCost = globalCosts[11],
                SetupCharge = globalCosts[12],
                MinimumOrderCost = globalCosts[13]
            };

            return View(costs);
        }

        /*
        [HttpPost]
        public async Task<ViewResult> GlobalCosts(decimal bar3globalcost, decimal bar3bendcost, decimal bar3cutcost,
                                                  decimal bar4globalcost, decimal bar4bendcost, decimal bar4cutcost,
                                                  decimal bar5globalcost, decimal bar5bendcost, decimal bar5cutcost,
                                                  decimal bar6globalcost, decimal bar6bendcost, decimal bar6cutcost,
                                                  decimal setupcharge, decimal minimumordercost)
        {
            List<Cost> globalCosts = await repo.Costs;

            //For some reason globalCosts[0] refers to Bar4GlobalCost
            //and globalCosts[3] refers to Bar3GlobalCost? We might need to figure out why later

            GlobalCosts costs = new GlobalCosts()
            {
                //For some reason globalCosts[0] refers to Bar4GlobalCost
                //and globalCosts[3] refers to Bar3GlobalCost? We might need to figure out why later
                Bar3GlobalCost = globalCosts[3],
                Bar3BendCost = globalCosts[1],
                Bar3CutCost = globalCosts[2],
                Bar4GlobalCost = globalCosts[0],
                Bar4BendCost = globalCosts[4],
                Bar4CutCost = globalCosts[5],
                Bar5GlobalCost = globalCosts[6],
                Bar5BendCost = globalCosts[7],
                Bar5CutCost = globalCosts[8],
                Bar6GlobalCost = globalCosts[9],
                Bar6BendCost = globalCosts[10],
                Bar6CutCost = globalCosts[11],
                SetupCharge = globalCosts[12],
                MinimumOrderCost = globalCosts[13]
            };


            repo.UpdateCostAsync(globalCosts[0], );

            return View(costs);
        }
        */
        
    }
}
