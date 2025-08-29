using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;
using HospitalityManagementSystems.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HospitalityManagementSystems.Data.Models;

namespace HospitalityManagementSystems.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointment _appointmentServices;

        public AppointmentsController(IAppointment appointmentServices)
        {
            _appointmentServices = appointmentServices;
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleTypes.User},{RoleTypes.Admin},{RoleTypes.Doctor},{RoleTypes.SuperAdmin},{RoleTypes.Administrator}")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var response = await _appointmentServices.GetAllAppointmentsAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Administrator}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var response = await _appointmentServices.GetAppointmentByIdAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleTypes.User}")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentsDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));

            dto.PatientId = userId;

            var response = await _appointmentServices.CreateAppointmentAsync(dto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleTypes.User}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] CreateAppointmentsDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            dto.PatientId = userId;
            var response = await _appointmentServices.UpdateAppointmentAsync(id, dto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleTypes.User},{RoleTypes.Administrator}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var response = await _appointmentServices.DeleteAppointmentAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
    }
}
