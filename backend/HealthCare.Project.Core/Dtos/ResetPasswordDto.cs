using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Dtos
{
    public class ResetPasswordDto
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmedPassword { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
