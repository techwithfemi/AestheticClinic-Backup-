# 📸 Step-by-Step Visual Testing Guide

## 🚀 Quick Start - 3 Steps

### STEP 1: Start the Application

```
Visual Studio
    ↓
Press F5 (or Debug → Start Debugging)
    ↓
Wait for:
  - Browser opens
  - Application loads
  - Console shows startup messages
    ↓
Look for this message in Output:
  ✅ SMTP Configuration validated successfully:
     Host: mail.logicversion.ng
     Port: 8889
     ...
```

**What You Should See:**
- Application running on `http://localhost:5001`
- Console shows no errors
- SMTP validation message appears

---

### STEP 2: Test Configuration via Browser

**In the Address Bar:**
```
http://localhost:5001/api/debug/check-smtp-config
```

**Press Enter**

**Expected Response (in browser):**
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

**Check These:**
- ✅ `status` is "OK"
- ✅ `host` is "mail.logicversion.ng"
- ✅ `port` is 8889
- ✅ `hasPassword` is true
- ✅ No null values

---

### STEP 3: Send Test Email

**In the Address Bar:**
```
http://localhost:5001/api/debug/send-test-email?testEmail=YOUR_EMAIL@gmail.com
```

Replace `YOUR_EMAIL@gmail.com` with your actual email

**Method: POST**

**In Browser:**
- For GET/POST requests from address bar, just type and press Enter
- Browser defaults to POST for form submissions

**Expected Response (in browser):**
```json
{
  "status": "SUCCESS",
  "message": "Test email sent successfully",
  "email": "your.email@gmail.com"
}
```

**Check These:**
- ✅ `status` is "SUCCESS"
- ✅ Email shows correct address
- ✅ No error message

**Now Check Your Email:**
- Open Gmail / Outlook / your email client
- Look for email from: `noreply@logicversion.ng`
- Subject: `Test Email - AestheticClinic EMR`
- It might be in SPAM folder

---

## 🎯 Detailed Testing Scenarios

### Scenario 1: Using Postman

**Step 1: Open Postman**
```
Download from postman.com (if you don't have it)
Or use browser's Network Tools (F12)
```

**Step 2: Create GET Request**
```
Method: GET
URL: http://localhost:5001/api/debug/check-smtp-config
```

**Step 3: Click Send**
```
See response on right side:
Status: 200 OK
Body: JSON with SMTP configuration
```

**Step 4: Create POST Request**
```
Method: POST
URL: http://localhost:5001/api/debug/send-test-email?testEmail=your@email.com
```

**Step 5: Click Send**
```
See response:
Status: 200 OK
Body: SUCCESS message
```

---

### Scenario 2: Using PowerShell

**Open PowerShell:**
```
Windows Key → type "PowerShell" → Enter
```

**Copy this command:**
```powershell
Invoke-RestMethod -Uri "http://localhost:5001/api/debug/check-smtp-config" | ConvertTo-Json
```

**Paste into PowerShell and press Enter**

**Expected output:**
```
status        : OK
message       : SMTP Configuration loaded
config        : @{host=mail.logicversion.ng; port=8889; ...}
```

**For sending test email:**
```powershell
$email = "your@email.com"
Invoke-RestMethod -Uri "http://localhost:5001/api/debug/send-test-email?testEmail=$email" -Method Post | ConvertTo-Json
```

---

### Scenario 3: Using Browser Developer Tools

**Step 1: Open Developer Tools**
```
Press F12
```

**Step 2: Go to Console Tab**
```
Click "Console" in the tabs
```

**Step 3: Copy this JavaScript**
```javascript
fetch('http://localhost:5001/api/debug/check-smtp-config')
  .then(r => r.json())
  .then(data => console.log(data))
```

**Step 4: Paste in Console and Press Enter**
```
Output shows SMTP configuration
```

