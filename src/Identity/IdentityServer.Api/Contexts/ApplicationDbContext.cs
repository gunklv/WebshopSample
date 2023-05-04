using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Models;

namespace IdentityServer.Contexts
{
    public class AccountDbContext : IdentityDbContext<ApplicationUser>
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Buyer", NormalizedName = "BUYER" },
                new IdentityRole { Id = "c1bfa3d5-9c80-41d1-86fb-c3b6020ade4f", Name = "Manager", NormalizedName = "MANAGER" });

        }
    }
}
