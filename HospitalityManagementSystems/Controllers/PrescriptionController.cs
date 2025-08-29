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
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescription _prescription;

        public PrescriptionController(IPrescription prescription)
        {
            _prescription = prescription;
        }


        [HttpGet]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Administrator}")]
        public async Task<IActionResult> GetAllPrescription()
        {
            return Ok(await _prescription.GetAllPrescriptionsAsync());
        }

        [HttpGet("id")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Administrator}")]
        public async Task<IActionResult> GetPrescriptionById(int id)
        {
            return Ok(await _prescription.GetPrescriptionByIdAsync(id));
        }

        [HttpGet("patientId")]
        [Authorize(Roles = $"{RoleTypes.User}")]
        public async Task<IActionResult> GetPresciptionByPatientId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            return Ok(await _prescription.GetPresciptionByPatientId(userId));
        }

        [HttpGet("doctorId")]
        [Authorize(Roles = $"{RoleTypes.Doctor}")]
        public async Task<IActionResult> GetPresciptionByDocotorId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            return Ok(await _prescription.GetPresciptionByDocotorId(userId));
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleTypes.User}")]
        public async Task<IActionResult> CreatePrescrption([FromBody] CreatePrescriptionDto createPrescriptionDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            //createPrescriptionDto.
            //is not complted
            return Ok(await _prescription.CreatePrescription(createPrescriptionDto));
        }

        [HttpPut("{id}")]
        //jo logjik e mir varet si e sheh bizensi kush munet me ba update dhe delete
        [Authorize(Roles = $"{RoleTypes.Doctor}")]
        public async Task<IActionResult> UpdatePrescrption(int id, [FromBody] CreatePrescriptionDto createPrescriptionDto)
        {
            return Ok(await _prescription.UpdatePrescriptionAsync(id, createPrescriptionDto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleTypes.Doctor}")]
        public async Task<IActionResult> DeletePrescrption(int id)
        {
            return Ok(await _prescription.DeletePrescriptionAsync(id));
        }





    }
}
