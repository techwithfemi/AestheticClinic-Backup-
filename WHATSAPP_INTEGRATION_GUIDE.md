# WhatsApp Messaging Integration Guide

## Overview

This application now includes WhatsApp messaging capabilities using the Twilio WhatsApp API. This allows you to send notifications, appointment reminders, and other messages directly to patient phone numbers via WhatsApp.

## Features

- ✅ Send direct WhatsApp messages
- ✅ Use pre-built message templates
- ✅ Send messages with media (images, documents, audio, video)
- ✅ Phone number validation and normalization
- ✅ Built-in error handling and retry logic
- ✅ Comprehensive logging for debugging
- ✅ Integration with existing email infrastructure

## Setup Instructions

### Step 1: Get Twilio WhatsApp Credentials

1. **Create a Twilio Account** (if you don't have one):
   - Go to https://www.twilio.com/console
   - Sign up for a free account

2. **Get Your WhatsApp Sandbox Number**:
   - Navigate to: Messaging → WhatsApp → Sandbox
   - You'll see your sandbox phone number (starts with +1 and looks like a regular phone number)
   - Copy this number - you'll need it

3. **Get Your Account Credentials**:
   - In the Twilio Console Dashboard, find:
     - **Account SID** (looks like: ACxxxxxxxxxxxxx)
     - **Auth Token** (looks like: your secret token)
   - Keep these credentials safe!

### Step 2: Configure appsettings.json

Add the WhatsApp configuration to your `appsettings.json` file:

```json
{
  "WhatsAppConfig": {
    "AccountSid": "ACxxxxxxxxxxxxx",
    "AuthToken": "your_auth_token_here",
    "FromPhoneNumber": "+1234567890",
    "Enabled": true,
    "MaxRetries": 3
  }
}
```

**For Sandbox Testing (Free):**
- Use the sandbox phone number provided by Twilio
- Test by sending messages to any number you've registered in the sandbox

**For Production:**
- Request production access in Twilio
- Use your approved WhatsApp business phone number
- Ensure you comply with WhatsApp's Business Policy

### Step 3: Example Usage in Code

#### Basic Message Sending

```csharp
public class AppointmentService
{
    private readonly IWhatsAppSender _whatsAppSender;

    public AppointmentService(IWhatsAppSender whatsAppSender)
    {
        _whatsAppSender = whatsAppSender;
    }

    public async Task NotifyPatientAsync(string phoneNumber, string appointmentDate)
    {
        var (success, messageId, error) = await _whatsAppSender.SendWhatsAppMessageAsync(
            phoneNumber,
            $"Your appointment is on {appointmentDate}. Please arrive 10 minutes early."
        );

        if (success)
        {
            _logger.LogInformation($"Message sent: {messageId}");
        }
        else
        {
            _logger.LogError($"Failed to send message: {error}");
        }
    }
}
```

#### Using Templates

```csharp
// Send appointment confirmation
var (success, messageId, error) = await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
    patientPhoneNumber,
    "appointment-confirmation",
    patientName,
    appointmentDate,
    appointmentTime,
    providerName,
    clinicLocation
);
```

#### Sending Messages with Media

```csharp
// Send invoice with document
var (success, messageId, error) = await _whatsAppSender.SendWhatsAppMessageWithMediaAsync(
    patientPhoneNumber,
    "Your invoice is ready. Please download attached.",
    "https://example.com/invoices/invoice123.pdf",
    "document"
);
```

#### Phone Number Validation

```csharp
using AestheticEMR.Server.Services.WhatsApp;

// Validate phone number
if (!WhatsAppUtilities.IsValidWhatsAppPhoneNumber(phoneNumber))
{
    // Handle invalid number
}

// Normalize phone number to E.164 format
var normalized = WhatsAppUtilities.NormalizePhoneNumber("1234567890");
// Result: "+11234567890" (assuming US number)
```

## Available Templates

The following message templates are built-in:

### Appointment Templates
- `appointment-confirmation` - Confirm appointment booking
- `appointment-reminder` - Remind about upcoming appointment
- `appointment-cancelled` - Notify of cancellation
- `appointment-rescheduled` - Confirm new appointment date

### Billing & Payment
- `invoice-sent` - Notify that invoice is ready
- `payment-reminder` - Remind about payment due
- `payment-received` - Confirm payment received

### Follow-up & Care
- `followup-consultation` - Schedule follow-up appointment
- `post-procedure-care` - Send post-procedure instructions
- `followup-survey` - Request patient feedback

### General Notifications
- `account-verification` - Send verification code
- `welcome-new-patient` - Welcome new patient
- `clinic-update` - Send clinic announcements

### Service-Specific
- `aesthetic-consultation-offer` - Special offers
- `dental-appointment-reminder` - Dental-specific reminder
- `lab-results-ready` - Notify results availability

## Testing the Integration

### Using the Debug Controller

The application includes a debug controller for testing WhatsApp functionality. **Important: This should be removed or secured in production!**

**Endpoints Available:**

1. **Send Text Message**
   ```
   POST /api/whatsappdebug/send-message
   ?phoneNumber=+1234567890
   &message=Hello, this is a test message!
   ```

2. **Send Template Message**
   ```
   POST /api/whatsappdebug/send-template
   ?phoneNumber=+1234567890
   &templateName=appointment-confirmation
   &variables=John,2024-01-15,2:00 PM,Dr. Smith,Main Clinic
   ```

3. **List Available Templates**
   ```
   GET /api/whatsappdebug/templates
   ```

4. **Validate Phone Number**
   ```
   GET /api/whatsappdebug/validate-phone?phoneNumber=+1234567890
   ```

5. **Generate WhatsApp Link**
   ```
   GET /api/whatsappdebug/whatsapp-link
   ?phoneNumber=+1234567890
   &message=Hello from clinic
   ```

6. **Send Message with Media**
   ```
   POST /api/whatsappdebug/send-with-media
   ?phoneNumber=+1234567890
   &messageBody=Here is your receipt
   &mediaUrl=https://example.com/receipt.pdf
   &mediaType=document
   ```

### Using cURL for Testing

```bash
# Test basic message
curl -X POST "https://localhost:5001/api/whatsappdebug/send-message?phoneNumber=%2B1234567890&message=Test%20Message" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Test template
curl -X POST "https://localhost:5001/api/whatsappdebug/send-template?phoneNumber=%2B1234567890&templateName=appointment-confirmation&variables=John,2024-01-15" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## Integration with Existing Features

### Integrating with Appointment Service

```csharp
public class AppointmentService
{
    private readonly IWhatsAppSender _whatsAppSender;
    private readonly IEmailSender _emailSender;

    public async Task CreateAppointmentAsync(CreateAppointmentRequest request)
    {
        // ... create appointment logic ...

        // Send both email and WhatsApp notification
        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
                request.PhoneNumber,
                "appointment-confirmation",
                request.PatientName,
                request.AppointmentDate.ToString("yyyy-MM-dd"),
                request.AppointmentTime,
                request.ProviderName,
                request.Location
            );
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            await _emailSender.SendEmailAsync(
                request.PatientName,
                request.Email,
                "Appointment Confirmation",
                "Your appointment has been confirmed..."
            );
        }
    }
}
```

### Integrating with Billing Service

```csharp
public class BillingService
{
    private readonly IWhatsAppSender _whatsAppSender;

