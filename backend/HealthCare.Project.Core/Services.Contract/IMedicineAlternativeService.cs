using HealthCare.Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Services.Contract
{
    public interface IMedicineAlternativeService
    {
        Task<List<MedicineAlternative>> GetAlternativesAsync(string medicineName);
        Task<List<MedicineAlternative>> SuggestNamesAsync(string name);
    }
    
}
