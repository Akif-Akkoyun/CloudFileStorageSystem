using Microsoft.AspNetCore.Mvc;

namespace App.WebUI.Controllers
{
    public class MySharedPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
