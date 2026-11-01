using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebGym.Controllers;

namespace WebGym.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public abstract class AdminBaseController : BaseController
    {
    }
}
