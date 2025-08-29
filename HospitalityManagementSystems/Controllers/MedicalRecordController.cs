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
    
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecord _medicalRecord;

        public MedicalRecordController(IMedicalRecord medicalRecord)
        {
            _medicalRecord = medicalRecord;
        }


        [HttpGet]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Administrator}")]
        public async Task<IActionResult> GetAllMedicalRecords()
        {
            return Ok(await _medicalRecord.GetAllMedicalRecordsAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin},{RoleTypes.Administrator}")]
        public async Task<IActionResult> GetMedicalRecordById(int id)
        {
            return Ok(await _medicalRecord.GetMedicalRecordsByIdAsync(id));
        }

        [HttpGet("patientid")]
        [Authorize(Roles = $"{RoleTypes.User}")]
        public async Task<IActionResult> GetAllMedicalRecordsByPatientId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            return Ok(await _medicalRecord.GetAllMedicalRecordsByPatientIdAsync(userId));
        }

        [HttpGet("doctorid")]
        [Authorize(Roles = $"{RoleTypes.Doctor}")]
        public async Task<IActionResult> GetAllMedicalRecordsByDoctorId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            return Ok(await _medicalRecord.GetAllMedicalRecordsByDoctorIdAsync(userId));
        }



        [HttpPost]
        [Authorize(Roles = $"{RoleTypes.Doctor}")]
        public async Task<IActionResult> CreateMedicalRecord([FromBody] CreateMedicalRecordDto createMedicalRecord)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            createMedicalRecord.DoctorId = userId;
            return Ok(await _medicalRecord.CreateMedicalRecordAsync(createMedicalRecord));
        }

        [HttpPut("{id}")]
        //jo logjik e mir varet si e sheh bizensi kush munet me ba update dhe delete
        [Authorize(Roles = $"{RoleTypes.Doctor}")]
        public async Task<IActionResult> UpdateMedicalRecord(int id, [FromBody] CreateMedicalRecordDto createMedicalRecordDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            createMedicalRecordDto.DoctorId = userId;
            return Ok(await _medicalRecord.UpdateMedicalRecordAsync(id, createMedicalRecordDto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleTypes.Doctor}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            return Ok(await _medicalRecord.DeleteMedicalRecordAsync(id));
        }
    }
}
