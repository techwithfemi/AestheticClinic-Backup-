# Email Sending Issue - Debugging & Resolution Guide

## Overview
This document outlines the issues found and fixed in the password reset email functionality, and provides steps to verify everything is working correctly.

---

## Issues Found & Fixed

### 1. **EmailSender Initialization Problem**
**Status:** ✅ FIXED

**Problem:**
```csharp
// BEFORE (PROBLEMATIC)
public class EmailSender(IOptions<AppSettings> config, ILogger<EmailSender> logger) : IEmailSender
{
    private readonly SmtpConfig config = config.Value.SmtpConfig!;  // ❌ ISSUES:
    // 1. Field name shadows parameter name
    // 2. Null-forgiving operator (!) ignores null values
    // 3. Silent failure if SmtpConfig is null
}
```

**Solution:**
```csharp
public class EmailSender(IOptions<AppSettings> configOptions, ILogger<EmailSender> logger) : IEmailSender
{
    private readonly SmtpConfig _smtpConfig = configOptions.Value.SmtpConfig 
        ?? throw new InvalidOperationException("SmtpConfig is not configured in appsettings.json");
    // ✅ Better error messaging
    // ✅ Fails fast during initialization
    // ✅ No null reference exceptions later
}
```

### 2. **Missing Logging in UserAccountService**
**Status:** ✅ FIXED

Added comprehensive logging to track the password reset flow:
- Service initialization logging
- Email sending attempt logging
- Error logging with detailed messages
- Success confirmation logging

### 3. **No SMTP Configuration Validation**
**Status:** ✅ FIXED

Created `SmtpConfigValidationService` that:
- Validates SMTP configuration on application startup
- Logs configuration details to help debug issues
- Identifies missing or invalid configuration early

---

## Testing Steps

### Step 1: Verify SMTP Configuration is Loaded
Run the application and check the console output. You should see:
```
✅ SMTP Configuration validated successfully:
   Host: mail.logicversion.ng
   Port: 8889
   UseSSL: False
   Email Address: noreply@logicversion.ng
   Username: noreply@logicversion.ng
```

If you see `⚠️ SmtpConfig is NULL!` or any other warning, check:
1. Your `appsettings.json` file has the SmtpConfig section
2. The configuration values are correct
3. The application is reading the correct configuration file

### Step 2: Check SMTP Configuration via API
Use the debug endpoint to verify configuration:

**Endpoint:** `GET /api/debug/check-smtp-config`
**Authorization:** None required (for testing)

**Expected Response:**
```json
{
  "status": "OK",
  "message": "SMTP Configuration loaded",
  "config": {
    "host": "mail.logicversion.ng",
    "port": 8889,
    "useSSL": false,
    "emailAddress": "noreply@logicversion.ng",
    "name": "VCP Aesthetic Clinic",
    "username": "noreply@logicversion.ng",
    "hasPassword": true
  }
}
```

### Step 3: Send a Test Email
Use the debug endpoint to send a test email:

**Endpoint:** `POST /api/debug/send-test-email?testEmail=yourtest@example.com`
**Authorization:** None required (for testing)
**Method:** POST
**Query Parameter:** `testEmail` (your test email address)

**Expected Response (Success):**
```json
{
  "status": "SUCCESS",
  "message": "Test email sent successfully",
  "email": "yourtest@example.com"
}
```

**Expected Response (Error):**
```json
{
  "status": "ERROR",
  "message": "Connection timeout",
  "stackTrace": "..."
}
```

Check your logs for detailed error messages if the request fails.

### Step 4: Test the Actual Forgot Password Flow
1. Open the login page
2. Click "Forgot Password"
3. Enter a valid username or email
4. Submit the form

**What should happen:**
- HTTP 204 (No Content) response
- Email should be sent to the user's email address
- Check the application logs for confirmation

**If it fails:**
- Check the browser console for error messages
- Check the application logs in the Output window
- Look for error messages from the EmailSender or UserAccountService

---

## Debugging Guide

### Breakpoints Not Being Hit?

**Solution:** The issue was likely in the EmailSender initialization. With the fixes in place:
1. Set a breakpoint in the `SendEmailAsync` method in EmailSender
2. Trigger the forgot password flow
3. The breakpoint should now be hit

