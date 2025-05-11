using Microsoft.AspNetCore.Mvc;

namespace App.WebUI.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