    public async Task SendInvoiceNotificationAsync(Invoice invoice, string patientPhone)
    {
        await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
            patientPhone,
            "invoice-sent",
            invoice.PatientName,
            invoice.InvoiceNumber,
            invoice.TotalAmount.ToString("C"),
            invoice.DueDate.ToString("yyyy-MM-dd")
        );
    }

    public async Task SendPaymentReminderAsync(Invoice invoice, string patientPhone)
    {
        var daysDue = (invoice.DueDate - DateTime.Now).Days;
        if (daysDue <= 3 && daysDue >= -30)  // Within 3 days or overdue by 30 days
        {
            await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
                patientPhone,
                "payment-reminder",
                invoice.PatientName,
                invoice.InvoiceNumber,
                invoice.RemainingBalance.ToString("C"),
                invoice.DueDate.ToString("yyyy-MM-dd")
            );
        }
    }
}
```

## Phone Number Format Requirements

WhatsApp requires phone numbers in **E.164 format**:
- Must start with `+`
- Followed by country code (1-3 digits)
- Followed by local number
- Total: 10-15 digits including country code

**Examples:**
- USA: `+12025551234`
- UK: `+441632960123`
- Nigeria: `+2348012345678`
- Australia: `+61412345678`

**Utility Methods:**
```csharp
// Normalize any format to E.164
string normalized = WhatsAppUtilities.NormalizePhoneNumber("2025551234");
// Result: "+12025551234" (assumes US)

