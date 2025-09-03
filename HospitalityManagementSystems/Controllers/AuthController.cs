using HospitalityManagementSystems.Data.Models;
using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;
using HospitalityManagementSystems.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalityManagementSystems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUser _userService;

        public AuthController(IUser userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
        {
            var result = await _userService.CreateUserAsync(createUserDto);
            return Ok(result);
        }

        //[HttpPost("registerDoctor")]
        //[Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin}")]
        //public async Task<IActionResult> RegisterDoctor([FromBody] CreateUserDto createUserDto)
        //{
        //    var result = await _userService.CreateDoctorAsync(createUserDto);
        //    return Ok(result);
        //}

        //[HttpPost("registerAdmin")]
        //[Authorize(Roles = $"{RoleTypes.SuperAdmin}")]
        //public async Task<IActionResult> RegisterAdmin([FromBody] CreateUserDto createUserDto)
        //{
        //    var result = await _userService.CreateAdminAsync(createUserDto);
        //    return Ok(result);
        //}

        //[HttpPost("registerNurse")]
        //[Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin}")]
        //public async Task<IActionResult> RegisterNurse([FromBody] CreateUserDto createUserDto)
        //{
        //    var result = await _userService.CreateNurseAsync(createUserDto);
        //    return Ok(result);
        //}

        //[HttpPost("registerAdminstrator")]
        //[Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.SuperAdmin}")]
        //public async Task<IActionResult> RegisterAdminstrator([FromBody] CreateUserDto createUserDto)
        //{
        //    var result = await _userService.CreateAdminstratorAsync(createUserDto);
        //    return Ok(result);
        //}

        [HttpPost("registerUserWithRole")]
        [Authorize(Roles = $"{RoleTypes.SuperAdmin}")]
        public async Task<IActionResult> RegisterUserWithRole([FromBody] CreateUserDto createUserWithRoleDto, string role)
        {
            var result = await _userService.CreateUserWithRoleAsync(createUserWithRoleDto, role);
            return Ok(result);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            return Ok(result);
        }

        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenDto)
        {
            var result = await _userService.RefreshToken(refreshTokenDto);
            return Ok(result);
        }
    }
}
