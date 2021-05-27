using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Data;
using System.Linq;

namespace ProjectManager.Api.Controllers {

  [ApiController]
  [Route("api/home")]
  public class HomeController : ControllerBase {

    private readonly IdentityDbContext identityDbContext;

    public HomeController(AppDbContext identityDbContext) {
      this.identityDbContext = identityDbContext;
    }

    [HttpGet]
    public string Index() {
      var test = this.identityDbContext.Users.Count();
      return "Hello World 1";
    }

    //[Authorize]
    //[HttpGet]
    //public string Secret() {
    //  return "Secret";
    //}
  }

}
