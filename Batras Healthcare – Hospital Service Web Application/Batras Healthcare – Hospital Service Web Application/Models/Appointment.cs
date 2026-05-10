namespace Batras_Healthcare___Hospital_Service_Web_Application.Models

{
    public class Appointment
    {
        public int Id { get; set; }

        public string PatientName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string DoctorName { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }

        public string? Symptoms { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}  


