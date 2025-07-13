using HealthCare.Project.Core.Dtos;
using HealthCare.Project.Core.Repository.Contract;
using HealthCare.Project.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Service.Services.Sms
{
    public class SmsHandler : ISmsHandler
    {
        private readonly ISmsService _smsService;

        public SmsHandler(ISmsService smsService)
        {
            _smsService = smsService;
        }

        public async Task<bool> SendSmsAsync(SmsRequestDto request)
        {
            string hardcodedMessage = "Hello,We noticed the medication wasn’t taken today. Please let us know if you need any assistance.Thank you!";
            return await _smsService.SendSmsAsync(request.PhoneNumber, hardcodedMessage);
        }
    }
}
