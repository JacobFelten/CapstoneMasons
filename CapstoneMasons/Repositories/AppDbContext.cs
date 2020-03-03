using CapstoneMasons.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CapstoneMasons.Repositories
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cost> Costs { get; set; }
        public DbSet<Formula> Formulas { get; set; }
        public DbSet<Leg> Legs { get; set; }
        public DbSet<Mandrel> Mandrels { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Shape> Shapes { get; set; }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            
            UserManager<IdentityUser> userManager =
                serviceProvider.GetService<UserManager<IdentityUser>>();//default user

            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Getting user info out of appsettings.json   
            string username = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
                IdentityUser user = new IdentityUser
                {
                    UserName = username,
                    //Name = username,    // Normally would be a real name, not a user name
                    Email = email
                };
                IdentityResult result = await userManager
                .CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
