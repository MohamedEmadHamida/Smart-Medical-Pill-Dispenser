using HealthCare.Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Repository.Contract
{
    public interface IMedicineAlternativeRepository
    {

            Task<MedicineAlternative> GetByNameAsync(string name);
            Task<List<MedicineAlternative>> FindByExactCompositionAsync(string composition, string excludeName);
            Task<List<MedicineAlternative>> SuggestNamesAsync(string name);

    }
}
