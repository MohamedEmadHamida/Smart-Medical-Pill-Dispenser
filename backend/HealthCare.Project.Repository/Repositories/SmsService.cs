using HealthCare.Project.Core.Services.Contract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace HealthCare.Project.Repository.Repositories
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _config;

        public SmsService(IConfiguration config)
        {
            _config = config;
            TwilioClient.Init(
                _config["Twilio:AccountSID"],
                _config["Twilio:AuthToken"]);
        }

        public async Task<bool> SendSmsAsync(string toPhoneNumber, string message)
        {
            var result = await MessageResource.CreateAsync(
                to: new PhoneNumber(toPhoneNumber),
                from: new PhoneNumber(_config["Twilio:FromPhoneNumber"]),
                body: message
            );

            return result.ErrorCode == null;
        }
    }
}
