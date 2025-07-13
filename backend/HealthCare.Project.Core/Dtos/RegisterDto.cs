using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "UserName Is Required!")]
        public string UserName { get; set; }

        [EmailAddress] 
        [Required(ErrorMessage = "Email Is Required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required!")]
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
