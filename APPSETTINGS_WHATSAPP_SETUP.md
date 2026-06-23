# ✅ appsettings.json - WhatsApp Configuration Added

## What's New

Your `appsettings.json` now includes the WhatsApp configuration section!

## Current Configuration

Your `appsettings.json` now has:

```json
"WhatsAppConfig": {
  "Enabled": false,
  "AccountSid": "ACxxxxxxxxxxxxx",
  "AuthToken": "your_auth_token_here",
  "FromPhoneNumber": "+1234567890",
  "MaxRetries": 3
}
```

## How to Enable WhatsApp

### Step 1: Get Twilio Credentials (5 minutes)
1. Go to https://www.twilio.com/console
2. Find your **Account SID** (looks like: `ACxxxxxxxxxxxxx`)
3. Find your **Auth Token** (your secret key)
4. Go to Messaging → WhatsApp → Sandbox
5. Note your **WhatsApp Sandbox Number**

### Step 2: Update appsettings.json

Open `AestheticEMR\AestheticEMR.Server\appsettings.json` and find:

```json
"WhatsAppConfig": {
  "Enabled": false,
  "AccountSid": "ACxxxxxxxxxxxxx",
  "AuthToken": "your_auth_token_here",
  "FromPhoneNumber": "+1234567890",
  "MaxRetries": 3
}
```

Replace with your actual credentials:

```json
"WhatsAppConfig": {
  "Enabled": true,
  "AccountSid": "AC1234567890abcdefghijklmno",
  "AuthToken": "your_actual_token_here",
  "FromPhoneNumber": "+14155552671",
  "MaxRetries": 3
}
```

### Step 3: Register Test Number (Sandbox Only)

In Twilio Console:
- Messaging → WhatsApp → Sandbox
- Under "Sandbox Participants", add your phone number
- Reply to the SMS from Twilio to activate

### Step 4: Test It!

Run your application and test:

```
GET /api/whatsappdebug/templates
```

Should return list of available templates.

## Configuration Fields

| Field | Required | Default | Example | Notes |
|-------|----------|---------|---------|-------|
| `Enabled` | No | `false` | `true` | Enable/disable feature |
| `AccountSid` | Yes | `ACxxxxxxxxxxxxx` | `AC1234567890abcdef` | From Twilio Console |
| `AuthToken` | Yes | `your_auth_token_here` | `actual_token_string` | From Twilio Console |
| `FromPhoneNumber` | Yes | `+1234567890` | `+14155552671` | WhatsApp number in E.164 |
| `MaxRetries` | No | `3` | `3` | Retry attempts |

## Easy Copy-Paste Setup

### For Sandbox Testing

```json
"WhatsAppConfig": {
  "Enabled": true,
  "AccountSid": "ACxxxxxxxxxxxxx",
  "AuthToken": "your_auth_token_from_console",
  "FromPhoneNumber": "+1YOUR_SANDBOX_NUMBER",
  "MaxRetries": 3
}
```

### For Production

```json
"WhatsAppConfig": {
  "Enabled": true,
  "AccountSid": "AC_PRODUCTION_SID",
  "AuthToken": "production_auth_token",
  "FromPhoneNumber": "+1YOUR_BUSINESS_NUMBER",
  "MaxRetries": 3
}
```

## File Locations

- **Main config:** `AestheticEMR\AestheticEMR.Server\appsettings.json`
- **Example file:** `appsettings.json.example`
- **Setup guide:** `WHATSAPP_CONFIGURATION.md`
- **Full documentation:** `WHATSAPP_INTEGRATION_GUIDE.md`

## ⚠️ Important Security Notes

### Development/Testing
- ✅ OK to use test credentials in appsettings.json for local development
- ✅ Use sandbox mode (free, 100 messages/day)
- ✅ Only works with registered test numbers

### Production
- ❌ Never commit real credentials to Git
- ❌ Use environment variables or secrets manager
- ❌ Rotate tokens regularly
- ❌ Use production-approved credentials only

### In .gitignore (already should be there)
```
appsettings.json
appsettings.Production.json
*.env
*.local
```

## Quick Verification

### Test if config is loaded:

```csharp
public class TestController : ControllerBase
{
    public TestController(IOptions<AppSettings> settings)
    {
        var whatsappConfig = settings.Value.WhatsAppConfig;
        var isEnabled = whatsappConfig?.Enabled ?? false;
        _logger.LogInformation($"WhatsApp Enabled: {isEnabled}");
    }
}
```

### Or use the debug controller:

```
GET /api/whatsappdebug/templates
```

If you get a list of templates, config is working!

## Troubleshooting

### "WhatsAppConfig is null"
- Check JSON syntax in appsettings.json
- Verify indentation is correct
- Use online JSON validator if unsure
- Restart application after changes

### "Invalid credentials"
- Copy AccountSid and AuthToken exactly from Twilio
- No extra spaces or characters
- Ensure you're in correct Twilio account

### "Invalid phone number"
- Must include `+` sign
- Must include country code (e.g., +1 for USA)
- Example: `+12025551234` (not `2025551234`)

### Messages not sending
- Check `Enabled` is `true`
- Check all credentials are correct
- In sandbox, number must be registered
- Check application logs

## Next Steps

1. ✅ Copy your credentials from Twilio
2. ✅ Update appsettings.json
3. ✅ Restart application
4. ✅ Test with `/api/whatsappdebug/templates`
5. ✅ Send test message
6. ✅ Integrate into services

## Example Full appsettings.json Section

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;"
  },

  "SmtpConfig": {
    "Host": "...",
    "Port": 587,
    "UseSSL": true,
    "Name": "Your Clinic",
    "EmailAddress": "noreply@clinic.com",
    "Password": "email_password"
  },

  "WhatsAppConfig": {
    "Enabled": true,
    "AccountSid": "ACxxxxxxxxxxxxx",
    "AuthToken": "your_auth_token_here",
    "FromPhoneNumber": "+1234567890",
    "MaxRetries": 3
  },

  "ClientBaseUrl": "http://localhost:4200",

  "Logging": {
    "PathFormat": "Logs/log-{Date}.log"
  },

  "AllowedHosts": "*"
}
```

## Reference Links

- **Twilio Console:** https://www.twilio.com/console
- **WhatsApp Setup:** https://www.twilio.com/docs/whatsapp
- **Configuration Guide:** WHATSAPP_CONFIGURATION.md
- **Full Integration Guide:** WHATSAPP_INTEGRATION_GUIDE.md

---

**Status:** ✅ Updated in appsettings.json
**Example File:** ✅ Created (appsettings.json.example)
**Build Status:** ✅ Successful

Ready to configure and use WhatsApp!
