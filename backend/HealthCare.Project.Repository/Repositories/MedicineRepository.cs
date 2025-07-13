using HealthCare.Project.Core.Entities;
using HealthCare.Project.Core.Repository.Contract;
using HealthCare.Project.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Repository.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly DrugDbContext _context;

        public MedicineRepository(DrugDbContext context)
        {
            _context = context;
        }

        public async Task AddMedicineAsync(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Medicine>> GetAllMedicineAsync(string userId)
        {
            await DeleteExpiredMedicinesAsync(DateOnly.FromDateTime(DateTime.UtcNow));
            return await _context.Medicines
                .Include(m => m.MedicineTimes)
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }

        public async Task<Medicine> GetMedicineByIdAsync(int id, string userId)
        {
            return await _context.Medicines
                .Include(m => m.MedicineTimes)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
        }

        public async Task UpdateMedicineAsync(Medicine medicine)
        {
            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMedicineAsync(Medicine medicine)
        {
            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
        }

        public async Task AddMedicineTimesAsync(IEnumerable<MedicineTime> medicineTimes)
        {
            await _context.MedicineTimes.AddRangeAsync(medicineTimes);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMedicineTimesAsync(int medicineId)
        {
            var times = await _context.MedicineTimes.Where(mt => mt.MedicineId == medicineId).ToListAsync();
            _context.MedicineTimes.RemoveRange(times);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteExpiredMedicinesAsync(DateOnly currentDate)
        {
            var expiredMedicines = await _context.Medicines
                .Where(m => m.EndDate < currentDate)
                .ToListAsync();

            if (expiredMedicines.Any())
            {
                _context.Medicines.RemoveRange(expiredMedicines);
                await _context.SaveChangesAsync();
            }
        }
    }
}

