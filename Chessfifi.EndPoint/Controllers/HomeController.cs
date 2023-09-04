using Chessfifi.EndPoint.Models;
using Chessfifi.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Chessfifi.EndPoint.Controllers;

public class HomeController : BaseController
{
    public HomeController(ILoggerFactory loggerFactory, IUserService userService) : base(loggerFactory, userService)
    {
    }
    public IActionResult Index()
    {

        return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}