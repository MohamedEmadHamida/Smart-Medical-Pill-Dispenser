using HealthCare.Project.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Services.Contract
{
    public interface IEmailService
    {
        void SendEmail(EmailDto email);
    }
}
