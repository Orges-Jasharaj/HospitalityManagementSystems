using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HospitalityManagementSystems.Data.Models;

namespace HospitalityManagementSystems.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Appointments> Appointments { get; set; }

        public DbSet<Prescription> Prescription { get; set; }

        public DbSet<MedicalRecord> MedicalRecord { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Department> Departaments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasQueryFilter(u => u.isActive);

            builder.Entity<Appointments>()
                .HasOne(a => a.Doctor)
                .WithMany(u => u.DoctorAppointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointments>()
                .HasOne(a => a.Patient)
                .WithMany(u => u.PatientAppointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MedicalRecord>()
                .HasOne(m => m.Doctor)
                .WithMany(u => u.DoctorMedicalRecords)
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MedicalRecord>()
                .HasOne(m => m.Patient)
                .WithMany(u => u.PatientMedicalRecords)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Prescription>()
                .HasOne(p => p.MedicalRecord)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.MedicalRecord)
                .WithMany(m => m.Payments)
                .HasForeignKey(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
