using HealthCare.Project.Core.Dtos;
using HealthCare.Project.Core.Entities;
using HealthCare.Project.Core.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HealthCare.Project.Service.Services.AddMedicine.MedicineService;

namespace HealthCare.Project.Service.Services.AddMedicine
{
    public class MedicineService
    {
        private readonly IMedicineRepository _medicineRepository;

        public MedicineService(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        public async Task<int> AddMedicineAsync(CreateMedicineDto dto, string userId)
        {
            if (userId == null)
                return -1;
            if (dto.ReminderTimes.Count != dto.TimesPerDay)
                throw new ArgumentException("Number of reminder times must match TimesPerDay.");

            var medicine = new Medicine
            {
                Name = dto.Name,
                Dose = dto.Dose,
                Amount = dto.Amount,
                NumberOfDays = dto.NumberOfDays,
                TimesPerDay = dto.TimesPerDay,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                UserId = userId
            };



            await _medicineRepository.AddMedicineAsync(medicine);

            if (medicine.Id <= 0)
                throw new Exception("Failed to save medicine");

            var medicineTimes = dto.ReminderTimes
                .Select(time => new MedicineTime
                {
                    MedicineId = medicine.Id,
                    ReminderTime = TimeOnly.ParseExact(time.ToString(), "h:mm tt", CultureInfo.InvariantCulture)
                }).ToList();

            await _medicineRepository.AddMedicineTimesAsync(medicineTimes);

            return medicine.Id;
        }

        public async Task<IEnumerable<MedicineDto>> GetAllMedicineAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return new List<MedicineDto>();
            var medicines = await _medicineRepository.GetAllMedicineAsync(userId);

            return medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Dose = m.Dose,
                Amount = m.Amount,
                NumberOfDays = m.NumberOfDays,
                TimesPerDay = m.TimesPerDay,
                StartDate = m.StartDate,
                EndDate = m.EndDate,
                ReminderTimes = m.MedicineTimes
                    .Select(mt => mt.ReminderTime.ToString("h:mm tt"))
                    .ToList()
            });
        }

        public async Task<MedicineDto> GetMedicineByIdAsync(int id, string userId)
        {
            var medicine = await _medicineRepository.GetMedicineByIdAsync(id, userId);
            if (medicine == null) return null;

            return new MedicineDto
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Dose = medicine.Dose,
                Amount = medicine.Amount,
                NumberOfDays = medicine.NumberOfDays,
                TimesPerDay = medicine.TimesPerDay,
                StartDate = medicine.StartDate,
                EndDate = medicine.EndDate,
                ReminderTimes = medicine.MedicineTimes
                    .Select(mt => mt.ReminderTime.ToString("h:mm tt"))
                    .ToList()
            };
        }

        public async Task UpdateMedicineAsync(int id, CreateMedicineDto dto, string userId)
        {
            var medicine = await _medicineRepository.GetMedicineByIdAsync(id, userId);
            if (medicine == null)
                throw new KeyNotFoundException("Medicine not found.");

            if (medicine.UserId != userId)
                throw new UnauthorizedAccessException("You do not have permission to update this medicine.");

            if (dto.ReminderTimes.Count != dto.TimesPerDay)
                throw new ArgumentException("Number of reminder times must match TimesPerDay.");

            medicine.Name = dto.Name;
            medicine.Dose = dto.Dose;
            medicine.Amount = dto.Amount;
            medicine.NumberOfDays = dto.NumberOfDays;
            medicine.TimesPerDay = dto.TimesPerDay;
            medicine.StartDate = dto.StartDate;
            medicine.EndDate = dto.EndDate;

            await _medicineRepository.UpdateMedicineAsync(medicine);
            await _medicineRepository.RemoveMedicineTimesAsync(id);

            var medicineTimes = dto.ReminderTimes
                .Select(time => new MedicineTime
                {
                    MedicineId = medicine.Id,
                    ReminderTime = TimeOnly.ParseExact(time.ToString(), "h:mm tt", CultureInfo.InvariantCulture)
                }).ToList();

            await _medicineRepository.AddMedicineTimesAsync(medicineTimes);
        }

        public async Task DeleteMedicineAsync(int id, string userId)
        {
            var medicine = await _medicineRepository.GetMedicineByIdAsync(id, userId);
            if (medicine == null) return;

            await _medicineRepository.RemoveMedicineTimesAsync(id);
            await _medicineRepository.DeleteMedicineAsync(medicine);
        }

     





    }
}