**For sending email:**
```javascript
fetch('http://localhost:5001/api/debug/send-test-email?testEmail=your@email.com', {
  method: 'POST'
})
.then(r => r.json())
.then(data => console.log(data))
```

---

## ✅ Complete Verification Workflow

```
┌─────────────────────────────────────────┐
│ STEP 1: Start Application               │
│ Action: Press F5                        │
│ Check: App starts, SMTP message appears │
└─────────────────────────────────────────┘
              ↓ Verified ✅
┌─────────────────────────────────────────┐
│ STEP 2: Check Configuration             │
│ Action: GET /api/debug/check-smtp-config│
│ Check: Status = OK, all fields present  │
└─────────────────────────────────────────┘
              ↓ Verified ✅
┌─────────────────────────────────────────┐
│ STEP 3: Send Test Email                 │
│ Action: POST .../send-test-email        │
│ Check: Status = SUCCESS                 │
└─────────────────────────────────────────┘
              ↓ Verified ✅
┌─────────────────────────────────────────┐
│ STEP 4: Check Email Inbox               │
│ Action: Open email client               │
│ Check: Email received from noreply@...  │
└─────────────────────────────────────────┘
              ↓ Verified ✅
┌─────────────────────────────────────────┐
│ STEP 5: Test Forgot Password            │
│ Action: Click forgot password link      │
│ Check: Email sent, password can reset   │
└─────────────────────────────────────────┘
              ↓ All Verified ✅
┌─────────────────────────────────────────┐
│ ✅ EMAIL SYSTEM WORKING PERFECTLY! ✅  │
└─────────────────────────────────────────┘
```

---

## 🔍 Debugging Breakpoints

### Setting a Breakpoint

**Step 1: Open EmailSender.cs**
```
File → Open File → EmailSender.cs
Location: AestheticEMR/AestheticEMR.Server/Services/Email/
```

**Step 2: Find SendEmailAsync method**
```csharp
public async Task<(bool success, string? errorMsg)> SendEmailAsync(
    MailboxAddress sender,
    MailboxAddress[] recipients,
    string subject,
    string body,
    bool isHtml = true)
{
    var message = new MimeMessage();  // ← Click here
```

**Step 3: Click on the line number**
```
Red dot appears = breakpoint set
```

**Step 4: Trigger the forgot password flow**
```
Go to: http://localhost:5001
Click: Forgot Password
Enter: username/email
Click: Send
```

**Step 5: Execution stops at breakpoint**
```
Yellow highlight shows execution point
Can inspect variables:
  - _smtpConfig (should be loaded)
  - sender (should have email)
  - recipients (should have email address)
```

**Step 6: Step through code**
```
F10 = Step over (next line)
F11 = Step into (enter function)
F5  = Continue
```

---

## 📊 Reading Test Results

### Configuration Test Response

**✅ Success (Status = OK):**
```json
{
  "status": "OK",
  "message": "SMTP Configuration loaded",
  "config": {
    "host": "mail.logicversion.ng",     ✅
    "port": 8889,                       ✅
    "useSSL": false,                    ✅
    "emailAddress": "noreply@logicversion.ng",  ✅
    "hasPassword": true                 ✅
  }
}
```

**❌ Failure (Status = ERROR):**
```json
{
  "status": "ERROR",
  "message": "SmtpConfig is NULL",
  "config": null
}
```

**Action:** Check appsettings.json has SmtpConfig section

---

### Email Sending Test Response

**✅ Success (Status = SUCCESS):**
```json
{
  "status": "SUCCESS",
  "message": "Test email sent successfully",
  "email": "your@example.com"
}
```

**❌ Failure - Connection (Status = ERROR):**
```json
{
  "status": "ERROR",
  "message": "Connection timeout",
  "stackTrace": "..."
}
```

**Action:** Verify SMTP server is accessible

**❌ Failure - Authentication:**
```json
{
  "status": "ERROR",
  "message": "Authentication failed",
  "stackTrace": "..."
}
```

