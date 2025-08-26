using HospitalityManagementSystems.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalityManagementSystems.Dtos.Responses
{
    public class AppointmentsDto
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public User Patient { get; set; }
        public string DoctorId { get; set; }
        public User Doctor { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
