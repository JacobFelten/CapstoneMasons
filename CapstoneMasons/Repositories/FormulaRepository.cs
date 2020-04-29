using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneMasons.Repositories
{
    public class FormulaRepository : IFormulaRepository
    {
        private AppDbContext context;

        public FormulaRepository(AppDbContext appDbContext)
        {
            context = appDbContext;
        }

        public Task<List<Formula>> Formulas
        {
            get { return context.Formulas.Include(f => f.Mandrel).ToListAsync(); }
        }

        public Task<List<Mandrel>> Mandrels
        {
            get { return context.Mandrels.ToListAsync(); }
        }

        public Task<bool> AddFormulaAsync(Formula f)
        {
            bool result = false;
            if (f != null)
            {
                result = true;
                context.Formulas.Add(f);
                context.SaveChanges();
            }            
            return Task.FromResult<bool>(result);
        }

        public Task<bool> DeleteFormulaAsync(Formula f)
        {
            bool result = false;
            if (f != null)
            {
                result = true;
                context.Formulas.Remove(f);
                context.SaveChanges();
            }
            return Task.FromResult<bool>(result);
        }

        public Task<IQueryable<Formula>> GetAllFormulasAsync()
        {
            return Task.FromResult<IQueryable<Formula>>(context.Formulas.AsQueryable<Formula>());
        }

        public async Task<Formula> GetFormulaByIdAsync(int? id)
        {
            if (id != null)
            {
                foreach (Formula f in await Formulas)
                    if (f.FormulaID == id)
                        return f;
                return null;
            }
            else
            {
                return null;
            }
                
        }

        public Task<bool> UpdateFormulaAsync(Formula oldF, Formula newF)
        {
            bool result = false;
            if (oldF != null && newF != null)
            {
                oldF.BarSize = newF.BarSize;
                oldF.Degree = newF.Degree;
                oldF.Mandrel = newF.Mandrel;
                oldF.PinNumber = newF.PinNumber;
                oldF.InGained = newF.InGained;
                oldF.LastChanged = TimeZoneInfo.ConvertTime(DateTime.Now,
                 TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
                context.Formulas.Update(oldF);
                context.SaveChanges();
                result = true;
            }
            return Task.FromResult<bool>(result);
        }

        public Task<bool> AddMandrelAsync(Mandrel m)
        {
            bool result = false;
            if (m != null)
            {
                result = true;
                context.Mandrels.Add(m);
                context.SaveChanges();
            }
            return Task.FromResult<bool>(result);
        }

        public async Task<Mandrel> GetMandrelByIdAsync(int? id)
        {
            if (id != null)
            {
                foreach (Mandrel m in await Mandrels)
                    if (m.MandrelID == id)
                        return m;
                return null;
            }
            else
            {
                return null;
            }

        }
        public async Task<Mandrel> GetMandrelByNameAsync(string name)
        {
            if (name != null)
            {
                foreach (Mandrel m in await Mandrels)
                    if (m.Name == name)
                        return m;
                return null;
            }
            else
            {
                return null;
            }

        }
    }
}
