using HospitalityManagementSystems.Data;
using HospitalityManagementSystems.Data.Models;
using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;
using HospitalityManagementSystems.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalityManagementSystems.Services.Implimentation
{
    public class MedicalRecordService : IMedicalRecord
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MedicalRecordService> _logger;

        public MedicalRecordService(AppDbContext context, ILogger<MedicalRecordService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseDto<bool>> CreateMedicalRecordAsync(CreateMedicalRecordDto createMedicalRecord)
        {
            var patient = await _context.Users.FirstOrDefaultAsync(u => u.Id == createMedicalRecord.PatientId);
            if (patient == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Patient not found",
                    errors: new List<ApiError>
                    {
                new ApiError { ErrorCode = "PATIENT_NOT_FOUND", ErrorMessage = $"No patient exists with Id {createMedicalRecord.PatientId}" }
                    }
                );
            }

            var doctor = await _context.Users.FirstOrDefaultAsync(u => u.Id == createMedicalRecord.DoctorId);
            if (doctor == null)
            {
                _logger.LogInformation($"Patient with this id {createMedicalRecord.PatientId} is trying to make an appointment with this doctor id {createMedicalRecord.DoctorId}");
                return ResponseDto<bool>.Failure(
                    message: "Doctor not found",
                    errors: new List<ApiError>
                    {
                new ApiError { ErrorCode = "DOCTOR_NOT_FOUND", ErrorMessage = $"No doctor exists with Id {createMedicalRecord.DoctorId}" }
                    }
                );
            }

            var medicalrecord = new MedicalRecord
            {
                PatientId = createMedicalRecord.PatientId,
                DoctorId = createMedicalRecord.DoctorId,
                Diagnosis = createMedicalRecord.Diagnosis,
                Treatment = createMedicalRecord.Treatment,
                CreatedBy = createMedicalRecord.DoctorId,
                CreatedDate = DateTime.UtcNow
            };

            _context.MedicalRecord.Add(medicalrecord);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"MedicalRecord created with ID: {medicalrecord.Id} in this date {medicalrecord.CreatedDate} by {medicalrecord.DoctorId}");

            return ResponseDto<bool>.SuccessResponse(true, "MedicalRecord created successfully");
        }

        public async Task<ResponseDto<bool>> DeleteMedicalRecordAsync(int id)
        {
            var medical = await _context.MedicalRecord.FirstOrDefaultAsync(u => u.Id == id);

            if (medical == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "MedicalRecord not found",
                    errors: new List<ApiError> {
                        new ApiError
                        {
                            ErrorCode = "MEDICALRECORD_NOT_FOUND",
                            ErrorMessage = $"No medicalrecord exists with Id {id}"
                        }
                    }
                );
            }
            _context.MedicalRecord.Remove(medical);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"MedicalRecrod with id : {id} has been deleted by {medical.DoctorId}");

            return ResponseDto<bool>.SuccessResponse(true, "MedicalRecord has been deleted successfully");
        }

        public async Task<ResponseDto<List<MedicalRecordDto>>> GetAllMedicalRecordsAsync()
        {
            var medicals = await _context.MedicalRecord
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Prescriptions)
                .Select(a => new  MedicalRecordDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    Patient = a.Patient,
                    DoctorId = a.DoctorId,
                    Doctor = a.Doctor,
                    Diagnosis = a.Diagnosis,
                    Treatment = a.Treatment,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedDate = a.UpdatedDate,
                    Prescriptions = a.Prescriptions
                }).ToListAsync();

            return ResponseDto<List<MedicalRecordDto>>.SuccessResponse(medicals);

        }

        public async Task<ResponseDto<MedicalRecordDto>> GetMedicalRecordsByIdAsync(int id)
        {
            var medical = await _context.MedicalRecord
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Prescriptions)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (medical == null)
            {
                return ResponseDto<MedicalRecordDto>.Failure(
                    message: "MedicalRecord not found",
                    errors: new List<ApiError>
                    {
                new ApiError
                {
                    ErrorCode = "MEDICALRECORD_NOT_FOUND",
                    ErrorMessage = $"No MedicalRecord exists with Id {id}"
                }
                    }
                );
            }

            var medicalDto = new MedicalRecordDto
            {
                Id = medical.Id,
                PatientId = medical.PatientId,
                Patient = medical.Patient,
                DoctorId = medical.DoctorId,
                Doctor = medical.Doctor,
                Diagnosis = medical.Diagnosis,
                Treatment = medical.Treatment,
                CreatedBy = medical.CreatedBy,
                CreatedDate = medical.CreatedDate,
                UpdatedBy = medical.UpdatedBy,
                UpdatedDate = medical.UpdatedDate,
                Prescriptions = medical.Prescriptions
            };

            return ResponseDto<MedicalRecordDto>.SuccessResponse(medicalDto);

        }

        public async Task<ResponseDto<bool>> UpdateMedicalRecordAsync(int id, CreateMedicalRecordDto createMedicalRecord)
        {
            var medicalrecord = await _context.MedicalRecord
                .FirstOrDefaultAsync(a=>a.Id == id);

            if (medicalrecord == null)
            {
                _logger.LogInformation($"MedicalRecord with this id {id} could not be found");
                return ResponseDto<bool>.Failure(
                    message: "MedicalRecord not found",
                    errors: new List<ApiError>
                    {
                new ApiError
                {
                    ErrorCode = "MEDICALRECORD_NOT_FOUND",
                    ErrorMessage = $"No MedicalRecord exists with Id {id}"
                }
                    }
                );
            }

            var patient = await _context.Users.FirstOrDefaultAsync(u => u.Id == createMedicalRecord.PatientId);
            if (patient == null)
            {
                _logger.LogInformation($"No patient found with Id {createMedicalRecord.PatientId}");
                return ResponseDto<bool>.Failure(
                    message: "Patient not found",
                    errors: new List<ApiError>
                    {
                new ApiError
                {
                    ErrorCode = "PATIENT_NOT_FOUND",
                    ErrorMessage = $"No patient exists with Id {createMedicalRecord.PatientId}"
                }
                    }
                );
            }

            var doctor = await _context.Users.FirstOrDefaultAsync(u => u.Id == createMedicalRecord.DoctorId);
            if (doctor == null)
            {
                _logger.LogInformation($"No doctor found with Id {createMedicalRecord.DoctorId}");
                return ResponseDto<bool>.Failure(
                    message: "Doctor not found",
                    errors: new List<ApiError>
                    {
                new ApiError
                {
                    ErrorCode = "DOCTOR_NOT_FOUND",
                    ErrorMessage = $"No doctor exists with Id {createMedicalRecord.DoctorId}"
                     }
                    }
                );
            }

            medicalrecord.PatientId = createMedicalRecord.PatientId;
            medicalrecord.DoctorId = createMedicalRecord.DoctorId;
            medicalrecord.Diagnosis = createMedicalRecord.Diagnosis;
            medicalrecord.Treatment = createMedicalRecord.Treatment;
            medicalrecord.UpdatedBy = createMedicalRecord.DoctorId;
            medicalrecord.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"MedicalRecord with this id {id} is been updated successfully by {createMedicalRecord.PatientId}");
            return ResponseDto<bool>.SuccessResponse(true, "MedicalRecord updated successfully");
        }
    }
}
