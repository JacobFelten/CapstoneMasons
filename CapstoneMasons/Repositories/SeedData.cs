using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;
using CapstoneMasons.Infrastructure;

namespace CapstoneMasons.Repositories
{
    public static class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            //AppDbContext context = app.ApplicationServices.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();

            if (!context.Mandrels.Any())
            {
                context.Mandrels.Add(new Mandrel
                {
                    Name = KnownObjects.NoneMandrel.Name,
                    Radius = KnownObjects.NoneMandrel.Radius
                });

                context.Mandrels.Add(new Mandrel
                {
                    Name = KnownObjects.SmallMandrel.Name,
                    Radius = KnownObjects.SmallMandrel.Radius
                });

                context.Mandrels.Add(new Mandrel
                {
                    Name = KnownObjects.MediumMandrel.Name,
                    Radius = KnownObjects.MediumMandrel.Radius
                });

                context.Mandrels.Add(new Mandrel
                {
                    Name = KnownObjects.LargeMandrel.Name,
                    Radius = KnownObjects.LargeMandrel.Radius
                });

                context.SaveChanges();
            }
        }
    }
}
