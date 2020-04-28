using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CapstoneMasons.Models;
using CapstoneMasons.ViewModels;

namespace CapstoneMasons.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.ShowPopUp = false;
            return View();
        }

        public IActionResult IndexPopUp(CreateQuote quote)
        {
           ViewBag.ShowPopUp = true;
           return View("Index",quote);
        }

        public IActionResult Credits()
        {
            return View();
        }

        public IActionResult NotFinished()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
