using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Api.Controllers {

  [ApiController]
  [Route("api/home")]
  public class HomeController : ControllerBase {
    
    [HttpGet]
    public string Index() {
      return "Hello World 2";
    }
  }

}
