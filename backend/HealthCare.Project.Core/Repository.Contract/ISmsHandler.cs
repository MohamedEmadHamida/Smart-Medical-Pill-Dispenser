using HealthCare.Project.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Repository.Contract
{
    public interface ISmsHandler
    {
        Task<bool> SendSmsAsync(SmsRequestDto request);
    }
}
