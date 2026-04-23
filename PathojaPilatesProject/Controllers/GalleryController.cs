using Microsoft.AspNetCore.Mvc;

namespace PathojaPilatesProject.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
