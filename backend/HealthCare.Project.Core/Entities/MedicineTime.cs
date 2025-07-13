using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Entities
{
    public class MedicineTime
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeOnly ReminderTime { get; set; }

        public Medicine Medicine { get; set; }
    }
}
