using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneMasons.Models;

namespace CapstoneMasons.Repositories
{
    public interface IFormulaRepository
    {
        public Task<List<Formula>> Formulas { get; }

        public Task<List<Mandrel>> Mandrels { get; }

        public Task<bool> AddFormulaAsync(Formula f);

        public Task<bool> DeleteFormulaAsync(Formula f);

        public Task<IQueryable<Formula>> GetAllFormulasAsync();

        public Task<Formula> GetFormulaByIdAsync(int? id);

        public Task<bool> UpdateFormulaAsync(Formula oldF, Formula newF);

        public Task<bool> AddMandrelAsync(Mandrel m);

        public Task<Mandrel> GetMandrelByIdAsync(int? id);

        public Task<Mandrel> GetMandrelByNameAsync(string name);
    }
}
