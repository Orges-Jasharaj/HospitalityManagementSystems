using HospitalityManagementSystems.Data;
using HospitalityManagementSystems.Data.Models;
using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;
using HospitalityManagementSystems.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalityManagementSystems.Services.Implimentation
{
    public class PrescriptionService : IPrescription
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PrescriptionService> _logger;

        public PrescriptionService(AppDbContext context, ILogger<PrescriptionService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<ResponseDto<bool>> CreatePrescription(CreatePrescriptionDto createPrescriptionDto)
        {
            var medicalrecord = await _context.MedicalRecord.FirstOrDefaultAsync(u => u.Id == createPrescriptionDto.MedicalRecordId);
            if (medicalrecord == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "MedicalRecord not found",
                    errors: new List<ApiError>
                    {
                        new ApiError{ ErrorCode = "MEDICALRECORD_NOT_FOUND", ErrorMessage = $"No medicalrecord found with this id {createPrescriptionDto.MedicalRecordId}"  }
                    }
                );
            }
            var prescription = new Prescription
            {
                MedicalRecordId = createPrescriptionDto.MedicalRecordId,
                MedicineName = createPrescriptionDto.MedicineName,
                Dosage = createPrescriptionDto.Dosage,
                Instructions = createPrescriptionDto.Instructions,
                CreatedBy = medicalrecord.DoctorId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = medicalrecord.DoctorId,
                UpdatedDate = DateTime.UtcNow
            };

            _context.Prescription.Add(prescription);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Prescrption created with ID: {prescription.Id} in this date {prescription.CreatedDate} by {medicalrecord.DoctorId}");
            return ResponseDto<bool>.SuccessResponse(true, "Prescrption created successfully");
        }

        public async Task<ResponseDto<bool>> DeletePrescriptionAsync(int id)
        {
            var prescrption = await _context.Prescription.FirstOrDefaultAsync(u => u.Id == id);
            if (prescrption != null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Prescrption not found",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode ="PRESCRPTION_NOT_FOUND",
                            ErrorMessage=$"No prescrption exisist with id {id}"
                        }
                    }
                );
            }
            _context.Prescription.Remove(prescrption);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Deleted {prescrption.Id}");

            return ResponseDto<bool>.SuccessResponse(true, "Prescrption has been deleted successfully");
        }

        public async Task<ResponseDto<List<PrescriptionDto>>> GetAllPrescriptionsAsync()
        {
            var prescrption = await _context.Prescription
                .Select(a => new PrescriptionDto
                {
                    Id = a.Id,
                    MedicalRecordId = a.MedicalRecordId,
                    MedicalRecord = a.MedicalRecord,
                    MedicineName = a.MedicineName,
                    Dosage = a.Dosage,
                    Instructions = a.Instructions,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedDate = a.UpdatedDate
                }).ToListAsync();
            return ResponseDto<List<PrescriptionDto>>.SuccessResponse(prescrption);
        }

        public async Task<ResponseDto<List<PrescriptionDto>>> GetPresciptionByPatientId(string patientId)
        {
            var prescriptions = await _context.Prescription
                .Include(p => p.MedicalRecord)
                .Where(p => p.MedicalRecord.PatientId == patientId)
                .Select(p => new PrescriptionDto
                {
                    Id = p.Id,
                    MedicalRecordId = p.MedicalRecordId,
                    MedicalRecord = p.MedicalRecord,
                    MedicineName = p.MedicineName,
                    Dosage = p.Dosage,
                    Instructions = p.Instructions,
                    CreatedBy = p.CreatedBy,
                    CreatedDate = p.CreatedDate,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedDate = p.UpdatedDate
                })
                .ToListAsync();

            if (!prescriptions.Any())
            {
                return ResponseDto<List<PrescriptionDto>>.Failure(
                    "No prescriptions found for this patient"
                );
            }

            return ResponseDto<List<PrescriptionDto>>.SuccessResponse(prescriptions);
        }


        public async Task<ResponseDto<List<PrescriptionDto>>> GetPresciptionByDocotorId(string doctorId)
        {
            var prescriptions = await _context.Prescription
                .Include(p => p.MedicalRecord)
                .Where(p => p.MedicalRecord.DoctorId == doctorId)
                .Select(p => new PrescriptionDto
                {
                    Id = p.Id,
                    MedicalRecordId = p.MedicalRecordId,
                    MedicalRecord = p.MedicalRecord,
                    MedicineName = p.MedicineName,
                    Dosage = p.Dosage,
                    Instructions = p.Instructions,
                    CreatedBy = p.CreatedBy,
                    CreatedDate = p.CreatedDate,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedDate = p.UpdatedDate
                })
                .ToListAsync();

            if (!prescriptions.Any())
            {
                return ResponseDto<List<PrescriptionDto>>.Failure(
                    "No prescriptions found for this doctor"
                );
            }

            return ResponseDto<List<PrescriptionDto>>.SuccessResponse(prescriptions);
        }


        public async Task<ResponseDto<PrescriptionDto>> GetPrescriptionByIdAsync(int id)
        {
            var prescrption = await _context.Prescription
                .FirstOrDefaultAsync(a => a.Id == id);
            if (prescrption == null)
            {
                return ResponseDto<PrescriptionDto>.Failure(
                    message: "Prescrption not found",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "PRESCRIPTION_NOT_FOUND",
                            ErrorMessage =$"NO prescrption exisist with Id{id}"
                        }
                    }
                );
            }

            var prescrptiondto = new PrescriptionDto
            {
                Id = prescrption.Id,
                MedicalRecordId = prescrption.MedicalRecordId,
                MedicalRecord = prescrption.MedicalRecord,
                MedicineName = prescrption.MedicineName,
                Dosage = prescrption.Dosage,
                Instructions = prescrption.Instructions,
                CreatedBy = prescrption.CreatedBy,
                CreatedDate = prescrption.CreatedDate,
                UpdatedBy = prescrption.UpdatedBy,
                UpdatedDate = prescrption.UpdatedDate
            };

            return ResponseDto<PrescriptionDto>.SuccessResponse(prescrptiondto);
        }

        public async Task<ResponseDto<bool>> UpdatePrescriptionAsync(int id, CreatePrescriptionDto createPrescriptionDto)
        {
            var prescrption = await _context.Prescription
                .FirstOrDefaultAsync(a => a.Id == id);
            if (prescrption == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Prescrption not found",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "PRESCRIPTION_NOT_FOUND",
                            ErrorMessage =$"NO prescrption exisist with Id{id}"
                        }
                    }
                );
            }

            prescrption.MedicalRecordId = createPrescriptionDto.MedicalRecordId;
            prescrption.MedicineName = createPrescriptionDto.MedicineName;
            prescrption.Dosage = createPrescriptionDto.Dosage;
            prescrption.Instructions = createPrescriptionDto.Instructions;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Prescrption with this id {id} is been updated successfully at {prescrption.UpdatedDate} from {prescrption.UpdatedBy}");
            return ResponseDto<bool>.SuccessResponse(true, "Presciptipn updated successfully");
        }
    }
}
