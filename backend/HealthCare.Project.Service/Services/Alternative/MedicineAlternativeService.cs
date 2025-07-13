using HealthCare.Project.Core.Entities;
using HealthCare.Project.Core.Repository.Contract;
using HealthCare.Project.Core.Services.Contract;
using HealthCare.Project.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Service.Services.Alternative
{
    public class MedicineAlternativeService : IMedicineAlternativeService
    {
        private readonly IMedicineAlternativeRepository _repository;

        public MedicineAlternativeService(IMedicineAlternativeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MedicineAlternative>> GetAlternativesAsync(string medicineName)
        {
            var target = await _repository.GetByNameAsync(medicineName);
            if (target == null) return new List<MedicineAlternative>();

            return await _repository.FindByExactCompositionAsync(target.Composition, medicineName);
        }
        public async Task<List<MedicineAlternative>> SuggestNamesAsync(string name)
        {
           
            var normalizedInput = name.ToLower();

            
            var allMatches = await _repository.SuggestNamesAsync(normalizedInput);

            
            var sortedMatches = allMatches
                .Where(m => m.Name != null && m.Name.ToLower().StartsWith(normalizedInput)) 
                .ToList();

            return sortedMatches;
        }

    }
}


