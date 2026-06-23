# WhatsApp Integration - Quick Start Guide

## What's New

Your AestheticClinic application now includes **WhatsApp messaging capabilities**! Send notifications, appointment reminders, and other messages directly to patients via WhatsApp.

## 5-Minute Setup

### 1. Get Twilio Credentials (5 min)
- Go to https://www.twilio.com/console (create account if needed)
- Copy your **Account SID** and **Auth Token** from the dashboard
- Note your **WhatsApp Sandbox Number** (Messaging → WhatsApp → Sandbox)

### 2. Configure appsettings.json
```json
{
  "WhatsAppConfig": {
    "AccountSid": "ACxxxxxxxxxxxxx",
    "AuthToken": "your_token_here",
    "FromPhoneNumber": "+1234567890",
    "Enabled": true,
    "MaxRetries": 3
  }
}
```

### 3. That's it! You're ready.

## Testing the Integration

### Option A: Using the Debug Controller

The app includes a built-in debug controller for testing. Make a request:

```
POST /api/whatsappdebug/send-message
?phoneNumber=%2B14155552671
&message=Hello%20from%20clinic
```

**Requirements:**
- Admin user logged in
- Phone number in E.164 format (+1234567890)

### Option B: Use in Code

```csharp
public class YourService
{
    private readonly IWhatsAppSender _whatsApp;

    public YourService(IWhatsAppSender whatsApp)
    {
        _whatsApp = whatsApp;
    }

    public async Task NotifyPatientAsync()
    {
        var (success, messageId, error) = await _whatsApp.SendWhatsAppMessageAsync(
            "+1234567890",
            "Hello! Your appointment is tomorrow at 2 PM."
        );

        if (!success)
            _logger.LogError($"Failed: {error}");
    }
}
```

## Built-in Message Templates

Use pre-made templates instead of writing messages:

```csharp
await _whatsApp.SendWhatsAppTemplateMessageAsync(
    "+1234567890",
    "appointment-confirmation",
    "John Smith",          // Patient name
    "2024-01-20",          // Date
    "2:00 PM",             // Time
    "Dr. Jane Doe",        // Provider
    "Main Clinic"          // Location
);
```

### Available Templates:
- `appointment-confirmation` - Confirm appointment
- `appointment-reminder` - Remind about upcoming appointment
- `appointment-cancelled` - Notify cancellation
- `appointment-rescheduled` - New appointment date
- `invoice-sent` - Invoice ready
- `payment-reminder` - Payment due
- `payment-received` - Payment received
- `followup-consultation` - Schedule follow-up
- `post-procedure-care` - Post-care instructions
- `followup-survey` - Request feedback
- And more...

See `WHATSAPP_INTEGRATION_GUIDE.md` for the complete list.

## Common Tasks

### Send appointment reminder

```csharp
await _whatsApp.SendWhatsAppTemplateMessageAsync(
    patient.PhoneNumber,
    "appointment-reminder",
    patient.Name,
    appointment.Date.ToString("yyyy-MM-dd"),
    appointment.Time,
    appointment.Provider.Name
);
```

### Send invoice notification

```csharp
await _whatsApp.SendWhatsAppTemplateMessageAsync(
    patient.PhoneNumber,
    "invoice-sent",
    patient.Name,
    invoice.Number,
    invoice.Amount.ToString("C"),
    invoice.DueDate.ToString("yyyy-MM-dd")
);
```

### Validate phone number

```csharp
if (!WhatsAppUtilities.IsValidWhatsAppPhoneNumber(phoneNumber))
{
    // Show error to user
}

// Auto-fix phone numbers
var normalized = WhatsAppUtilities.NormalizePhoneNumber("2025551234");
// Returns: "+12025551234"
```

### Send message with image/document

```csharp
await _whatsApp.SendWhatsAppMessageWithMediaAsync(
    "+1234567890",
    "Here's your receipt",
    "https://clinic.com/receipts/123.pdf",
    "document"
);
```

## Phone Number Format

WhatsApp requires **E.164 format**: `+[country code][number]`

Examples:
- USA: `+12025551234`
- UK: `+441632960123`
- Nigeria: `+2348012345678`

**Utility to help:**
```csharp
var formatted = WhatsAppUtilities.NormalizePhoneNumber("2025551234");
// Auto-formats to: +12025551234 (assumes US)
```

## Important Notes

### ⚠️ Sandbox Mode (Testing)
- **Only 100 messages per day**
- **Can only send to registered test numbers**
- **Messages are free**
- Perfect for testing before production

### 🚀 Production Mode
- **Request production approval from Twilio**
- **Requires WhatsApp Business verification**
- **Messages cost ~$0.001-$0.005 each**
- **Can send to any number**

### 🔒 Security
- **Never commit credentials** to Git
- **Use environment variables** in production
- **Remove WhatsAppDebugController** before going live
- **Always validate phone numbers**

## Troubleshooting

| Problem | Solution |
|---------|----------|
| "Auth Token Invalid" | Check AccountSid and AuthToken match Twilio console |
| "Invalid phone number" | Use E.164 format (+country code + number) |
| "Message not sent" | Check phone number is registered in sandbox |
| "WhatsAppConfig not found" | Add config to appsettings.json |

## Next Steps

1. ✅ Configure credentials (see Setup above)
2. ✅ Test with debug controller
3. ✅ Integrate into services (see Common Tasks above)
4. ✅ Add phone numbers to patient records
5. ✅ Request production access from Twilio
6. ✅ Remove debug controller before deployment

## Documentation Files

- **WHATSAPP_INTEGRATION_GUIDE.md** - Complete guide with all details
- **This file** - Quick reference

## Support

- **Twilio Docs:** https://www.twilio.com/docs/whatsapp
- **Twilio Console:** https://www.twilio.com/console

---

**Ready to send messages?** Start with the debug controller to test your setup!
