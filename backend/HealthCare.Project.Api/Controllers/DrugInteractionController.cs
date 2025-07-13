using HealthCare.Project.Service.Services.DrugInteractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Project.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DrugInteractionController : ControllerBase
    {
        private readonly DrugInteractionServices _services;

        public DrugInteractionController(DrugInteractionServices services)
        {
            _services = services;
        }


        [HttpGet("interaction")]
        public async Task<IActionResult> GetInteraction([FromQuery] string drug1, [FromQuery] string drug2)
        {
            var interaction = await _services.GetInteractionAsync(drug1, drug2);
            if (interaction == null)
            {
                return NotFound("No Interaction Found.");
            }
            return Ok(interaction);
        }

        [HttpGet("autocomplete")]
        public async Task<IActionResult> GetDrugSuggestions([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            var suggestions = await _services.GetDrugSuggestionsAsync(query);

            if (suggestions == null || !suggestions.Any())
            {
                return NotFound("No suggestions found.");
            }

            return Ok(suggestions);
        }

    }
}
