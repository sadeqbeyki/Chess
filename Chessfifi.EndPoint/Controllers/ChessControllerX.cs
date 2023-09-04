using Microsoft.AspNetCore.Mvc;

namespace Chessfifi.EndPoint.Controllers
{
    public class ChessControllerX : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
