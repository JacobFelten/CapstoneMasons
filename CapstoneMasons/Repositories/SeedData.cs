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


            //Adds the cost seed data if none are found to exist
            if (!context.Costs.Any())
            { 
                /*Bar 3 Costs SeedData */
                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar3GlobalCost.Name,
                    Price = 3.00M
                });

                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar3BendCost.Name,
                    Price = 1.00M
                });

                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar3CutCost.Name,
                    Price = 1.00M
                });


                /*Bar 4 Costs SeedData */
                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar4GlobalCost.Name,
                    Price = 4.00M
                });

                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar4BendCost.Name,
                    Price = 2.00M
                });

                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar4CutCost.Name,
                    Price = 2.00M
                });


                /*Bar 5 Costs SeedData */
                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar5GlobalCost.Name,
                    Price = 5.00M
                });

                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar5BendCost.Name,
                    Price = 3.00M
                });

                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar5CutCost.Name,
                    Price = 3.00M
                });


                /*Bar 6 Costs SeedData */
                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar6GlobalCost.Name,
                    Price = 6.00M
                });

                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar6BendCost.Name,
                    Price = 4.00M
                });

                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.Bar6CutCost.Name,
                    Price = 4.00M
                });


                /* Fees and Charges Seed Data */

                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.SetupCharge.Name,
                    Price = 15.00M
                });

                context.Costs.Add(new Cost
                {
                    Name = KnownObjects.MinimumOrderCost.Name,
                    Price = 200.00M
                });

                context.SaveChanges();
            }
        }
    }
}
