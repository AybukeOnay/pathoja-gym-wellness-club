using Microsoft.AspNetCore.Mvc;
using PathojaPilatesProject.Models;
using PathojaPilatesProject.Models.Home;
using System.Diagnostics;

namespace PathojaPilatesProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var popupEvent = _configuration
                .GetSection("HomePopupEvent")
                .Get<HomePopupEventVM>();

            var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            var turkeyNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyTimeZone);

            if (popupEvent != null
                && popupEvent.IsActive
                && turkeyNow.Date < popupEvent.ExpireDate.Date)
            {
                return View(popupEvent);
            }

            return View(null);
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
