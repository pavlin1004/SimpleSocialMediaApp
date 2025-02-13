using Microsoft.AspNetCore.Mvc;

namespace SimpleSocialApp.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
