using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;

namespace HospitalityManagementSystems.Services.Interface
{
    public interface IUser
    {
        Task<ResponseDto<bool>> CreateUserAsync(CreateUserDto createUserDto);
        //Task<ResponseDto<bool>> CreateDoctorAsync(CreateUserDto createUserDto);
        //Task<ResponseDto<bool>> CreateAdminAsync(CreateUserDto createUserDto);
        //Task<ResponseDto<bool>> CreateAdminstratorAsync(CreateUserDto createUserDto);
        //Task<ResponseDto<bool>> CreateNurseAsync(CreateUserDto createUserDto);
        Task<ResponseDto<bool>> CreateUserWithRoleAsync(CreateUserDto createUserDto, string role);
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ResponseDto<UserDto>> GetUserByIdAsync(string userId);
        Task<ResponseDto<List<UserDto>>> GetAllUsersAsync();
        Task<ResponseDto<bool>> DeleteUserAsync(string userId);
        Task<ResponseDto<bool>> UpdateUserAsync(string userId, UpdateUserDto userDto);

        Task<ResponseDto<bool>> ChangeUserPassword(ChangePasswordDto changePasswordDto);

        Task<ResponseDto<LoginResponseDto>> RefreshToken(RefreshTokenRequestDto refreshTokenDto);

    }
}
