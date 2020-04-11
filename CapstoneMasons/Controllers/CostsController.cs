using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CapstoneMasons.Models;

namespace CapstoneMasons.Controllers
{
    public class CostsController : Controller
    {
        private readonly ILogger<CostsController> _logger;


        // Add reference to the Cost Repo!!!

        public CostsController(ILogger<CostsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ViewResult GlobalCosts()
        {
            return View();
        }

        [HttpPost]
        public ViewResult UpdateGlobalCosts()
        {
            return View(/*Costs from Repo*/);
        }
        
    }
}
