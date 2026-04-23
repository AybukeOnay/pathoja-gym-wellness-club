using Microsoft.AspNetCore.Mvc;

namespace PathojaPilatesProject.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
