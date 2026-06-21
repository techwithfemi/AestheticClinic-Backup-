# Email Configuration Testing Guide

## Changes Made

### 1. Updated SMTP Configuration
**File:** `AestheticEMR.Server/appsettings.json`

Changed from:
- Username: `noreply@logicversiononline.com`
- Email Address: `noreply@logicversiononline.com`

To:
- Username: `info@logicversion.ng`
- Email Address: `info@logicversion.ng`

### 2. Added Test Email Endpoint
**File:** `AestheticEMR.Server/Controllers/UserAccountController.cs`

New endpoint: `POST /api/account/test-email?recipientEmail={email}`

This endpoint allows you to test email sending without going through the full password reset flow.

### 3. Added Enhanced Logging
**Files Modified:**
- `AestheticEMR.Server/Services/Email/EmailSender.cs` - Detailed SMTP logging
- `AestheticEMR.Core/Services/Account/UserAccountService.cs` - Professional email template

## Testing Steps

### Step 1: Rebuild and Restart Application

1. **Close the running application** (Stop debugging in Visual Studio)
2. **Rebuild the solution**:
   - Right-click on solution → Rebuild Solution
   - Or press `Ctrl+Alt+F7`
3. **Start the application** (F5 or Ctrl+F5)
4. **Wait for the application to fully start** (watch the console for startup messages)

### Step 2: Test Email Configuration Using Test Endpoint

#### Using Postman or curl:

```bash
# Replace your-test-email@gmail.com with your actual email
curl -X POST "https://localhost:7085/api/account/test-email?recipientEmail=your-test-email@gmail.com"
```

#### Using Swagger UI:

1. Navigate to: `https://localhost:7085/swagger`
2. Find the `POST /api/account/test-email` endpoint
3. Click "Try it out"
4. Enter your email in the `recipientEmail` parameter
5. Click "Execute"

### Step 3: Check Application Logs

Immediately after sending the test email:

```powershell
# Search for "Attempting to send email" in the latest log
Get-Content "C:\Users\Administrator\source\repos\Medicals\AestheticClinic\AestheticEMR\AestheticEMR.Server\Logs\log-$(Get-Date -Format yyyyMMdd).log" | Select-String -Pattern "email|Email|SMTP" -Context 1
```

#### Expected Log Output (Success):

```
[INF] Attempting to send email to your-email@gmail.com with subject 'Test Email' from info@logicversion.ng
[INF] Connected to SMTP server mail5005.smarterasp.net:8889
[INF] Authenticated with SMTP server as info@logicversion.ng
[INF] Email sent successfully to your-email@gmail.com
```

#### Expected Log Output (Failure):

```
[ERR] An error occurred whilst sending email to your-email@gmail.com with subject 'Test Email'
[ERR] [Details of the specific error]
```

### Step 4: Check Your Email

1. **Check Inbox** - Look for an email from "VCP Aesthetic Clinic"
2. **Check Spam/Junk Folder** - Gmail often filters unknown senders
3. **Whitelist the sender**:
   - In Gmail: Add the sender to contacts or mark as "Not Spam"
   - In other email clients: Similar process

## Troubleshooting

### Issue 1: No Log Entries for Email

**Meaning:** The test endpoint wasn't called or the code wasn't rebuilt

**Solution:**
1. Verify the application was restarted (check the startup logs)
2. Verify the application is running on port 7085
3. Make sure you're calling the correct endpoint

### Issue 2: Authentication Failed Error

```
[ERR] Authentication failed: ...
```

**Meaning:** SMTP credentials are incorrect

**Solution:**
1. Verify in `appsettings.json`:
   - Username: `info@logicversion.ng`
   - Password: `logic@123`
2. Test credentials directly with the mail provider
3. Check if the account is locked or restricted

### Issue 3: Connection Refused Error

```
[ERR] Could not connect to host 'mail5005.smarterasp.net' on port 8889
```

**Meaning:** SMTP server is unreachable

**Solution:**
1. Check internet connection
2. Verify firewall isn't blocking port 8889
3. Verify the SMTP host and port are correct
4. Contact smarterasp.net support

### Issue 4: SSL/TLS Error

```
[ERR] The SMTP server does not support the requested encryption method
```

**Meaning:** SSL/UseSSL setting is incorrect

**Current Configuration:** `UseSSL: false` (Correct for port 8889)

**Solution:**
- Do NOT change to `UseSSL: true` (unless you change port)

### Issue 5: Email Sent But Not Received

**Meaning:** SMTP accepted the email but it's being rejected downstream or flagged as spam

**Solutions:**
1. Check Gmail spam folder first
2. Check your email provider's settings for:
   - Forwarding rules
   - Email filters
   - Blocked senders list
3. Contact email provider about:
   - SPF/DKIM/DMARC configuration
   - Domain authentication records
   - IP reputation

## After Verifying Email Works

### Test Password Reset Feature

Once the test email works:

1. Go to: `https://localhost:4200/login`
2. Click "Forgot Password"
3. Enter your test email address
4. Click "Send reset link"
5. Check your email for the password reset link

### Check Logs for Password Reset

```powershell
Get-Content "C:\Users\Administrator\source\repos\Medicals\AestheticClinic\AestheticEMR\AestheticEMR.Server\Logs\log-$(Get-Date -Format yyyyMMdd).log" | Select-String -Pattern "Password Reset" -Context 2
```

## Configuration Alternatives

If email still doesn't work with smarterasp.net, consider:

### Option 1: Gmail SMTP
```json
"SmtpConfig": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "UseSSL": true,
  "Name": "VCP Aesthetic Clinic",
  "Username": "your-gmail@gmail.com",
  "EmailAddress": "your-gmail@gmail.com",
  "Password": "[App-Specific Password]"
}
```

### Option 2: SendGrid
- Sign up at sendgrid.com
- Create API key
- Much better deliverability

### Option 3: Mailgun
- Sign up at mailgun.com
- Easy integration
- Good for production

## Reference Files

- Updated appsettings.json with new email address
- UserAccountController.cs with test-email endpoint
- UserAccountService.cs with SendTestEmailAsync method
- EmailSender.cs with enhanced logging
