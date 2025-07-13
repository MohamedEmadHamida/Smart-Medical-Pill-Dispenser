using HealthCare.Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Dtos
{
    public class CreateMedicineDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Dose { get; set; }
        [Required]

        public int Amount { get; set; }
        [Required]

        public int NumberOfDays { get; set; }
        [Required]

        public int TimesPerDay { get; set; }
        [Required]

        public DateOnly StartDate { get; set; }
        [Required]

        public DateOnly EndDate { get; set; }

        [Required]
        
        public List<string> ReminderTimes { get; set; } = new();
    }
}
