using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;

namespace HospitalityManagementSystems.Services.Interface
{
    public interface IPrescription
    {
        Task<ResponseDto<bool>> CreatePrescription(CreatePrescriptionDto createPrescriptionDto);
        Task<ResponseDto<bool>> UpdatePrescriptionAsync(int id, CreatePrescriptionDto createPrescriptionDto);
        Task<ResponseDto<bool>> DeletePrescriptionAsync(int id);

        Task<ResponseDto<List<PrescriptionDto>>> GetAllPrescriptionsAsync();
        Task<ResponseDto<PrescriptionDto>> GetPrescriptionByIdAsync(int id);


    }
}
