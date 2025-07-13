using HealthCare.Project.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Entities
{
    public class Medicine
    {
        
        public int Id { get; set; }

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


        public string UserId { get; set; }
        [Required]
        public List<MedicineTime> MedicineTimes { get; set; } = new();
    }

    }

