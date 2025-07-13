using HealthCare.Project.Core.Dtos;
using HealthCare.Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Repository.Contract
{
    public interface IDrugInteractionRepository
    {
        Task<DrugInteraction?> GetInteractionAsync(string drug1, string drug2);
        Task<IEnumerable<string>> GetDrugSuggestionsAsync(string query);

    
    }
}