// Validate format
bool isValid = WhatsAppUtilities.IsValidWhatsAppPhoneNumber("+12025551234");
// Result: true

// Extract country code
string countryCode = WhatsAppUtilities.ExtractCountryCode("+12025551234");
// Result: "1"
```

## Custom Templates

You can add custom templates at runtime:

```csharp
WhatsAppTemplates.AddCustomTemplate(
    "custom-message",
    "Hello {0},\n\nYour custom message content here: {1}\n\nBest regards"
);

// Use it
await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
    phoneNumber,
    "custom-message",
    "John",
    "Custom content"
);
```

## Error Handling

The WhatsApp sender returns detailed error information:

```csharp
var (success, messageId, errorMsg) = await _whatsAppSender.SendWhatsAppMessageAsync(
    phoneNumber,
    "Your message"
);

if (!success)
{
    // Handle specific errors
    if (errorMsg.Contains("Invalid phone number"))
    {
        // Show validation error to user
    }
    else if (errorMsg.Contains("disabled"))
    {
        // WhatsApp feature is disabled in config
    }
    else
    {
        // Other errors
        _logger.LogError(errorMsg);
    }
}
```

## Common Issues & Solutions

### Issue: "Invalid phone number format"
**Solution:** Ensure phone number starts with `+` and contains only digits. Use `NormalizePhoneNumber()` for normalization.

### Issue: "WhatsAppConfig is not configured"
**Solution:** Add WhatsApp configuration to `appsettings.json`

### Issue: "Auth Token Invalid"
**Solution:** 
1. Verify AccountSid and AuthToken are copied correctly from Twilio Console
2. Check that there are no extra spaces or special characters
3. Ensure the token hasn't expired

### Issue: "Message not delivering"
**Solution:**
1. Verify recipient has registered with WhatsApp (required for Twilio sandbox)
2. Check phone number format - use `+` with country code
3. Verify Twilio account isn't rate limited
4. Check Twilio logs for delivery errors

### Issue: "Sandbox limit reached"
**Solution:** 
1. For testing, you can only send to numbers you've registered
2. In production, request production access from Twilio
3. Implement queue/batch messaging for high volume

## Production Considerations

### Security
1. **Never log auth tokens** - they're sensitive credentials
2. **Use environment variables** for secrets
3. **Restrict debug endpoints** to administrators only
4. **Validate all phone numbers** before sending

### Performance
1. **Implement message queuing** for bulk sends
2. **Use async/await** to avoid blocking threads
3. **Implement retry logic** for failed messages
4. **Monitor API usage** and Twilio quotas

### Compliance
1. **Get explicit consent** before sending marketing messages
2. **Respect Do Not Disturb hours** in patient's timezone
3. **Include opt-out information** in messages
4. **Maintain audit logs** of all messages sent
5. **Follow WhatsApp's Business Policy**

## Removing the Debug Controller

For production deployment, remove or secure the WhatsAppDebugController:

```csharp
// In Program.cs, comment out or remove:
// app.MapControllers(); // if specific routing
// Or in the controller, change authorization:
[Authorize(Roles = "SuperAdmin")] // More restrictive
```

Alternatively, delete the file entirely:
```
AestheticEMR\AestheticEMR.Server\Controllers\WhatsAppDebugController.cs
```

## Support & Documentation

- **Twilio Documentation**: https://www.twilio.com/docs/whatsapp
- **Twilio Console**: https://www.twilio.com/console
- **WhatsApp Business API**: https://developers.facebook.com/docs/whatsapp/business-platform/get-started
- **Phone Number Formats**: https://en.wikipedia.org/wiki/E.164

## FAQ

**Q: Can I send messages without WhatsApp installed on my phone?**
A: Yes! This uses the Twilio WhatsApp API, not your personal WhatsApp account.

**Q: What's the cost?**
A: Twilio WhatsApp messages start at approximately $0.001-$0.005 per message depending on destination country.

**Q: Can I use my own WhatsApp Business number?**
A: Yes, but requires production approval and WhatsApp Business API integration.

**Q: How many messages can I send per day?**
A: Depends on your Twilio account tier and WhatsApp approval level. Start with sandbox (limited testing).

**Q: Can I schedule messages?**
A: The basic integration doesn't include scheduling. Consider using a job scheduler like Hangfire for delayed sends.

---

**Created:** 2024
**Version:** 1.0
**Status:** Active
