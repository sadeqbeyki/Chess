using Chessfifi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Chessfifi.EndPoint.Controllers
{
    public class ChessController : Controller
    {
        private readonly ChessDbContext _context;

        public ChessController(ChessDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
