using System.ComponentModel.DataAnnotations;

namespace HospitalityManagementSystems.Dtos.Requests
{
    public class CreateDepartamentDto
    {
        [Required]
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
