using System.ComponentModel.DataAnnotations;

namespace HospitalityManagementSystems.Data.Models
{
    
        public class Department
        {
            [Key]
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            public ICollection<User> Doctors { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedAt { get; set; }
            public string? UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }

    }

    
}
