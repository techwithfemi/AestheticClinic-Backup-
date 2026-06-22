# 🧪 Debug Endpoints Testing Guide

## Quick Start - Test Email Sending in 3 Steps

### Step 1: Check SMTP Configuration

**Endpoint:** `GET http://localhost:5001/api/debug/check-smtp-config`

**Using Browser:**
1. Start the application (F5)
2. Navigate to: `http://localhost:5001/api/debug/check-smtp-config`
3. You should see JSON response

**Expected Response (Success):**
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

**What to Check:**
- ✅ Status should be "OK"
- ✅ All configuration fields populated
- ✅ No null values
- ✅ Email address matches: noreply@logicversion.ng

---

### Step 2: Send Test Email

**Endpoint:** `POST http://localhost:5001/api/debug/send-test-email?testEmail=YOUR_EMAIL`

**Using Browser (Manual):**
1. Open Browser DevTools (F12)
2. Go to Console tab
3. Paste this code:

```javascript
fetch('http://localhost:5001/api/debug/send-test-email?testEmail=your@email.com', {
  method: 'POST'
})
.then(response => response.json())
.then(data => console.log(JSON.stringify(data, null, 2)))
.catch(error => console.error('Error:', error));
```

4. Replace `your@email.com` with your actual email
5. Press Enter

**Expected Response (Success):**
```json
{
  "status": "SUCCESS",
  "message": "Test email sent successfully",
  "email": "your@email.com"
}
```

**Using cURL (PowerShell):**
```powershell
$email = "your@email.com"
$response = Invoke-RestMethod -Uri "http://localhost:5001/api/debug/send-test-email?testEmail=$email" -Method POST
$response | ConvertTo-Json
```

**Using Postman:**
1. Open Postman
2. Create new POST request
3. URL: `http://localhost:5001/api/debug/send-test-email?testEmail=your@email.com`
4. Click Send
5. Check response

---

### Step 3: Check Your Email

After sending test email:

1. **Check Inbox** for email from: `noreply@logicversion.ng`
2. **Check Spam Folder** (sometimes ends up there)
3. **Look for Subject:** `Test Email - AestheticClinic EMR`
4. **Expected Content:** "This is a test email to verify SMTP configuration is working correctly."

---

## Detailed Testing Instructions

### Test 1: Verify SMTP Configuration on Startup

**What to Do:**
1. Start the application (F5)
2. Look at console output
3. Look for this message:

```
[Information] ✅ SMTP Configuration validated successfully:
[Information]    Host: mail.logicversion.ng
[Information]    Port: 8889
[Information]    UseSSL: False
[Information]    Email Address: noreply@logicversion.ng
[Information]    Username: noreply@logicversion.ng
```

**Success Criteria:**
- ✅ Message appears in console
- ✅ Configuration is logged
- ✅ No error messages before this

**If It Fails:**
- ❌ If you see: `⚠️ SmtpConfig is NULL!`
  → Check appsettings.json has SmtpConfig section

- ❌ If app doesn't start
  → Check for exceptions in console
  → Check that all required packages are installed

---

### Test 2: Verify SMTP Configuration via API

**What to Do:**
```
GET http://localhost:5001/api/debug/check-smtp-config
```

**Using Different Methods:**

**Method A: Browser**
```
1. Start app (F5)
2. Open browser
3. Type URL: http://localhost:5001/api/debug/check-smtp-config
4. Press Enter
5. See JSON response
```

**Method B: PowerShell**
```powershell
# Option 1: Invoke-RestMethod (easiest)
$response = Invoke-RestMethod -Uri "http://localhost:5001/api/debug/check-smtp-config"
$response | ConvertTo-Json

# Option 2: Invoke-WebRequest (more details)
$response = Invoke-WebRequest -Uri "http://localhost:5001/api/debug/check-smtp-config"
$response.Content | ConvertFrom-Json | ConvertTo-Json
```

**Method C: cURL**
```bash
curl http://localhost:5001/api/debug/check-smtp-config
```

**Method D: Postman**
```
1. Open Postman
2. Method: GET
3. URL: http://localhost:5001/api/debug/check-smtp-config
4. Click Send
5. View response in "Pretty" JSON format
```

**What to Check in Response:**
```json
{
  "status": "OK",  // ✅ Should be "OK"
  "message": "SMTP Configuration loaded",
  "config": {
    "host": "mail.logicversion.ng",  // ✅ Should match
    "port": 8889,                     // ✅ Should be 8889
    "useSSL": false,                  // ✅ Should be false
    "emailAddress": "noreply@logicversion.ng",  // ✅ Should match
    "name": "VCP Aesthetic Clinic",
    "username": "noreply@logicversion.ng",  // ✅ Should match
    "hasPassword": true  // ✅ Should be true
  }
}
```

