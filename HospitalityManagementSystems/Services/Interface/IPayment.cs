using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;

namespace HospitalityManagementSystems.Services.Interface
{
    public interface IPayment
    {
        Task<ResponseDto<bool>> CreatePaymentAsync(CreatePaymentDto createPaymentDto);
        Task<ResponseDto<bool>> UpdatePaymentAsync(int id, CreatePaymentDto updatepaymentDto);
        Task <ResponseDto<bool>> DeletePaymentAsync(int id);
        Task<ResponseDto<PaymentDto>> GetPaymentByIdAsync(int id);
        Task<ResponseDto<List<PaymentDto>>> GetAllPayments();
        Task<ResponseDto<List<PaymentDto>>> GetAllPaymentsByPatientId(string id);
    }
}
