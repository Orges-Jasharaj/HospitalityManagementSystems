using HospitalityManagementSystems.Data.Models;

namespace HospitalityManagementSystems.Dtos.Responses
{
    public class DepartamentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Doctors { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
