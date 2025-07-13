using HealthCare.Project.Core.Entities;
using HealthCare.Project.Core.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Service.Services.DrugInteractions
{
    public class DrugInteractionServices
    {
        private readonly IDrugInteractionRepository _repository;

        public DrugInteractionServices(IDrugInteractionRepository repository)
        {
            _repository = repository;
        }

        public Task<DrugInteraction?> GetInteractionAsync(string drug1, string drug2)
        {
            return _repository.GetInteractionAsync(drug1, drug2);
        }

        public async Task<IEnumerable<string>> GetDrugSuggestionsAsync(string query)
        {
            return await _repository.GetDrugSuggestionsAsync(query);
        }
    }
}
