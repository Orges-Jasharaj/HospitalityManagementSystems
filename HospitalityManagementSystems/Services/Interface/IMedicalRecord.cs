using Azure;
using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;

namespace HospitalityManagementSystems.Services.Interface
{
    public interface IMedicalRecord
    {
        Task<ResponseDto<bool>> CreateMedicalRecordAsync(CreateMedicalRecordDto createMedicalRecord);
        Task<ResponseDto<bool>> UpdateMedicalRecordAsync(int id, CreateMedicalRecordDto createMedicalRecord);
        Task<ResponseDto<bool>> DeleteMedicalRecordAsync(int id);
        Task<ResponseDto<List<MedicalRecordDto>>> GetAllMedicalRecordsAsync();
        Task<ResponseDto<MedicalRecordDto>> GetMedicalRecordsByIdAsync(int id);
    }
}
