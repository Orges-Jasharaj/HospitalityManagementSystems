using HospitalityManagementSystems.Data;
using HospitalityManagementSystems.Data.Models;
using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;
using HospitalityManagementSystems.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalityManagementSystems.Services.Implimentation
{
    public class PaymentService : IPayment
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(AppDbContext appDbContext, ILogger<PaymentService> logger)
        {
            _appDbContext= appDbContext;
            _logger= logger;
        }

        public async Task<ResponseDto<bool>> CreatePaymentAsync(CreatePaymentDto createPaymentDto)
        {
            var medicalrecord = await _appDbContext.MedicalRecord.FirstOrDefaultAsync(a=> a.Id == createPaymentDto.MedicalRecordId);
            if (medicalrecord == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "MedicalRecord not found",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "MEDICALRECORD_NOT_FOUND",
                            ErrorMessage = $"No medicalrecord found with this id {createPaymentDto.MedicalRecordId}"
                        }
                    }
                );
            }

            var payment = new Payment
            {
                MedicalRecordId = createPaymentDto.MedicalRecordId,
                Amount = createPaymentDto.Amount,
                PaymentMethod = createPaymentDto.PaymentMethod,
                Status = createPaymentDto.Status,
                Notes = createPaymentDto.Notes,
                CreatedBy = createPaymentDto.CreatedBy,
                CreatedDate = createPaymentDto.CreatedDate
            };

            _appDbContext.Payments.Add( payment );
            await _appDbContext.SaveChangesAsync();
            _logger.LogInformation($"Payment created with ID : {payment.Id} in this");
            return ResponseDto<bool>.SuccessResponse(true, "Payment created successfully");

        }

        public async Task<ResponseDto<bool>> DeletePaymentAsync(int id)
        {
            var payment = await _appDbContext.Payments.FirstOrDefaultAsync( a => a.Id == id);
            if ( payment == null )
            {
                return ResponseDto<bool>.Failure(
                    message: "Payment not found",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "PAYMENT_NOT_FOUND",
                            ErrorMessage = $"No payment exists with Id {id}"
                        }
                    }
                );
            }

            _appDbContext.Payments.Remove( payment );
            await _appDbContext.SaveChangesAsync();
            _logger.LogInformation($"Payment with id : {id}");
            return ResponseDto<bool>.SuccessResponse(true, "Payment has been deleted successfully");
        }

        public async Task<ResponseDto<List<PaymentDto>>> GetAllPayments()
        {
            var payment = await _appDbContext.Payments
                .Select(a => new PaymentDto
                {
                    Id = a.Id,
                    MedicalRecordId = a.MedicalRecordId,
                    MedicalRecord = a.MedicalRecord,
                    Amount = a.Amount,
                    PaymentMethod = a.PaymentMethod,
                    Status = a.Status,
                    CompletedDate = a.CompletedDate,
                    Notes = a.Notes,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedDate = a.UpdatedDate,
                }).ToListAsync();

            return ResponseDto<List<PaymentDto>>.SuccessResponse(payment);
        }

        public async Task<ResponseDto<List<PaymentDto>>> GetAllPaymentsByPatientId(string id)
        {
            var payments = await _appDbContext.Payments
                .Where(p => p.MedicalRecord.PatientId == id)
                .Select(a => new PaymentDto
                {
                    Id = a.Id,
                    MedicalRecordId = a.MedicalRecordId,
                    Amount = a.Amount,
                    PaymentMethod = a.PaymentMethod,
                    Status = a.Status,
                    CompletedDate = a.CompletedDate,
                    Notes = a.Notes,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedDate = a.UpdatedDate,
                }).ToListAsync();

            return ResponseDto<List<PaymentDto>>.SuccessResponse(payments);
        }


        public async Task<ResponseDto<PaymentDto>> GetPaymentByIdAsync(int id)
        {
            var payment = await _appDbContext.Payments
                .Include(p => p.MedicalRecord)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return ResponseDto<PaymentDto>.Failure(
                    "Payment not found",
                    new List<ApiError> { new ApiError { ErrorCode = "NOT_FOUND", ErrorMessage = $"Payment with id {id} not found" } }
                );
            }

            var dto = new PaymentDto
            {
                Id = payment.Id,
                MedicalRecordId = payment.MedicalRecordId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status,
                CompletedDate = payment.CompletedDate,
                Notes = payment.Notes,
                CreatedBy = payment.CreatedBy,
                CreatedDate = payment.CreatedDate,
                UpdatedBy = payment.UpdatedBy,
                UpdatedDate = payment.UpdatedDate,
            };

            return ResponseDto<PaymentDto>.SuccessResponse(dto);
        }


        public async Task<ResponseDto<bool>> UpdatePaymentAsync(int id, CreatePaymentDto updatePaymentDto)
        {
            var payment = await _appDbContext.Payments.FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null)
            {
                return ResponseDto<bool>.Failure(
                    "Payment not found",
                    new List<ApiError> { new ApiError { ErrorCode = "NOT_FOUND", ErrorMessage = $"Payment with id {id} not found" } }
                );
            }

            payment.Amount = updatePaymentDto.Amount;
            payment.PaymentMethod = updatePaymentDto.PaymentMethod;
            payment.Status = updatePaymentDto.Status;
            payment.Notes = updatePaymentDto.Notes;
            payment.UpdatedBy = updatePaymentDto.UpdatedBy;
            payment.UpdatedDate = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();
            _logger.LogInformation($"Payment with ID {id} updated successfully");

            return ResponseDto<bool>.SuccessResponse(true, "Payment updated successfully");
        }

    }
}
