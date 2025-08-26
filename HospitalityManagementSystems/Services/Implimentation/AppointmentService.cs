using HospitalityManagementSystems.Data;
using HospitalityManagementSystems.Data.Models;
using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;
using HospitalityManagementSystems.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalityManagementSystems.Services.Implimentation
{
    public class AppointmentService : IAppointment
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
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
                return ResponseDto<bool>.Failure(
                    message: "Doctor not found",
                    errors: new List<ApiError>
                    {
                new ApiError { ErrorCode = "DOCTOR_NOT_FOUND", ErrorMessage = $"No doctor exists with Id {createAppointmentsDto.DoctorId}" }
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

            return ResponseDto<bool>.SuccessResponse(true, "Appointment created successfully");
        }



        public async Task<ResponseDto<bool>> DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(u => u.Id==id);
            if (appointment == null)
            {
                return ResponseDto<bool>.Failure(
                    message: "Appointment not found",
                    errors: new List<ApiError>
                    {
                new ApiError
                {
                    ErrorCode = "PATIENT_NOT_FOUND",
                    ErrorMessage = $"No appointment exists with Id {id}"
                }
                    }
                );
            }
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync( );

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
                    CreatedBy = a.PatientId
                }).ToListAsync();

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

            appointment.PatientId = updateDto.PatientId;
            appointment.DoctorId = updateDto.DoctorId;
            appointment.AppointmentDate = updateDto.AppointmentDate;
            appointment.Reason = updateDto.Reason;
            appointment.Status = updateDto.Status;
            appointment.UpdatedBy = updateDto.PatientId;

            await _context.SaveChangesAsync();

            return ResponseDto<bool>.SuccessResponse(true, "Appointment updated successfully");
        }


    }
}
