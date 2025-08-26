using HospitalityManagementSystems.Data.Models;
using System.Security.Claims;
using static HospitalityManagementSystems.Dtos.TokenDtos;


namespace ToDoWebAPI.Service.Interface
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, List<string> roles);
        RefreshTokenDto GenerateRrefreshToken();

        ClaimsPrincipal GetClaimsPrincipal(string token);
    }
}
