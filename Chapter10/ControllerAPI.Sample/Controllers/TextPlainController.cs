using Microsoft.AspNetCore.Mvc;

namespace ControllerAPI.Sample.Controllers
{
    [Route("text-plain")]
    [ApiController]
    public class TextPlainController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("response");
        }
    }
}
