using HealthCare.Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Dtos
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dose { get; set; }
        public int Amount { get; set; }
        public int NumberOfDays { get; set; }
        public int TimesPerDay { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public List<string> ReminderTimes { get; set; } = new();
    }
}
