using Microsoft.AspNetCore.Mvc;

namespace ControllerAPI.Sample.Controllers
{
    [Route("jsons")]
    [ApiController]
    public class JsonsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var response = new[]
            {
                new PersonData { Name = "Andrea", Surname = "Tosato", BirthDate = new DateTime(2022, 01, 01) },
                new PersonData { Name = "Emanuele", Surname = "Bartolesi", BirthDate = new DateTime(2022, 01, 01) },
                new PersonData { Name = "Marco", Surname = "Minerva", BirthDate = new DateTime(2022, 01, 01) }
            };
            return Ok(response);
        }
    }

    public class PersonData
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
