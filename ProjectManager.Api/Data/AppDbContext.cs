using Microsoft.EntityFrameworkCore;
using ProjectManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Data {

  public class AppDbContext : DbContext {

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Card> Cards { get; set; }

  }

}
