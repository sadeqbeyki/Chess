using Microsoft.AspNetCore.Mvc;

namespace Chessfifi.EndPoint.Controllers
{
    public class ChessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
