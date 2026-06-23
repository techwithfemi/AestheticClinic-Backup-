# WhatsApp Configuration Setup Guide

## Quick Configuration

Add or update the `WhatsAppConfig` section in your `appsettings.json`:

```json
"WhatsAppConfig": {
  "Enabled": false,
  "AccountSid": "ACxxxxxxxxxxxxx",
  "AuthToken": "your_auth_token_here",
  "FromPhoneNumber": "+1234567890",
  "MaxRetries": 3
}
```

## Step-by-Step Setup

### 1. Create Twilio Account
- Go to https://www.twilio.com
- Sign up for a free account
- Verify your email

### 2. Get WhatsApp Sandbox Credentials
- Login to Twilio Console: https://www.twilio.com/console
- Navigate to: **Messaging** → **WhatsApp** → **Sandbox**
- You'll see:
  - Your **WhatsApp Sandbox Number** (e.g., +1 234 567 8901)
  - A test message to send to activate

### 3. Find Your Account SID and Auth Token
- In the Twilio Console Dashboard
- Look for **Account SID** (starts with `AC`, like: `ACxxxxxxxxxxxxx`)
- Look for **Auth Token** (your secret key)

### 4. Update appsettings.json

```json
"WhatsAppConfig": {
  "Enabled": true,
  "AccountSid": "ACxxxxxxxxxxxxx",  // Your Account SID from Twilio
  "AuthToken": "your_token_here",   // Your Auth Token from Twilio
  "FromPhoneNumber": "+1234567890", // Your WhatsApp Sandbox number
  "MaxRetries": 3
}
```

### 5. Register Test Numbers (Sandbox)
- In Twilio Console: Messaging → WhatsApp → Sandbox
- Under "Sandbox Participants", add your test phone numbers
- Reply to the activation message from Twilio
- Wait for confirmation (usually instant)

### 6. Test It!
- Run your application
- Go to: `/api/whatsappdebug/templates` in your browser
- Should see list of templates
- Try sending a test message with the debug controller

## For Production

### Request Production Access
1. In Twilio Console, request WhatsApp Business API access
2. Setup WhatsApp Business Account
3. Get approval from Twilio & WhatsApp
4. Update credentials in production `appsettings.Production.json`

### Environment Variables (Recommended)
Instead of hardcoding in appsettings.json, use environment variables:

```powershell
# PowerShell
$env:WhatsAppConfig__AccountSid = "ACxxxxxxxxxxxxx"
$env:WhatsAppConfig__AuthToken = "your_token_here"
$env:WhatsAppConfig__FromPhoneNumber = "+1234567890"
$env:WhatsAppConfig__Enabled = "true"
```

Or in `.env` file:
```
WhatsAppConfig__AccountSid=ACxxxxxxxxxxxxx
WhatsAppConfig__AuthToken=your_token_here
WhatsAppConfig__FromPhoneNumber=+1234567890
WhatsAppConfig__Enabled=true
```

## Configuration Fields Explained

| Field | Example | Description |
|-------|---------|-------------|
| `Enabled` | `true` | Enable/disable WhatsApp feature |
| `AccountSid` | `ACxxxxxxxxxxxxx` | Your Twilio Account ID (from Console) |
| `AuthToken` | `your_token_here` | Your Twilio Auth Token (from Console) |
| `FromPhoneNumber` | `+1234567890` | WhatsApp sender number in E.164 format |
| `MaxRetries` | `3` | Retry attempts for failed messages |

## Troubleshooting

### "WhatsAppConfig not found"
- Check spelling in appsettings.json
- Ensure JSON is valid (use online JSON validator)
- Restart the application

### "Invalid AccountSid or AuthToken"
- Copy exactly from Twilio Console (no spaces)
- Ensure you're using the correct account
- Check token hasn't expired

### "Invalid phone number"
- Must be in E.164 format: `+[country code][number]`
- Example USA: `+12025551234`
- No spaces, parentheses, or dashes

### Messages not sending
- Check `Enabled` is set to `true`
- Verify credentials are correct
- In sandbox mode, number must be registered
- Check application logs for detailed errors

## Testing the Configuration

### Using Debug Controller
```bash
# Get list of templates
curl "https://localhost:5001/api/whatsappdebug/templates" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Validate phone number
curl "https://localhost:5001/api/whatsappdebug/validate-phone?phoneNumber=%2B12025551234" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Send test message
curl -X POST "https://localhost:5001/api/whatsappdebug/send-message?phoneNumber=%2B12025551234&message=Test%20message" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## Security Notes

⚠️ **IMPORTANT:**
- Never commit credentials to Git
- Use environment variables in production
- Use secrets manager in Azure/AWS
- Rotate tokens regularly
- Restrict API access to authorized users only

## Next Steps

1. Create Twilio account (5 min)
2. Get credentials (5 min)
3. Update appsettings.json (2 min)
4. Register test number (5 min)
5. Test with debug controller (10 min)
6. Integrate into your services (1-2 hours)
7. Request production access when ready

## Quick Links

- **Twilio Console:** https://www.twilio.com/console
- **WhatsApp Setup:** https://www.twilio.com/docs/whatsapp/quickstart
- **Twilio Docs:** https://www.twilio.com/docs/whatsapp
- **E.164 Format:** https://en.wikipedia.org/wiki/E.164

---

**Total setup time:** 20-30 minutes
**Cost:** Free for testing (sandbox mode)
**Production cost:** $0.001-$0.005 per message
