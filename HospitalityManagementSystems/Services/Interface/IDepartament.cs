using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;

namespace HospitalityManagementSystems.Services.Interface
{
    public interface IDepartament
    {
        Task<ResponseDto<bool>> CreateDepartament(CreateDepartamentDto createDepartamentDto);
        Task<ResponseDto<bool>> UpdateDepartament(int id, CreateDepartamentDto createDepartamentDto);
        Task<ResponseDto<bool>> DeleteDepartament(int id);
        Task<ResponseDto<List<DepartamentDto>>> GetAllDepartaments();
        Task<ResponseDto<DepartamentDto>> GetDepartamentById(int id);
    }
}
