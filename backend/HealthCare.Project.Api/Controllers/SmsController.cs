using HealthCare.Project.Core.Dtos;
using HealthCare.Project.Core.Repository.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly ISmsHandler _smsHandler;

        public SmsController(ISmsHandler smsHandler)
        {
            _smsHandler = smsHandler;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromBody] SmsRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.PhoneNumber) || request.PhoneNumber.Length > 15)
                return BadRequest("Invalid phone number");

            var success = await _smsHandler.SendSmsAsync(request);
            return success ? Ok("Message sent") : StatusCode(500, "Failed to send message");
        }
    }
}
