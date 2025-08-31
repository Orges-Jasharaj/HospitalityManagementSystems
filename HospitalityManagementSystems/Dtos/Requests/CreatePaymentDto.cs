using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalityManagementSystems.Dtos.Requests
{
    public class CreatePaymentDto
    {
        [Required]
        public int MedicalRecordId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? CompletedDate { get; set; }
        public string? Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } 

    }
}
