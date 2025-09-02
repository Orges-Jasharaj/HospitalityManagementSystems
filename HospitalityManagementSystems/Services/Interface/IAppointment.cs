using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;

namespace HospitalityManagementSystems.Services.Interface
{
    public interface IAppointment
    {
        Task<ResponseDto<bool>> CreateAppointmentAsync(CreateAppointmentsDto createAppointmentsDto);
        Task<ResponseDto<bool>> UpdateAppointmentAsync(int id,CreateAppointmentsDto createAppointmentsDto);
        Task <ResponseDto<bool>> DeleteAppointmentAsync(int id);
        Task<ResponseDto<List<AppointmentsDto>>> GetAllAppointmentsAsync();
        Task<ResponseDto<AppointmentsDto>> GetAppointmentByIdAsync(int id);
        Task<ResponseDto<List<AppointmentsDto>>> GetAllAppointmentsByPatientIdAsync(string doctorid);
        Task<ResponseDto<List<AppointmentsDto>>> GetAllAppointmentsByDoctorIdAsync(string patientid);
    }
}
