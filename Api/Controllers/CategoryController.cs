using Api.Common;
using Api.CustomAttribute;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        [CustomAuthorize(UserRoleType.Admin)]
        public IActionResult Index()
        {
            return Ok("deneme");
        }
    }
}
