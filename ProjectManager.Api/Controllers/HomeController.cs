using Microsoft.AspNetCore.Mvc;
using ProjectManager.Data;
using System.Linq;

namespace ProjectManager.Api.Controllers {

  [ApiController]
  [Route("api/home")]
  public class HomeController : ControllerBase {

    public HomeController(AppDbContext identityDbContext) {
    }

    [HttpGet]
    public string Index() {
      return "Hello World 1";
    }

    //[Authorize]
    //[HttpGet]
    //public string Secret() {
    //  return "Secret";
    //}
  }

}
