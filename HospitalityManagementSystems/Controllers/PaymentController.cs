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
    public class PaymentController : ControllerBase
    {
        private readonly IPayment _payment;

        public PaymentController(IPayment payment)
        {
            _payment = payment;
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleTypes.SuperAdmin}")]
        public async Task<IActionResult> GetAllPayments()
        {
            return Ok(await _payment.GetAllPayments());
        }

        [HttpGet("mypayment")]
        [Authorize(Roles = $"{RoleTypes.SuperAdmin}")]
        public async Task<IActionResult> GetMyPayments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            return Ok(await _payment.GetAllPaymentsByPatientId(userId));
        }

        [HttpGet("patientId")]
        [Authorize(Roles = $"{RoleTypes.SuperAdmin}")]
        public async Task<IActionResult> GetPaymentByPatientId(string id)
        {
            return Ok(await _payment.GetAllPaymentsByPatientId(id));
        }

        [HttpGet("paymentid")]
        [Authorize(Roles = $"{RoleTypes.SuperAdmin}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            return Ok(await _payment.GetPaymentByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleTypes.SuperAdmin}")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            dto.CreatedBy = userId;
            dto.CreatedDate = DateTime.Now;
            return Ok(await _payment.CreatePaymentAsync(dto));
        }


        [HttpPut("id")]
        [Authorize(Roles = $"{RoleTypes.SuperAdmin}")]
        public async Task<IActionResult> UpdatePayment(int id,[FromBody] CreatePaymentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ResponseDto<bool>.Failure("Unauthorized: user id not found"));
            }
            dto.UpdatedBy = userId;
            dto.UpdatedDate = DateTime.Now;
            return Ok(await _payment.UpdatePaymentAsync(id, dto));

        }

        [HttpDelete]
        [Authorize(Roles = $"{RoleTypes.SuperAdmin}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            return Ok(await _payment.DeletePaymentAsync(id));
        }



    }
}
