using HealthCare.Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Repository.Contract
{
    public interface IMedicineRepository
    {
        Task AddMedicineAsync(Medicine medicine);
        Task<IEnumerable<Medicine>> GetAllMedicineAsync(string userId);
        Task<Medicine> GetMedicineByIdAsync(int id, string userId);
        Task UpdateMedicineAsync(Medicine medicine);
        Task DeleteMedicineAsync(Medicine medicine);
        Task AddMedicineTimesAsync(IEnumerable<MedicineTime> medicineTimes);
        Task RemoveMedicineTimesAsync(int medicineId);
        Task DeleteExpiredMedicinesAsync(DateOnly currentDate);
    }
}