---

### Test 3: Send Test Email

**What to Do:**
```
POST http://localhost:5001/api/debug/send-test-email?testEmail=YOUR_EMAIL
```

**PowerShell Script (Copy & Run):**
```powershell
# Full test script
$email = "your-email@example.com"  # ← CHANGE THIS
$endpoint = "http://localhost:5001/api/debug/send-test-email"

Write-Host "Testing email sending to: $email" -ForegroundColor Cyan

try {
    $response = Invoke-RestMethod -Uri "$endpoint`?testEmail=$email" -Method POST

    if ($response.status -eq "SUCCESS") {
        Write-Host "✅ Test email sent successfully!" -ForegroundColor Green
        Write-Host "Email: $($response.email)" -ForegroundColor Green
        Write-Host "Message: $($response.message)" -ForegroundColor Green
        Write-Host "`nCheck your email inbox (including spam folder)" -ForegroundColor Yellow
    }
    else {
        Write-Host "❌ Failed to send email" -ForegroundColor Red
        Write-Host "Status: $($response.status)" -ForegroundColor Red
        Write-Host "Message: $($response.message)" -ForegroundColor Red
    }
}
catch {
    Write-Host "❌ Error sending test email:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}
```

**Quick JavaScript (Browser Console):**
```javascript
// Replace 'your@email.com' with your email
const testEmail = 'your@email.com';

fetch(`http://localhost:5001/api/debug/send-test-email?testEmail=${testEmail}`, {
  method: 'POST'
})
.then(response => response.json())
.then(data => {
  console.log('Response:', data);
  if (data.status === 'SUCCESS') {
    console.log('✅ Email sent successfully! Check your inbox.');
  } else {
    console.log('❌ Failed:', data.message);
  }
})
.catch(error => console.error('❌ Error:', error));
```

**What to Check:**
- ✅ Response status: "SUCCESS"
- ✅ Email address echoed back
- ✅ Message: "Test email sent successfully"

**If It Fails:**
- ❌ "Connection timeout" → SMTP server not accessible
- ❌ "Authentication failed" → Check SMTP credentials
- ❌ "Invalid email" → Email format incorrect

---

### Test 4: Full Forgot Password Flow

**What to Do:**
1. Open login page: `http://localhost:5001`
2. Look for "Forgot Password?" link
3. Click it
4. Enter username or email
5. Click "Send" button
6. Check your email

**Expected Flow:**
```
User clicks "Forgot Password?"
    ↓
Form opens asking for username/email
    ↓
User enters: admin (or your username)
    ↓
User clicks "Send"
    ↓
HTTP 204 response (No Content)
    ↓
Email arrives with password reset link
```

**What to Check:**
- ✅ Form submits without error
- ✅ No error message appears
- ✅ Email arrives within 1 minute
- ✅ Email contains password reset link
- ✅ Can click link and reset password

**Testing This:**

1. **In Application:**
   - Start app (F5)
   - Navigate to login page
   - Click "Forgot Password?"
   - Enter a test username
   - Click "Send"

2. **Monitor Logs:**
   - Watch console output
   - Look for log messages:
     ```
     [Information] Starting password reset email process for user: admin
     [Information] Sending password reset email to: admin@example.com
     [Information] Email sent successfully to admin@example.com
     ```

3. **Check Email:**
   - Open your email client
   - Check inbox for email from noreply@logicversion.ng
   - Check spam folder if not in inbox
   - Click the password reset link

---

## Setting Breakpoints While Testing

### Test 5: Verify Breakpoints Work

**What to Do:**
1. Open `EmailSender.cs`
2. Find the `SendEmailAsync` method with `MailboxAddress sender` parameter
3. Set a breakpoint on line where message is created:
   ```csharp
   var message = new MimeMessage();
   ```

4. Trigger the forgot password flow:
   - Go to login page
   - Click "Forgot Password?"
   - Enter email
   - Click "Send"

5. **Breakpoint should be hit!** ✅

**What You Should See:**
```
Debugging window opens
    ↓
Breakpoint is highlighted (yellow)
    ↓
Variables window shows:
    - sender (MailboxAddress)
    - recipients (MailboxAddress[])
    - subject (string)
    - body (string)
    ↓
_smtpConfig is NOT null ✅
    ↓
Can step through code (F10/F11)
```

**If Breakpoint Doesn't Hit:**
- ❌ Check that you're using the right method
- ❌ Verify configuration is loaded
- ❌ Check for exceptions in output window

---

## Checking Application Logs

### Test 6: Verify Logging

**Where Logs Are:**
- File: `Logs/log-{Date}.log`
- Console: Visual Studio Debug Output window

**What to Look For After Sending Test Email:**

**Success Logs:**
```
[Information] Attempting to send email to your@email.com with subject 'Test Email - AestheticClinic EMR'
[Information] Connecting with StartTLS upgrade (port 8889)
[Information] Connected to SMTP server mail.logicversion.ng:8889 with security: StartTls
[Information] Authenticating with username: noreply@logicversion.ng
[Information] Successfully authenticated with SMTP server as noreply@logicversion.ng
[Information] Email sent successfully to your@email.com
```

**Error Logs (If Something Goes Wrong):**
```
[Error] An error occurred whilst sending email to your@email.com with subject 'Test Email'
[Error] Connection timeout
[Error] Authentication failed
[Error] Invalid configuration
```

**How to View Logs in Visual Studio:**
1. Debug → Windows → Output
2. Look for messages from EmailSender
3. Search for "Email" or "SMTP"

---

## Complete Testing Checklist

```
STARTUP TESTS:
  [ ] Application starts without errors
  [ ] Console shows SMTP validation message
  [ ] No exceptions in output

CONFIGURATION TESTS:
  [ ] GET /api/debug/check-smtp-config returns OK
  [ ] All SMTP fields are populated
  [ ] No null values

EMAIL SENDING TESTS:
  [ ] POST /api/debug/send-test-email returns SUCCESS
  [ ] Email received in inbox (check spam too)
  [ ] Email shows correct sender (noreply@logicversion.ng)
  [ ] Email shows correct subject (Test Email - AestheticClinic EMR)

FORGOT PASSWORD TESTS:
  [ ] Click "Forgot Password" on login
  [ ] Enter username/email
  [ ] Submit form
  [ ] HTTP 204 response
  [ ] Email arrives with reset link
  [ ] Can click reset link
  [ ] Can complete password reset

DEBUGGING TESTS:
  [ ] Set breakpoint in EmailSender.SendEmailAsync
  [ ] Trigger forgot password flow
  [ ] Breakpoint is hit
  [ ] Can step through code
  [ ] SmtpConfig is not null

LOGGING TESTS:
  [ ] Check console output
  [ ] Look for "Email sent successfully" message
  [ ] Check Logs directory for log file
  [ ] Verify email sending is logged
```

---

## Troubleshooting While Testing

### Issue: Configuration Returns NULL

**Error Response:**
```json
{
  "status": "ERROR",
  "message": "SmtpConfig is null",
  "config": null
}
```

**Solution:**
1. Check `appsettings.json`
2. Verify SmtpConfig section exists
3. Verify all fields are populated:
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
4. Restart application

---

### Issue: Connection Timeout

**Error Response:**
```json
{
  "status": "ERROR",
  "message": "Connection timeout"
}
```

**Solution:**
1. Verify SMTP server is online: `mail.logicversion.ng`
2. Verify port 8889 is accessible
3. Check firewall isn't blocking connection
4. Try different test email address
5. Contact email provider for status

---

### Issue: Authentication Failed

**Error Response:**
```json
{
  "status": "ERROR",
  "message": "Authentication failed"
}
```

**Solution:**
1. Verify SMTP credentials:
   - Username: noreply@logicversion.ng
   - Password: Khide@321!!
2. Check if account is active
3. Check if account allows SMTP access
4. Contact email provider to verify access

---

### Issue: Email Not Arriving

**Solution:**
1. Check spam/junk folder
2. Verify email address is correct
3. Check SMTP server logs (if accessible)
4. Try sending to different email address
5. Check mail server status
6. Verify no filters blocking emails

---

## Quick Reference

### URLs
```
Check Configuration:
  http://localhost:5001/api/debug/check-smtp-config

Send Test Email:
  http://localhost:5001/api/debug/send-test-email?testEmail=YOUR_EMAIL

Forgot Password:
  http://localhost:5001/login (then click "Forgot Password?")
```

### Expected Responses
```
Configuration Check - Success:
  Status: OK

Test Email - Success:
  Status: SUCCESS

Forgot Password - Success:
  HTTP 204 No Content
  Email arrives
```

### Log Patterns
```
Success:
  "Email sent successfully to"

Error:
  "An error occurred whilst sending email"
  "Connection timeout"
  "Authentication failed"
```

---

## Next Steps

1. ✅ Run Test 1 (Verify Configuration on Startup)
2. ✅ Run Test 2 (Check Configuration via API)
3. ✅ Run Test 3 (Send Test Email)
4. ✅ Run Test 4 (Test Forgot Password Flow)
5. ✅ Run Test 5 (Verify Breakpoints Work)
6. ✅ Run Test 6 (Verify Logging)
7. ✅ Complete Testing Checklist

**If All Pass:** Email sending is working! ✅

---

**Status: Ready to Test** ✅
