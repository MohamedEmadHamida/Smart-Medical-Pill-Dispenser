using HealthCare.Project.Core.Entities;
using HealthCare.Project.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Repository.Data.Contexts
{
    public class DrugDbContext : DbContext
    {
        public DrugDbContext(DbContextOptions<DrugDbContext> options) : base(options)
        {

        }
        public DbSet<DrugInteraction> DrugInteractions { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineTime> MedicineTimes { get; set; }
        public DbSet<MedicineAlternative> MedicineAlternatives { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder); 


        }
    }
}
