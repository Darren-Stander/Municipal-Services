using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Controllers
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
            return View();
        }

        public IActionResult ReportIssues()
        {
            return RedirectToAction("Index", "ReportIssue");
        }

        public IActionResult LocalEvents()
        {
            return RedirectToAction("Index", "Event");
        }

        // To be implemented later
        public IActionResult ServiceRequestStatus()
        {
            
            ViewBag.Message = "Service Request Status feature will be implemented soon.";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} // End of file
