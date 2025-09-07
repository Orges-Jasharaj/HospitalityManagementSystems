using HospitalityManagementSystems.Data;
using HospitalityManagementSystems.Data.Models;
using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;
using HospitalityManagementSystems.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HospitalityManagementSystems.Services.Implimentation
{
    public class AppointmentService : IAppointment
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(AppDbContext context, ILogger<AppointmentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseDto<bool>> CreateAppointmentAsync(CreateAppointmentsDto createAppointmentsDto)
        {
            var patient = await _context.Users.FirstOrDefaultAsync(u => u.Id == createAppointmentsDto.PatientId);
            if (patient == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Patient not found",
                    errors: new List<ApiError>
                    {
                        new ApiError { ErrorCode = "PATIENT_NOT_FOUND", ErrorMessage = $"No patient exists with Id {createAppointmentsDto.PatientId}" }
                    }
                );
            }

            var doctor = await _context.Users.FirstOrDefaultAsync(u => u.Id == createAppointmentsDto.DoctorId);
            if (doctor == null)
            {
                _logger.LogInformation($"Patient with this id {createAppointmentsDto.PatientId} is trying to make an appointment with this doctor id {createAppointmentsDto.DoctorId}");
                return ResponseDto<bool>.Failure(
                    message: "Doctor not found",
                    errors: new List<ApiError>
                    {
                        new ApiError { ErrorCode = "DOCTOR_NOT_FOUND", ErrorMessage = $"No doctor exists with Id {createAppointmentsDto.DoctorId}" }
                    }
                );
            }

            // kontrollo overlaping (30 minuta)
            var startTime = createAppointmentsDto.AppointmentDate;
            var endTime = startTime.AddMinutes(30);

            var isOverlapping = await _context.Appointments.AnyAsync(a =>
                a.DoctorId == createAppointmentsDto.DoctorId &&
                (a.AppointmentDate < endTime) &&
                (a.AppointmentDate.AddMinutes(30) > startTime) &&
                a.Status != "Cancelled"
            );

            if (isOverlapping)
            {
                return ResponseDto<bool>.Failure(
                    message: "Doctor already has an appointment in this time slot",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "TIME_CONFLICT",
                            ErrorMessage = $"Doctor already has an appointment between {startTime} and {endTime}"
                        }
                    }
                );
            }

            var appointment = new Appointments
            {
                PatientId = createAppointmentsDto.PatientId,
                DoctorId = createAppointmentsDto.DoctorId,
                AppointmentDate = createAppointmentsDto.AppointmentDate,
                Reason = createAppointmentsDto.Reason,
                Status = createAppointmentsDto.Status,
                CreatedBy = createAppointmentsDto.PatientId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Appointment created with ID: {appointment.Id} in this date {appointment.CreatedDate} by {appointment.PatientId}");
            return ResponseDto<bool>.SuccessResponse(true, "Appointment created successfully");
        }

        public async Task<ResponseDto<bool>> DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(u => u.Id == id);
            if (appointment == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Appointment not found",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "APPOINTMENT_NOT_FOUND",
                            ErrorMessage = $"No appointment exists with Id {id}"
                        }
                    }
                );
            }
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Appointment with id : {id} has been deleted by {appointment.PatientId}");

            return ResponseDto<bool>.SuccessResponse(true, "Appointment has been deleted successfully");
        }

        public async Task<ResponseDto<List<AppointmentsDto>>> GetAllAppointmentsAsync()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Select(a => new AppointmentsDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    Patient = a.Patient,
                    DoctorId = a.DoctorId,
                    Doctor = a.Doctor,
                    AppointmentDate = a.AppointmentDate,
                    Reason = a.Reason,
                    Status = a.Status,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedDate = a.UpdatedDate,
                }).ToListAsync();

            return ResponseDto<List<AppointmentsDto>>.SuccessResponse(appointments);
        }

        public async Task<ResponseDto<List<AppointmentsDto>>> GetAllAppointmentsByDoctorIdAsync(string doctorId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId)
                .Select(a => new AppointmentsDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    Patient = a.Patient,
                    DoctorId = a.DoctorId,
                    Doctor = a.Doctor,
                    AppointmentDate = a.AppointmentDate,
                    Reason = a.Reason,
                    Status = a.Status,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedDate = a.UpdatedDate,
                })
                .ToListAsync();

            if (!appointments.Any())
            {
                return ResponseDto<List<AppointmentsDto>>.Failure(
                    message: "No appointments found for this doctor",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "APPOINTMENT_NOT_FOUND",
                            ErrorMessage = $"No appointments found for doctor with Id {doctorId}"
                        }
                    }
                );
            }

            return ResponseDto<List<AppointmentsDto>>.SuccessResponse(appointments);
        }

        public async Task<ResponseDto<List<AppointmentsDto>>> GetAllAppointmentsByPatientIdAsync(string patientId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .Select(a => new AppointmentsDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    Patient = a.Patient,
                    DoctorId = a.DoctorId,
                    Doctor = a.Doctor,
                    AppointmentDate = a.AppointmentDate,
                    Reason = a.Reason,
                    Status = a.Status,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedDate = a.UpdatedDate,
                })
                .ToListAsync();

            if (!appointments.Any())
            {
                return ResponseDto<List<AppointmentsDto>>.Failure(
                    message: "No appointments found for this patient",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "APPOINTMENT_NOT_FOUND",
                            ErrorMessage = $"No appointments found for patient with Id {patientId}"
                        }
                    }
                );
            }

            return ResponseDto<List<AppointmentsDto>>.SuccessResponse(appointments);
        }

        public async Task<ResponseDto<AppointmentsDto>> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return ResponseDto<AppointmentsDto>.Failure(
                    message: "Appointment not found",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "APPOINTMENT_NOT_FOUND",
                            ErrorMessage = $"No appointment exists with Id {id}"
                        }
                    }
                );
            }

            var appointmentDto = new AppointmentsDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                Patient = appointment.Patient,
                DoctorId = appointment.DoctorId,
                Doctor = appointment.Doctor,
                AppointmentDate = appointment.AppointmentDate,
                Reason = appointment.Reason,
                Status = appointment.Status
            };

            return ResponseDto<AppointmentsDto>.SuccessResponse(appointmentDto);
        }

        public async Task<ResponseDto<bool>> UpdateAppointmentAsync(int id, CreateAppointmentsDto updateDto)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                _logger.LogInformation($"Appointment with this id {id} could not be found");
                return ResponseDto<bool>.Failure(
                    message: "Appointment not found",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "APPOINTMENT_NOT_FOUND",
                            ErrorMessage = $"No appointment exists with Id {id}"
                        }
                    }
                );
            }

            var patient = await _context.Users.FirstOrDefaultAsync(u => u.Id == updateDto.PatientId);
            if (patient == null)
            {
                _logger.LogInformation($"No patient found with Id {updateDto.PatientId}");
                return ResponseDto<bool>.Failure(
                    message: "Patient not found",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "PATIENT_NOT_FOUND",
                            ErrorMessage = $"No patient exists with Id {updateDto.PatientId}"
                        }
                    }
                );
            }

            var doctor = await _context.Users.FirstOrDefaultAsync(u => u.Id == updateDto.DoctorId);
            if (doctor == null)
            {
                _logger.LogInformation($"No doctor found with Id {updateDto.DoctorId}");
                return ResponseDto<bool>.Failure(
                    message: "Doctor not found",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "DOCTOR_NOT_FOUND",
                            ErrorMessage = $"No doctor exists with Id {updateDto.DoctorId}"
                        }
                    }
                );
            }

            var startTime = updateDto.AppointmentDate;
            var endTime = startTime.AddMinutes(30);

            var isOverlapping = await _context.Appointments.AnyAsync(a =>
                a.DoctorId == updateDto.DoctorId &&
                a.Id != id && 
                (a.AppointmentDate < endTime) &&
                (a.AppointmentDate.AddMinutes(30) > startTime) &&
                a.Status != "Cancelled"
            );

            if (isOverlapping)
            {
                return ResponseDto<bool>.Failure(
                    message: "Doctor already has an appointment in this time slot",
                    errors: new List<ApiError>
                    {
                        new ApiError
                        {
                            ErrorCode = "TIME_CONFLICT",
                            ErrorMessage = $"Doctor already has an appointment between {startTime} and {endTime}"
                        }
                    }
                );
            }

            appointment.PatientId = updateDto.PatientId;
            appointment.DoctorId = updateDto.DoctorId;
            appointment.AppointmentDate = updateDto.AppointmentDate;
            appointment.Reason = updateDto.Reason;
            appointment.Status = updateDto.Status;
            appointment.UpdatedBy = updateDto.PatientId;
            appointment.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Appointment with this id {id} is been updated successfully by {updateDto.PatientId}");
            return ResponseDto<bool>.SuccessResponse(true, "Appointment updated successfully");
        }
    }
}
