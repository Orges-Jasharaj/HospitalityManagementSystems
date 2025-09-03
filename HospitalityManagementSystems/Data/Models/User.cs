using Microsoft.AspNetCore.Identity;

namespace HospitalityManagementSystems.Data.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool isActive { get; set; } = true;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public int? DepartamentId { get; set; }
        public ICollection<Appointments> DoctorAppointments { get; set; }
        public ICollection<Appointments> PatientAppointments { get; set; }
        public ICollection<MedicalRecord> DoctorMedicalRecords { get; set; }
        public ICollection<MedicalRecord> PatientMedicalRecords { get; set; }
    }
}
