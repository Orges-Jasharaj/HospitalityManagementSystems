using Microsoft.AspNetCore.Identity;

namespace HospitalityManagementSystems.Data.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public ICollection<Appointments> DoctorAppointments { get; set; }
        public ICollection<Appointments> PatientAppointments { get; set; }
    }
}
