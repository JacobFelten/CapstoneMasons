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
    }
}
