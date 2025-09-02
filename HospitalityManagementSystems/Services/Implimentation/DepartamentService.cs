using HospitalityManagementSystems.Data;
using HospitalityManagementSystems.Data.Models;
using HospitalityManagementSystems.Dtos.Requests;
using HospitalityManagementSystems.Dtos.Responses;
using HospitalityManagementSystems.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalityManagementSystems.Services.Implimentation
{
    public class DepartamentService : IDepartament
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DepartamentService> _logger;

        public DepartamentService(AppDbContext context, ILogger<DepartamentService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<ResponseDto<bool>> CreateDepartament(CreateDepartamentDto createDepartamentDto)
        {
            var existingDepartament = await _context.Departaments
                .FirstOrDefaultAsync(d => d.Name.ToLower() == createDepartamentDto.Name.ToLower());

            if (existingDepartament != null)
            {
                return ResponseDto<bool>.Failure("Departament with the same name already exists.");
            }

            var newDepartament = new Department
            {
                Name = createDepartamentDto.Name,
                CreatedBy = createDepartamentDto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
            };
            _context.Departaments.Add(newDepartament);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Departament created successfully with this name", createDepartamentDto.Name);
            return ResponseDto<bool>.SuccessResponse(true, "Departament created successfully.");
        }

        public async Task<ResponseDto<bool>> DeleteDepartament(int id)
        {
            var existingDepartament = await _context.Departaments.FirstOrDefaultAsync(x=> x.Id == id);
            if (existingDepartament == null)
            {
                return ResponseDto<bool>.Failure("Departament not found.");
            }
            _context.Departaments.Remove(existingDepartament);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Departament deleted successfully with this id", id);
            return ResponseDto<bool>.SuccessResponse(true, "Departament deleted successfully.");
        }

        public async Task<ResponseDto<List<DepartamentDto>>> GetAllDepartaments()
        {
            var departaments = await _context.Departaments
                .Select(d => new DepartamentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    CreatedBy = d.CreatedBy,
                    CreatedAt = d.CreatedAt,
                    UpdatedBy = d.UpdatedBy,
                    UpdatedAt = d.UpdatedAt
                })
                .ToListAsync();
            return ResponseDto<List<DepartamentDto>>.SuccessResponse(departaments, "Departaments retrieved successfully.");
        }

        public async Task<ResponseDto<DepartamentDto>> GetDepartamentById(int id)
        {
            var departament = await _context.Departaments
                .Where(d => d.Id == id)
                .Select(d => new DepartamentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    CreatedBy = d.CreatedBy,
                    CreatedAt = d.CreatedAt,
                    UpdatedBy = d.UpdatedBy,
                    UpdatedAt = d.UpdatedAt
                })
                .FirstOrDefaultAsync();
            if (departament == null)
            {
                return ResponseDto<DepartamentDto>.Failure("Departament not found.");
            }
            return ResponseDto<DepartamentDto>.SuccessResponse(departament, "Departament retrieved successfully.");
        }

        public async Task<ResponseDto<bool>> UpdateDepartament(int id, CreateDepartamentDto createDepartamentDto)
        {
            var existingDepartament = await _context.Departaments.FirstOrDefaultAsync(x => x.Id == id);
            if (existingDepartament == null)
            {
                return ResponseDto<bool>.Failure("Departament not found.");
            }
            existingDepartament.Name = createDepartamentDto.Name;
            existingDepartament.UpdatedBy = createDepartamentDto.UpdatedBy;
            existingDepartament.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Departament updated successfully with this id", id);
            return ResponseDto<bool>.SuccessResponse(true, "Departament updated successfully.");

        }
    }
}
