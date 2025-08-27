using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;
using HospitalityManagementSystems.Services.Interface;
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
        public async Task<IActionResult> GetAllMedicalRecords()
        {
            return Ok(await _medicalRecord.GetAllMedicalRecordsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicalRecordById(int id)
        {
            return Ok(await _medicalRecord.GetMedicalRecordsByIdAsync(id));
        }

        [HttpPost]
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
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            return Ok(await _medicalRecord.DeleteMedicalRecordAsync(id));
        }




    }
}
