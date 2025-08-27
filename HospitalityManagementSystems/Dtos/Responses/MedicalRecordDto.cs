using HospitalityManagementSystems.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalityManagementSystems.Dtos.Responses
{
    public class MedicalRecordDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public User Patient { get; set; }
        [Required]
        public string DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public User Doctor { get; set; }

        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