**Action:** Check SMTP username and password

---

## 📱 Email Format

### Expected Test Email

**From:** VCP Aesthetic Clinic <noreply@logicversion.ng>

**To:** your@email.com

**Subject:** Test Email - AestheticClinic EMR

**Body:**
```
Test Email from AestheticClinic EMR

This is a test email to verify SMTP configuration is working correctly.

If you received this email, your email system is configured properly.

Timestamp: 2024-06-22T10:30:45.1234567Z
```

---

### Expected Password Reset Email

**From:** VCP Aesthetic Clinic <noreply@logicversion.ng>

**To:** your@email.com

**Subject:** Password Reset Request

**Body:**
```
Dear [Your Name],

You have requested a password reset for your AestheticClinic EMR account.

Please click the link below to reset your password:
http://localhost:5001/login?reset=true&userNameOrEmail=...&token=...

This link is valid for 24 hours.

If you did not request this reset, please ignore this email.

Regards,
AestheticClinic EMR Team
```

---

## 🔧 Troubleshooting During Testing

### Problem: "Cannot GET /api/debug/check-smtp-config"

**Cause:** Application not running

**Solution:**
```
1. Open Visual Studio
2. Press F5 to start debugging
3. Wait for app to start
4. Try again
```

---

### Problem: "Connection timeout"

**Cause:** SMTP server not reachable

**Test:**
```powershell
Test-NetConnection mail.logicversion.ng -Port 8889
```

**Expected:** `TcpTestSucceeded : True`

**If False:**
- Check internet connection
- Check firewall settings
- Try different network

---

### Problem: "Authentication failed"

**Cause:** Wrong SMTP credentials

**Check:**
- Username: noreply@logicversion.ng
- Password: Khide@321!!
- Account is active
- Account allows SMTP access

---

### Problem: "Email not arriving"

**Steps:**
1. Wait 2-3 minutes (email is slow)
2. Check SPAM folder
3. Check email address is correct
4. Try different email address
5. Check SMTP server status

---

## ✨ Success Indicators

### ✅ Everything Working:
```
1. SMTP config returns OK           ✅
2. Test email sends successfully    ✅
3. Email arrives in inbox           ✅
4. Forgot password flow works       ✅
5. Breakpoints can be set           ✅
6. Can step through code            ✅
7. Logs show success messages       ✅
```

### ⚠️ Partially Working:
```
Configuration loads but:
- Email not arriving → Check credentials/SMTP server
- Breakpoints not hit → Check configuration loading
- Logs show errors → Check error messages
```

### ❌ Not Working:
```
- Config returns NULL → Check appsettings.json
- App won't start → Check for errors in Visual Studio
- Connection timeout → Check SMTP server access
```

---

## 📋 Testing Checklist

```
Pre-Testing:
  [ ] Visual Studio is open
  [ ] Application code is open
  [ ] appsettings.json has SmtpConfig

Starting:
  [ ] Press F5 to start debugging
  [ ] App loads successfully
  [ ] Console shows SMTP validation

Configuration Test:
  [ ] Navigate to check-smtp-config endpoint
  [ ] Response status is OK
  [ ] All fields populated

Email Test:
  [ ] Navigate to send-test-email endpoint
  [ ] Response status is SUCCESS
  [ ] Check email inbox
  [ ] Email received successfully

Breakpoint Test:
  [ ] Set breakpoint in EmailSender
  [ ] Trigger forgot password
  [ ] Breakpoint is hit
  [ ] Can inspect variables

Full Flow Test:
  [ ] Click Forgot Password on login
  [ ] Enter username/email
  [ ] Submit form
  [ ] Email arrives
  [ ] Can click reset link
  [ ] Password reset works

Final Check:
  [ ] All tests passed ✅
  [ ] Email system working ✅
  [ ] Ready for production ✅
```

---

**Ready to start testing? Go to STEP 1 above!** ✅
