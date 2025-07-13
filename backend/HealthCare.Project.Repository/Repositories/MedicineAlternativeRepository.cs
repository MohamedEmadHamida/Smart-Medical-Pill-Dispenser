using HealthCare.Project.Core.Entities;
using HealthCare.Project.Core.Repository.Contract;
using HealthCare.Project.Repository.Data.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare.Project.Repository.Repositories
{
    public class MedicineAlternativeRepository : IMedicineAlternativeRepository
    {
        private readonly DrugDbContext _context;

        public MedicineAlternativeRepository(DrugDbContext context)
        {
            _context = context;
        }

        public async Task<MedicineAlternative> GetByNameAsync(string name)
        {
            return await _context.MedicineAlternatives
                .FirstOrDefaultAsync(m => m.Name != null && EF.Functions.Like(m.Name, $"%{name}%"));
        }

        public async Task<List<MedicineAlternative>> FindByExactCompositionAsync(string composition, string excludeName)
        {
            var normalizedComposition = NormalizeComposition(composition);
            var excludeNameLower = excludeName.ToLower().Trim();

            var allCandidates = await _context.MedicineAlternatives
                .Where(m =>
                    m.Name != null &&
                    m.Composition != null &&
                    !m.Name.ToLower().Contains(excludeNameLower))
                .ToListAsync();


            return allCandidates
                .Where(m => NormalizeComposition(m.Composition) == normalizedComposition)
                .ToList();
        }

        private string NormalizeComposition(string input)
        {
            return string.Join(" ", input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }


        public async Task<List<MedicineAlternative>> SuggestNamesAsync(string name)
        {
            // Convert the entered text to lowercase letters only
            var normalizedInput = name.ToLower();

            // Search for names that begin with the entered letters, ignoring capital and lowercase letters.
            return await _context.MedicineAlternatives
                .Where(m => m.Name != null && m.Name.ToLower().StartsWith(normalizedInput)) // Ensure the name begins with the entered letters.
                .ToListAsync();
        }
    }
}
