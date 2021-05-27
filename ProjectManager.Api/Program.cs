using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Api {
  public class Program {
    public static void Main(string[] args) {
      var host = CreateHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope()) {
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        if (env.IsDevelopment()) {
          var user = new IdentityUser("test");
          userMgr.CreateAsync(user, "password").GetAwaiter().GetResult();
        }

      }

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => {
              webBuilder.UseStartup<Startup>();
            });
  }
}
