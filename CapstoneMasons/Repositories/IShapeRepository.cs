using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.Repositories
{
    public interface IShapeRepository
    {
        public Task<List<Shape>> Shapes { get; }

        public Task<bool> AddShapeAsync(int? qID, Shape s);

        public Task<bool> AddShapeLegAsync(Shape s, Leg l);

        public Task<bool> DeleteShapeAsync(Shape s);

        public Task<IQueryable<Shape>> GetAllShapesAsync();

        public Task<Shape> GetShapeByIdAsync(int? id);

        public Task<bool> UpdateShapesAsync(Shape oldS, Shape newS);
    }
}
