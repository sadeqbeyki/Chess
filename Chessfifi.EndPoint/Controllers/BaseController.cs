using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Chessfifi.Services.Service;
using Chessfifi.EndPoint.Models.Common;

namespace Chessfifi.EndPoint.Controllers;
public class BaseController : Controller
{
    private readonly ILogger _logger;
    private IUserService _userService;

    public BaseController(
        ILoggerFactory loggerFactory,
        IUserService userService)
    {
        _logger = loggerFactory.CreateLogger("base");
        _userService = userService;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            try
            {
                var user = _userService.GetUser(userId);
                var model = new UserViewModel();
                model.Id = user.Id;
                model.IsEmailConfirmed = user.IsEmailConfirmed;
                ViewBag.User = model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cant find user");
            }
        }

        base.OnActionExecuted(context);
    }
}
