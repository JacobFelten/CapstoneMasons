using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.Repositories
{
    public class FakeFormulaRepository : IFormulaRepository
    {
        List<Formula> formulas = new List<Formula>();
        List<Mandrel> mandrels = new List<Mandrel>();

        public Task<List<Formula>> Formulas
        {
            get { return Task.FromResult<List<Formula>>(formulas); }
        }

        public Task<List<Mandrel>> Mandrels
        {
            get { return Task.FromResult<List<Mandrel>>(mandrels); }
        }

        public Task<bool> AddFormulaAsync(Formula f)
        {
            bool result = false;
            if (f != null)
            {
                result = true;
                formulas.Add(f);
            }
            return Task.FromResult<bool>(result);
        }

        public Task<bool> DeleteFormulaAsync(Formula f)
        {
            bool result = false;
            if (f != null)
            {
                result = true;
                formulas.Remove(f);
            }
            return Task.FromResult<bool>(result);
        }

        public Task<IQueryable<Formula>> GetAllFormulasAsync()
        {
            return Task.FromResult<IQueryable<Formula>>(formulas.AsQueryable<Formula>());
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
                mandrels.Add(m);
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
