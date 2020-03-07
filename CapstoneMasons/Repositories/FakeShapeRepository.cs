using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.Repositories
{
    public class FakeShapeRepository : IShapeRepository
    {
        List<Shape> shapes = new List<Shape>();

        public Task<List<Shape>> Shapes
        {
            get
            {
                return Task.FromResult<List<Shape>>(shapes);
            }
        }

        public Task<bool> AddShapeAsync(Shape s)
        {
            bool result = false;
            if (s != null)
            {
                result = true;
                shapes.Add(s);
            }
            return Task.FromResult<bool>(result);
        }

        public Task<bool> DeleteShapeAsync(Shape s)
        {
            bool result = false;
            if (s != null)
            {
                result = true;
                shapes.Remove(s);
            }
            return Task.FromResult<bool>(result);
        }

        public Task<IQueryable<Shape>> GetAllShapesAsync()
        {
            return Task.FromResult<IQueryable<Shape>>(shapes.AsQueryable<Shape>());
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
                result = true;
            }
            return Task.FromResult<bool>(result);
        }
    }
}
