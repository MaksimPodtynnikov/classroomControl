using classroomLibrary.Data.Models;
using classroomLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using classroomLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace classroomLibrary.Controllers
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
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Objects()
        {
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
}
