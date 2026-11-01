using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WebGym.Controllers
{
    public abstract class BaseController : Controller
    {
        protected int? CurrentUserId
        {
            get
            {
                var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.TryParse(idStr, out var id) ? id : null;
            }
        }

        protected string? CurrentUserEmail => User.FindFirstValue(ClaimTypes.Email);

        protected string? CurrentUserName => User.FindFirstValue(ClaimTypes.Name);

        protected string? CurrentUserRole => User.FindFirstValue(ClaimTypes.Role);

        protected void SetSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        protected void SetErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
        }

        protected void SetInfoMessage(string message)
        {
            TempData["InfoMessage"] = message;
        }
    }
}
