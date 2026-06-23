namespace AestheticEMR.Core.Services
{
    public interface ISmsTemplateService
    {
        string BuildAppointmentMessage(string patientName, DateTime? appointmentDate, DateTime? appointmentTime, string? clinicType, string action);
        string BuildAttendanceMessage(string patientName, DateTime attendanceDate, string? clinicType, string? consultId, string action);
        string BuildBirthdayMessage(string patientName, DateTime dateOfBirth);
    }
}
