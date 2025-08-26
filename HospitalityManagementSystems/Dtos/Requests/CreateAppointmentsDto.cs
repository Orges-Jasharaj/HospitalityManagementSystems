namespace HospitalityManagementSystems.Dtos.Requests
{
    public class CreateAppointmentsDto
    {
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; } = DateTime.Now;
        public string Reason { get; set; }
        public string Status { get; set; }

    }
}
