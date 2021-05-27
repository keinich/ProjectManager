using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

      AddIdentity(services);

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

      app.UseIdentityServer();

      // auth here

      app.UseEndpoints(endpoints => {
        endpoints.MapDefaultControllerRoute();
        endpoints.MapRazorPages();
      });

      UpgradeDatabase(app);
    }

    private void AddIdentity(IServiceCollection services) {
      services.AddIdentity<IdentityUser, IdentityRole>(
        options => {
          if (this.env.IsDevelopment()) {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
          }
          else {
            //TODO
          }
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

      services.ConfigureApplicationCookie(config => {
        config.LoginPath = "/Account/Login";
      });

      var identityServerBuilder = services.AddIdentityServer();

      identityServerBuilder.AddAspNetIdentity<IdentityUser>();

      if (env.IsDevelopment()) {
        identityServerBuilder.AddInMemoryIdentityResources(new IdentityResource[] {
          new IdentityResources.OpenId(),
          new IdentityResources.Profile()
        });

        identityServerBuilder.AddInMemoryClients(new Client[] {
          new Client {
            ClientId = "nuxt-js-app",
            AllowedGrantTypes = GrantTypes.Code,

            RedirectUris = new[] { "http://localohst:3000" },
            PostLogoutRedirectUris = new[] { "http://localohst:3000" },
            AllowedCorsOrigins = new[] { "http://localohst:3000" },

            RequirePkce = true,
            AllowAccessTokensViaBrowser = true,
            RequireConsent = false,
            RequireClientSecret = false
          }
        });


        identityServerBuilder.AddDeveloperSigningCredential();
      }
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
