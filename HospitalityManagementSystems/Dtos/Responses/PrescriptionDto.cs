using HospitalityManagementSystems.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalityManagementSystems.Dtos.Responses
{
    public class PrescriptionDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
