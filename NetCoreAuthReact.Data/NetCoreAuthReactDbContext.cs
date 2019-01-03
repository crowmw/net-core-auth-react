using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreAuthReact.Data.Models;

namespace NetCoreAuthReact.Data
{
    public class NetCoreAuthReactDbContext : IdentityDbContext
    {
        public NetCoreAuthReactDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
