using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectManager.Data;

namespace ProjectManager.Api {
  public class Startup {

    private const string AllCors = "All";

    public Startup(IConfiguration config) {
      Configuration = config;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
      services.AddControllers();
       
      services.AddDbContext<AppDbContext>(
        options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
      ); ;

      services.AddCors(
        options => options.AddPolicy(AllCors,
          build => build.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
        )
      );
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseCors(AllCors);

      app.UseRouting();

      app.UseEndpoints(endpoints => {
        endpoints.MapDefaultControllerRoute();
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
