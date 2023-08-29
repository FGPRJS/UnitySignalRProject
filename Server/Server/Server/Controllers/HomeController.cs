using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ContentResult Index()
        {
            var html = System.IO.File.ReadAllText(@"./wwwroot/HomeIndex.html");

            return base.Content(html, "text/html");
        }
    }
}
