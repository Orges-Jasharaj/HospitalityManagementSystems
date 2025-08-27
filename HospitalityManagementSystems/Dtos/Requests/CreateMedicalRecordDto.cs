using System.ComponentModel.DataAnnotations;

namespace HospitalityManagementSystems.Dtos.Requests
{
    public class CreateMedicalRecordDto
    {
        [Required]
        public string PatientId { get; set; }
        [Required]
        public string DoctorId { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
    }
}
