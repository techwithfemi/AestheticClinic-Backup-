# WhatsApp Integration Examples

This document shows practical examples of how to integrate WhatsApp messaging into your existing services.

## Table of Contents
- [Appointment Service Integration](#appointment-service-integration)
- [Billing Service Integration](#billing-service-integration)
- [Following Up with Patients](#following-up-with-patients)
- [Error Handling](#error-handling)
- [Best Practices](#best-practices)

---

## Appointment Service Integration

### Example 1: Notify on Appointment Confirmation

```csharp
using AestheticEMR.Core.Services;
using AestheticEMR.Server.Services.WhatsApp;

public class AppointmentService : IAppointmentService
{
    private readonly IWhatsAppSender _whatsAppSender;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(IWhatsAppSender whatsAppSender, ILogger<AppointmentService> logger)
    {
        _whatsAppSender = whatsAppSender;
        _logger = logger;
    }

    public async Task<hAppointment> CreateAppointmentAsync(CreateAppointmentRequest request)
    {
        // Create appointment in database
        var appointment = new hAppointment
        {
            PatientID = request.PatientId,
            AppointmentDate = request.AppointmentDate,
            AppointmentTime = request.AppointmentTime,
            // ... other properties
        };

        // Save to database
        _dbContext.hAppointments.Add(appointment);
        await _dbContext.SaveChangesAsync();

        // Send WhatsApp confirmation
        if (!string.IsNullOrEmpty(request.PatientPhoneNumber))
        {
            var normalizedPhone = WhatsAppUtilities.NormalizePhoneNumber(request.PatientPhoneNumber);

            if (normalizedPhone != null)
            {
                var (success, messageId, error) = await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
                    normalizedPhone,
                    "appointment-confirmation",
                    request.PatientName,
                    request.AppointmentDate.ToString("yyyy-MM-dd"),
                    request.AppointmentTime,
                    request.ProviderName,
                    request.ClinicLocation
                );

                if (success)
                {
                    _logger.LogInformation("Appointment confirmation sent to {Phone} (MessageId: {MessageId})",
                        normalizedPhone, messageId);
                }
                else
                {
                    _logger.LogWarning("Failed to send appointment confirmation to {Phone}: {Error}",
                        normalizedPhone, error);
                }
            }
        }

        return appointment;
    }
}
```

### Example 2: Send Appointment Reminders

```csharp
public class AppointmentReminderService
{
    private readonly IWhatsAppSender _whatsAppSender;
    private readonly IAppointmentService _appointmentService;
    private readonly IHPatientService _patientService;
    private readonly ILogger<AppointmentReminderService> _logger;

    // This would be run by a scheduled job (e.g., Hangfire)
    public async Task SendRemindersForTomorrowAsync()
    {
        var tomorrow = DateTime.Now.AddDays(1).Date;

        // Get all appointments for tomorrow
        var appointmentsTomorrow = await _appointmentService.GetAppointmentsForDateAsync(tomorrow);

        foreach (var appointment in appointmentsTomorrow)
        {
            try
            {
                var patient = await _patientService.GetPatientByIdAsync(appointment.PatientID);

                if (patient?.Phn_No != null && 
                    WhatsAppUtilities.IsValidWhatsAppPhoneNumber(patient.Phn_No))
                {
                    var (success, messageId, error) = await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
                        patient.Phn_No,
                        "appointment-reminder",
                        patient.Fname, // Patient first name
                        appointment.AppointmentDate.ToString("yyyy-MM-dd"),
                        appointment.AppointmentTime,
                        appointment.Provider.Name
                    );

                    if (!success)
                    {
                        _logger.LogWarning("Reminder not sent to {Patient}: {Error}",
                            patient.Fname, error);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending reminder for appointment {AppointmentId}",
                    appointment.Id);
            }
        }
    }
}
```

### Example 3: Cancel Appointment and Notify

```csharp
public async Task CancelAppointmentAsync(int appointmentId, string cancellationReason = "")
{
    var appointment = await _dbContext.hAppointments.FindAsync(appointmentId);

    if (appointment == null)
        throw new ArgumentException("Appointment not found");

    var patient = await _dbContext.Vwhpatients.FirstOrDefaultAsync(p => p.PtID == appointment.PatientID);

    // Mark as cancelled
    appointment.Status = "Cancelled";
    appointment.CancellationReason = cancellationReason;

    await _dbContext.SaveChangesAsync();

    // Notify patient via WhatsApp
    if (patient?.Phn_No != null)
    {
        await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
            patient.Phn_No,
            "appointment-cancelled",
            patient.Fname,
            appointment.AppointmentDate.ToString("yyyy-MM-dd"),
            appointment.AppointmentTime,
            "Please call to reschedule: +1-555-0123"
        );
    }
}
```

---

## Billing Service Integration

### Example 1: Send Invoice Notification

```csharp
public class BillingService : IBillingService
{
    private readonly IWhatsAppSender _whatsAppSender;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<BillingService> _logger;

    public async Task<Billing> CreateAndSendInvoiceAsync(CreateInvoiceRequest request)
    {
        // Create invoice
        var invoice = new Billing
        {
            BillDate = DateTime.Now,
            BillNumber = GenerateInvoiceNumber(),
            PatientId = request.PatientId,
            Amount = request.TotalAmount,
            DueDate = DateTime.Now.AddDays(30)
        };

        _dbContext.Billings.Add(invoice);
        await _dbContext.SaveChangesAsync();

        // Get patient info
        var patient = await _dbContext.Vwhpatients.FirstOrDefaultAsync(p => p.PtID == request.PatientId);

        // Send invoice notification via WhatsApp
        if (patient?.Phn_No != null)
        {
            var normalizedPhone = WhatsAppUtilities.NormalizePhoneNumber(patient.Phn_No);

            if (normalizedPhone != null)
            {
                var (success, messageId, error) = await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
                    normalizedPhone,
                    "invoice-sent",
                    patient.Fname,
                    invoice.BillNumber,
                    invoice.Amount.ToString("C"),
                    invoice.DueDate.ToString("yyyy-MM-dd")
                );

                if (!success)
                {
                    _logger.LogWarning("Invoice notification failed for patient {PatientId}: {Error}",
                        request.PatientId, error);
                }
            }
        }

        return invoice;
    }
}
```

### Example 2: Payment Reminders for Overdue Invoices

```csharp
public class PaymentReminderService
{
    private readonly IWhatsAppSender _whatsAppSender;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<PaymentReminderService> _logger;

    // Run this as a scheduled job
    public async Task SendOverduePaymentRemindersAsync()
    {
        // Get invoices due in next 3 days or overdue
        var upcomingDueDate = DateTime.Now.AddDays(3);
        var overdueInvoices = await _dbContext.Billings
            .Where(b => b.DueDate <= upcomingDueDate && b.DueDate > DateTime.Now.AddDays(-30))
            .Include(b => b.Patient)
            .ToListAsync();

        foreach (var invoice in overdueInvoices)
        {
            try
            {
                var patient = invoice.Patient;

                if (patient?.Phn_No != null && 
                    WhatsAppUtilities.IsValidWhatsAppPhoneNumber(patient.Phn_No))
                {
                    // Check if already reminded (add tracking if needed)
                    if (invoice.ReminderSentDate == null)
                    {
                        var (success, _, error) = await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
                            patient.Phn_No,
                            "payment-reminder",
                            patient.Fname,
                            invoice.BillNumber,
                            invoice.RemainingAmount.ToString("C"),
                            invoice.DueDate.ToString("yyyy-MM-dd")
                        );

                        if (success)
                        {
                            invoice.ReminderSentDate = DateTime.Now;
                            await _dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            _logger.LogWarning("Payment reminder failed for invoice {InvoiceId}: {Error}",
                                invoice.Id, error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment reminder for invoice {InvoiceId}",
                    invoice.Id);
            }
        }
    }
}
```

### Example 3: Confirm Payment Received

```csharp
public async Task RecordPaymentAndNotifyAsync(Payment payment)
{
    // Record payment
    _dbContext.Payments.Add(payment);

    var invoice = await _dbContext.Billings.FindAsync(payment.BillingId);
    invoice.PaidAmount += payment.Amount;

    await _dbContext.SaveChangesAsync();

    // Notify patient
    var patient = invoice.Patient;

    if (patient?.Phn_No != null)
    {
        await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
            patient.Phn_No,
            "payment-received",
            patient.Fname,
            payment.Amount.ToString("C"),
            invoice.BillNumber,
            payment.ReferenceNumber,
            invoice.RemainingAmount.ToString("C")
        );
    }
}
```

---

## Following Up with Patients

### Example 1: Post-Procedure Care Instructions

```csharp
public class AestheticConsultationService
{
    private readonly IWhatsAppSender _whatsAppSender;

    public async Task SendPostProcedureCareAsync(
        AestheticConsultation consultation,
        string careInstructions)
    {
        var patient = await _patientService.GetPatientAsync(consultation.PatientId);

        if (patient?.Phn_No != null)
        {
            await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
                patient.Phn_No,
                "post-procedure-care",
                patient.FirstName,
                consultation.ProcedureName,  // e.g., "Botox", "Fillers"
                careInstructions,
                EmergencyContactNumber
            );
        }
    }
}
```

### Example 2: Follow-up Appointment Scheduling

```csharp
public async Task ScheduleFollowUpAndNotifyAsync(
    AestheticConsultation previousConsultation,
    DateTime followUpDate,
    TimeSpan followUpTime)
{
    // Create new appointment
    var followUpAppointment = new hAppointment
    {
        PatientID = previousConsultation.PatientId,
        AppointmentDate = followUpDate,
        AppointmentTime = followUpTime.ToString(@"hh\:mm"),
        Type = "Follow-up"
    };

    _dbContext.hAppointments.Add(followUpAppointment);
    await _dbContext.SaveChangesAsync();

    // Notify patient
    var patient = previousConsultation.Patient;

    if (patient?.Phn_No != null)
    {
        await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
            patient.Phn_No,
            "followup-consultation",
            patient.FirstName,
            previousConsultation.ProcedureName,
            followUpDate.ToString("yyyy-MM-dd"),
            followUpTime.ToString(@"hh\:mm")
        );
    }
}
```

### Example 3: Patient Satisfaction Survey

```csharp
public async Task SendSatisfactionSurveyAsync(
    AestheticConsultation consultation,
    string surveyUrl)
{
    var patient = consultation.Patient;

    if (patient?.Phn_No != null)
    {
        await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
            patient.Phn_No,
            "followup-survey",
            patient.FirstName,
            consultation.ProcedureName,
            surveyUrl
        );
    }
}
```

---

## Error Handling

### Example: Robust Message Sending with Retry

```csharp
public class RobustWhatsAppSender
{
    private readonly IWhatsAppSender _whatsAppSender;
    private readonly ILogger<RobustWhatsAppSender> _logger;

    public async Task<bool> SendWithRetryAsync(
        string phoneNumber,
        string messageBody,
        int maxRetries = 3)
    {
        var attempt = 0;

        while (attempt < maxRetries)
        {
            try
            {
                var (success, messageId, error) = await _whatsAppSender.SendWhatsAppMessageAsync(
                    phoneNumber,
                    messageBody
                );

                if (success)
                {
                    _logger.LogInformation("Message sent successfully to {Phone} (MessageId: {MessageId})",
                        phoneNumber, messageId);
                    return true;
                }

                attempt++;

                if (attempt < maxRetries)
                {
                    // Wait before retrying (exponential backoff)
                    var delayMs = (int)Math.Pow(2, attempt) * 1000;
                    _logger.LogWarning("Message failed, retrying in {DelaySeconds}s. Error: {Error}",
                        delayMs / 1000, error);
                    await Task.Delay(delayMs);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception sending message to {Phone}",
                    phoneNumber);
                attempt++;
            }
        }

        _logger.LogError("Failed to send message to {Phone} after {MaxRetries} attempts",
            phoneNumber, maxRetries);
        return false;
    }
}
```

---

## Best Practices

### 1. Always Validate Phone Numbers
```csharp
var normalizedPhone = WhatsAppUtilities.NormalizePhoneNumber(userPhone);
if (normalizedPhone == null || !WhatsAppUtilities.IsValidWhatsAppPhoneNumber(normalizedPhone))
{
    // Show error or skip
    return;
}
```

### 2. Check if WhatsApp is Enabled
```csharp
public class SafeWhatsAppService
{
    private readonly IWhatsAppSender _whatsAppSender;
    private readonly IOptions<AppSettings> _options;

    public async Task SendIfEnabledAsync(string phone, string message)
    {
        if (!_options.Value.WhatsAppConfig?.Enabled ?? false)
        {
            _logger.LogWarning("WhatsApp is disabled, message not sent");
            return;
        }

        await _whatsAppSender.SendWhatsAppMessageAsync(phone, message);
    }
}
```

### 3. Don't Block on WhatsApp Sending
```csharp
// BAD: Blocks appointment creation if message fails
await _whatsAppSender.SendWhatsAppMessageAsync(phone, message);
appointment.Save();

// GOOD: Save first, send async in background
appointment.Save();

// Fire and forget (with logging)
_ = _whatsAppSender.SendWhatsAppMessageAsync(phone, message)
    .ContinueWith(task =>
    {
        if (task.IsFaulted)
            _logger.LogError("Message delivery failed");
    });
```

### 4. Log Message Activities
```csharp
var (success, messageId, error) = await _whatsAppSender.SendWhatsAppMessageAsync(
    patientPhone,
    "Your appointment is tomorrow"
);

if (success)
{
    _logger.LogInformation(
        "WhatsApp sent: Phone={Phone}, MessageId={MessageId}",
        patientPhone, messageId);
}
else
{
    _logger.LogError(
        "WhatsApp failed: Phone={Phone}, Error={Error}",
        patientPhone, error);
}
```

### 5. Provide Fallback to Email
```csharp
public async Task NotifyPatientAsync(string name, string phone, string email, string message)
{
    // Try WhatsApp first
    var phoneNormalized = WhatsAppUtilities.NormalizePhoneNumber(phone);
    if (phoneNormalized != null)
    {
        var (success, _, _) = await _whatsAppSender.SendWhatsAppMessageAsync(
            phoneNormalized,
            message);

        if (success)
            return;
    }

    // Fall back to email
    if (!string.IsNullOrEmpty(email))
    {
        await _emailSender.SendEmailAsync(name, email, "Notification", message);
    }
}
```

---

**Ready to use these examples? Adapt them to your services!**
