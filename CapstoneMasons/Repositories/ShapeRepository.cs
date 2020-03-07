using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneMasons.Repositories
{
    public class ShapeRepository : IShapeRepository
    {
        private AppDbContext context;

        public ShapeRepository(AppDbContext appDbContext)
        {
            context = appDbContext;
        }

        public Task<List<Shape>> Shapes
        {
            get 
            { 
                return context.Shapes
                    .Include(s => s.Legs)
                        .ThenInclude(l => l.Mandrel)
                    .ToListAsync(); 
            }
        }

        public Task<bool> AddShapeAsync(Shape s)
        {
            bool result = false;
            if (s != null)
            {
                result = true;
                context.Shapes.Add(s);
                context.SaveChanges();
            }
            return Task.FromResult<bool>(result);
        }

        public Task<bool> DeleteShapeAsync(Shape s)
        {
            bool result = false;
            if (s != null)
            {
                result = true;
                context.Shapes.Remove(s);
                context.SaveChanges();
            }
            return Task.FromResult<bool>(result);
        }

        public Task<IQueryable<Shape>> GetAllShapesAsync()
        {
            return Task.FromResult<IQueryable<Shape>>(context.Shapes.AsQueryable<Shape>());
        }

        public async Task<Shape> GetShapeByIdAsync(int? id)
        {
            if (id != null)
            {
                foreach (Shape s in await Shapes)
                    if (s.ShapeID == id)
                        return s;
                return null;
            }
            else
            {
                return null;
            }

        }

        public Task<bool> UpdateShapesAsync(Shape oldS, Shape newS)
        {
            bool result = false;
            if (oldS != null && newS != null)
            {
                oldS.BarSize = newS.BarSize;
                oldS.LegCount = newS.LegCount;
                oldS.Legs.Clear();
                foreach (Leg l in newS.Legs)
                    oldS.Legs.Add(l);
                oldS.Qty = newS.Qty;
                oldS.NumCompleted = newS.NumCompleted;
                context.Shapes.Update(oldS);
                context.SaveChanges();
                result = true;
            }
            return Task.FromResult<bool>(result);
        }
    }
}
