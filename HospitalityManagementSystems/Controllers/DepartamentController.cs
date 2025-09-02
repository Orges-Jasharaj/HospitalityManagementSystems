using HospitalityManagementSystems.Data.Models;
using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;
using HospitalityManagementSystems.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalityManagementSystems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RoleTypes.SuperAdmin)]
    public class DepartamentController : ControllerBase
    {
        private readonly IDepartament _departamentService;

        public DepartamentController(IDepartament departamentService)
        {
            _departamentService = departamentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartaments()
        {
            var departaments = await _departamentService.GetAllDepartaments();
            return Ok(departaments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartamentById(int id)
        {
            var departament = await _departamentService.GetDepartamentById(id);
            if (departament == null)
            {
                return NotFound();
            }
            return Ok(departament);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartament([FromBody] CreateDepartamentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            dto.CreatedBy = userId;
            dto.CreatedAt = DateTime.UtcNow;
            return Ok(await _departamentService.CreateDepartament(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartament(int id, [FromBody] CreateDepartamentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            dto.UpdatedBy = userId;
            dto.UpdatedAt = DateTime.UtcNow;
            return Ok(await _departamentService.UpdateDepartament(id, dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartament(int id)
        {
            return Ok(await _departamentService.DeleteDepartament(id));
        }



    }
}
