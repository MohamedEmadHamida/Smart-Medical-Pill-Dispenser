using HealthCare.Project.Core.Entities;
using HealthCare.Project.Repository.Data.Contexts;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare.Project.Repository.Data
{
    public class MedicineAlternativeDbContextSeed
    {
        public async static Task SeedAsync(DrugDbContext _context)
        {
            if (!_context.MedicineAlternatives.Any())
            {
                // 1. Read Data From JSON File
                var drugData = File.ReadAllText(@"..\HealthCare.Project.Repository\Data\DataSeed\Medicine.json");

                // 2. Convert JSON String to List<MedicineAlternative>
                var medicineAlternatives = JsonSerializer.Deserialize<List<MedicineAlternative>>(drugData);

                // 3. Seed Data To DB
                if (medicineAlternatives is not null && medicineAlternatives.Count > 0)
                {
                    await _context.MedicineAlternatives.AddRangeAsync(medicineAlternatives);
                    await _context.SaveChangesAsync();
                }
            }

        }



    }
}






