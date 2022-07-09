using Microsoft.AspNetCore.Mvc;

namespace razorweb.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
   [HttpGet]
   public IActionResult Index()
   {
      return Ok("Hello World");
   }
}