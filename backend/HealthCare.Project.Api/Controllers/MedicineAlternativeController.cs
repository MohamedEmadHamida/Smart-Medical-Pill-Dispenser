using HealthCare.Project.Core.Entities;
using HealthCare.Project.Core.Services.Contract;
using HealthCare.Project.Service.Services.Alternative;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineAlternativeController : ControllerBase
    {
        private readonly IMedicineAlternativeService _service;

        public MedicineAlternativeController(IMedicineAlternativeService service)
        {
            _service = service;
        }

        [HttpGet("alternatives")]
        public async Task<IActionResult> GetAlternatives([FromQuery] string name)
        {
            var result = await _service.GetAlternativesAsync(name);
            if (result == null || result.Count == 0)
                return NotFound("No alternatives found.");

            return Ok(result);
        }

        [HttpGet("suggest")]
        public async Task<ActionResult<List<MedicineAlternative>>> SuggestNames(string name)
        {
            var result = await _service.SuggestNamesAsync(name);

            if (result == null || result.Count == 0)
            {
                return NotFound("No suggestions found.");
            }

            return Ok(result);
        }
    }
}

