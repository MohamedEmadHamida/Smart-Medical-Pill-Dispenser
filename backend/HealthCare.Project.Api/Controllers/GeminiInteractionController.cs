using HealthCare.Project.Core.Repository.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiInteractionController : ControllerBase
    {
        private readonly IInteractionChecker _interactionChecker;

        public GeminiInteractionController(IInteractionChecker interactionChecker)
        {
            _interactionChecker = interactionChecker;
        }

        [HttpGet("Check")]
        public async Task<IActionResult> CheckInteraction(
            [FromQuery(Name = "med1")] string medicine1,
            [FromQuery(Name = "med2")] string medicine2)
        {
            if (string.IsNullOrWhiteSpace(medicine1) || string.IsNullOrWhiteSpace(medicine2))
                return BadRequest("You must provide exactly two medicine names as query parameters: 'med1' and 'med2'.");

            var result = await _interactionChecker.CheckInteractionAsync(medicine1, medicine2);

            return Ok(new
            {
                Medicine1 = medicine1,
                Medicine2 = medicine2,
                result.InteractionFound,
                Cause = result.Description
            });
        }
    }
}

