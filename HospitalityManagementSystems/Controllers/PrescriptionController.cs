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
    [Authorize]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescription _prescription;

        public PrescriptionController(IPrescription prescription)
        {
            _prescription = prescription;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllPrescription()
        {
            return Ok(await _prescription.GetAllPrescriptionsAsync());
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetPrescriptionById(int id)
        {
            return Ok(await _prescription.GetPrescriptionByIdAsync(id));
        }

        [HttpPost]
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
        public async Task<IActionResult> UpdatePrescrption(int id, [FromBody] CreatePrescriptionDto createPrescriptionDto)
        {
            return Ok(await _prescription.UpdatePrescriptionAsync(id, createPrescriptionDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescrption(int id)
        {
            return Ok(await _prescription.DeletePrescriptionAsync(id));
        }


    }
}
