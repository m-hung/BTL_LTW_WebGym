using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebGym.Controllers;

namespace WebGym.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize(Roles = "Member,Admin")]
    public abstract class MemberBaseController : BaseController
    {
    }
}
