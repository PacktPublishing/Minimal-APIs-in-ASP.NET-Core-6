using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ControllerAPI.Sample.Controllers
{
    [Route("validations")]
    [ApiController]
    public class ValidationsController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(ValidationData data)
        {
            return Ok(data);
        }
    }

    public class ValidationData
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }
    }
}
