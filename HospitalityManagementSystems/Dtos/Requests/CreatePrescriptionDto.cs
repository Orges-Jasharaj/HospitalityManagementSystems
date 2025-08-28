using HospitalityManagementSystems.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalityManagementSystems.Dtos.Requests
{
    public class CreatePrescriptionDto
    {
        [Required]
        public int MedicalRecordId { get; set; }
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }
    }
}
