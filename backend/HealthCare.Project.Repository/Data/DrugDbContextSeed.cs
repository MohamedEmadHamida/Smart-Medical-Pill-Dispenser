using HealthCare.Project.Core.Entities;
using HealthCare.Project.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare.Project.Repository.Data
{
    public class DrugDbContextSeed
    {
        public async static Task SeedAsync(DrugDbContext _context)
        {
            if (_context.DrugInteractions.Count() == 0)
            {
                // 1.Read Data From Json File

                var drugData = File.ReadAllText(@"..\HealthCare.Project.Repository\Data\DataSeed\DrugInteraction.json");

                // 2. Confert Json String To List<T>

                var drugs = JsonSerializer.Deserialize<List<DrugInteraction>>(drugData);

                // 3. Seed Data To DB
                if (drugs is not null && drugs.Count() > 0)
                {
                    await _context.DrugInteractions.AddRangeAsync(drugs);

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
