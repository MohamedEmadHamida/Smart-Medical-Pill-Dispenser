using HealthCare.Project.Core.Dtos;
using HealthCare.Project.Core.Entities;
using HealthCare.Project.Service.Services.AddMedicine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthCare.Project.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly MedicineService _medicineService;

        public MedicineController(MedicineService medicineService)
        {
            _medicineService = medicineService;
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CreateMedicineDto dto)
        {
            var medicineId = await _medicineService.AddMedicineAsync(dto, GetUserId());

            return CreatedAtAction(nameof(GetById), new { id = medicineId }, new { message = "Medicine added successfully" });
        }

       
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var medicines = await _medicineService.GetAllMedicineAsync(GetUserId());
            return Ok(medicines);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var medicine = await _medicineService.GetMedicineByIdAsync(id, GetUserId());
            if (medicine == null)
                return NotFound(new { message = "Medicine not found or unauthorized" });

            return Ok(medicine);
        }

       


        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateMedicineDto dto)
        {
            await _medicineService.UpdateMedicineAsync(id, dto, GetUserId()); 
            return Ok(new { message = "Medicine updated successfully" });
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           await _medicineService.DeleteMedicineAsync(id, GetUserId());
           

            return Ok(new { message = "Medicine deleted successfully" });
        }
    }
}
