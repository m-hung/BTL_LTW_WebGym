using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebGym.Controllers;

namespace WebGym.Areas.PT.Controllers
{
    [Area("PT")]
    [Authorize(Roles = "PT,Trainer,Admin")]
    public abstract class PTBaseController : BaseController
    {
    }
}
