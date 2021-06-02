using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectManager.Data;

namespace ProjectManager.Api {
  public class Startup {

    private const string AllCors = "All";
    private readonly IWebHostEnvironment env;

    public Startup(IConfiguration config, IWebHostEnvironment env) {
      Configuration = config;
      this.env = env;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {

      services.AddDbContext<AppDbContext>(
        options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
      ); ;

      services.AddControllers();

      services.AddRazorPages();

      services.AddCors(
        options => options.AddPolicy(AllCors,
          build => build.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
        )
      );
    }

    public void Configure(IApplicationBuilder app) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseCors(AllCors);

      app.UseRouting();

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapDefaultControllerRoute();
        endpoints.MapRazorPages();
      });

      UpgradeDatabase(app);
    }

    private void UpgradeDatabase(IApplicationBuilder app) {
      using (var serviceScope = app.ApplicationServices.CreateScope()) {
        var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
        if (context != null && context.Database != null) {
          context.Database.Migrate();
        }
      }
    }
  }
}
