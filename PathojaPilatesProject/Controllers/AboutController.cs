using Microsoft.AspNetCore.Mvc;

namespace PathojaPilatesProject.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