**If breakpoints still aren't hit:**
1. Check the Output window for any initialization errors
2. Verify SMTP configuration is loaded (Step 2 above)
3. Check that UserAccountService is properly receiving the EmailSender instance

### Mail Not Being Sent?

**Check these in order:**

1. **SMTP Configuration**
   - Is SmtpConfig loading correctly? (Step 2 above)
   - Are all required fields present (Host, Port, EmailAddress)?

2. **Network Connectivity**
   - Can the application reach the SMTP server?
   - Use the test email endpoint (Step 3) to verify

3. **SMTP Credentials**
   - Username and Password correct?
   - Account has permission to send emails?

4. **Port & Encryption**
   - Is the SMTP server using the correct port?
   - Port 8889 with UseSSL=false should use StartTLS (port 587) or plain connection
   - Port 465 with UseSSL=true should use SSL/TLS on connect

5. **Firewall/Network Issues**
   - Is the SMTP server accessible from your network?
   - Are there any firewall rules blocking the connection?

### Checking the Logs

The application uses file logging. Check your logs directory:
**Default:** `{application-root}/Logs/log-{Date}.log`

Look for messages like:
```
[Information] Attempting to send email to user@example.com with subject 'Password Reset Request'
[Information] Connected to SMTP server mail.logicversion.ng:8889
[Information] Email sent successfully to user@example.com
```

Or error messages:
```
[Error] An error occurred whilst sending email: Connection timeout
[Error] Failed to send password reset email to user@example.com: Authentication failed
```

---

## Key Code Changes

### 1. EmailSender.cs
- Fixed configuration initialization
- Better error handling
- Proper SMTP connection logic

### 2. UserAccountService.cs
- Added ILogger dependency
- Added comprehensive logging to SendPasswordResetEmailAsync
- Added try-catch for error handling

### 3. SmtpConfigValidationService.cs (New)
- Validates SMTP configuration on startup
- Provides clear feedback on configuration status
- Helps identify issues early

### 4. EmailDebugController.cs (New)
- Debug endpoints to test email configuration
- Check SMTP config endpoint: `GET /api/debug/check-smtp-config`
- Send test email endpoint: `POST /api/debug/send-test-email?testEmail=...`

---

## SMTP Configuration Reference

Your current configuration in `appsettings.json`:
```json
"SmtpConfig": {
  "Host": "mail.logicversion.ng",
  "Port": 8889,
  "UseSSL": false,
  "Name": "VCP Aesthetic Clinic",
  "Username": "noreply@logicversion.ng",
  "EmailAddress": "noreply@logicversion.ng",
  "Password": "Khide@321!!"
}
```

**What each field means:**
- **Host:** SMTP server address
- **Port:** SMTP server port (8889 is non-standard, verify with your email provider)
- **UseSSL:** If true, uses SSL/TLS on connect (port 465). If false, might use StartTLS (port 587)
- **Name:** Display name for sender (appears in email "From" field)
- **EmailAddress:** Email address for sender
- **Username:** SMTP authentication username
- **Password:** SMTP authentication password

---

## Production Notes

⚠️ **Important:** The `EmailDebugController` debug endpoints are included in the code for testing purposes. Before deploying to production:

1. **Remove or secure the debug endpoints** - Either delete EmailDebugController.cs or protect it with authentication
2. **Review the SMTP configuration** - Ensure credentials are secure
3. **Test email sending** in a staging environment first
4. **Monitor logs** for any email-related errors in production

---

## Next Steps

1. ✅ Verify fixes are in place (all 4 files modified/created)
2. ✅ Run the application
3. ✅ Check logs for SMTP configuration validation message
4. ✅ Test SMTP configuration using debug endpoint
5. ✅ Send test email using debug endpoint
6. ✅ Test forgot password flow end-to-end
7. ✅ Check production readiness (remove debug endpoints if needed)

---

## Contact & Support

If you continue to experience issues:
1. Check the debugging guide above
2. Review logs for specific error messages
3. Verify SMTP server connectivity
4. Test SMTP credentials with another email client
5. Contact your email provider for SMTP configuration details
