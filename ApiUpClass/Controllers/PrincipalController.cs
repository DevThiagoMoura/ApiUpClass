using Microsoft.AspNetCore.Mvc;

namespace ApiUpClass.Controllers
{
    [Route("/")]
    [ApiController]
    public class PrincipalController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { api = "ApiUpClass", status = "up" });
        }
    }
}
