# 🎯 LIVE TESTING - Ready to Use Code

Copy & paste these commands directly into your terminal to test!

---

## Test 1: Check SMTP Configuration (Easiest)

### PowerShell (Recommended for Windows)

```powershell
# Copy and paste this entire block into PowerShell

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Testing SMTP Configuration..." -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

$url = "http://localhost:5001/api/debug/check-smtp-config"

try {
    Write-Host "Sending request to: $url" -ForegroundColor Yellow
    $response = Invoke-RestMethod -Uri $url -Method Get

    Write-Host "`n✅ SUCCESS - Configuration Loaded!" -ForegroundColor Green
    Write-Host "`nResponse:" -ForegroundColor Green
    $response | ConvertTo-Json | Write-Host

    Write-Host "`n✅ SMTP is ready!" -ForegroundColor Green
}
catch {
    Write-Host "`n❌ ERROR:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}
```

### Browser (Simplest)

1. Start your application (F5)
2. Open browser and go to:
   ```
   http://localhost:5001/api/debug/check-smtp-config
   ```
3. You should see JSON response

---

## Test 2: Send Test Email

### PowerShell (Recommended for Windows)

```powershell
# Change this email to yours
$testEmail = "your.email@gmail.com"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Sending Test Email..." -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

$url = "http://localhost:5001/api/debug/send-test-email?testEmail=$testEmail"

try {
    Write-Host "Sending test email to: $testEmail" -ForegroundColor Yellow
    Write-Host "Endpoint: $url" -ForegroundColor Gray
    Write-Host ""

    $response = Invoke-RestMethod -Uri $url -Method Post

    if ($response.status -eq "SUCCESS") {
        Write-Host "✅ Email sent successfully!" -ForegroundColor Green
        Write-Host "Status: $($response.status)" -ForegroundColor Green
        Write-Host "Message: $($response.message)" -ForegroundColor Green
        Write-Host "Sent to: $($response.email)" -ForegroundColor Green
        Write-Host "`n📧 Check your email inbox (including spam folder)!" -ForegroundColor Yellow
    }
    else {
        Write-Host "❌ Email sending failed" -ForegroundColor Red
        $response | ConvertTo-Json | Write-Host
    }
}
catch {
    Write-Host "`n❌ ERROR:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}
```

### Browser + Developer Console (Alternative)

1. Start your application (F5)
2. Open browser and go to: `http://localhost:5001`
3. Press F12 to open Developer Tools
4. Click on "Console" tab
5. Copy and paste this code:

```javascript
// Change this to your email
const email = 'your.email@gmail.com';

fetch(`http://localhost:5001/api/debug/send-test-email?testEmail=${email}`, {
  method: 'POST'
})
.then(response => response.json())
.then(data => {
  console.log('%c✅ Response:', 'color: green; font-size: 14px; font-weight: bold;');
  console.table(data);

  if (data.status === 'SUCCESS') {
    console.log('%c✅ Email sent successfully!', 'color: green; font-size: 12px;');
    console.log(`%c📧 Check your inbox: ${data.email}`, 'color: blue; font-size: 12px;');
  } else {
    console.log('%c❌ Failed to send email', 'color: red; font-size: 12px;');
  }
})
.catch(error => {
  console.error('%c❌ Error:', 'color: red; font-size: 14px;', error);
});
```

---

## Test 3: Complete Email Testing Flow

### PowerShell Script (All-in-One)

```powershell
# ============================================
# Complete Email Testing Script
# ============================================
# Change this to your email address
$testEmail = "your.email@gmail.com"

Write-Host ""
Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║   Email Sending - Complete Test       ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# Test 1: Check Configuration
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray
Write-Host "TEST 1: Checking SMTP Configuration" -ForegroundColor Yellow
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray

try {
    $configUrl = "http://localhost:5001/api/debug/check-smtp-config"
    $config = Invoke-RestMethod -Uri $configUrl -Method Get

    if ($config.status -eq "OK") {
        Write-Host "✅ SMTP Configuration Verified!" -ForegroundColor Green
        Write-Host "   Host: $($config.config.host)" -ForegroundColor Green
        Write-Host "   Port: $($config.config.port)" -ForegroundColor Green
        Write-Host "   Email: $($config.config.emailAddress)" -ForegroundColor Green
    }
    else {
        Write-Host "❌ Configuration Error: $($config.message)" -ForegroundColor Red
        exit 1
    }
}
catch {
    Write-Host "❌ Cannot reach application at http://localhost:5001" -ForegroundColor Red
    Write-Host "   Make sure the app is running (F5)" -ForegroundColor Yellow
    exit 1
}

Write-Host ""

# Test 2: Send Test Email
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray
Write-Host "TEST 2: Sending Test Email" -ForegroundColor Yellow
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray
Write-Host "Sending to: $testEmail" -ForegroundColor Cyan

try {
    $emailUrl = "http://localhost:5001/api/debug/send-test-email?testEmail=$testEmail"
    $emailResponse = Invoke-RestMethod -Uri $emailUrl -Method Post

    if ($emailResponse.status -eq "SUCCESS") {
        Write-Host "✅ Test Email Sent Successfully!" -ForegroundColor Green
        Write-Host "   Status: $($emailResponse.status)" -ForegroundColor Green
        Write-Host "   Message: $($emailResponse.message)" -ForegroundColor Green
        Write-Host "   Email: $($emailResponse.email)" -ForegroundColor Green
    }
    else {
        Write-Host "❌ Email Sending Failed: $($emailResponse.message)" -ForegroundColor Red
        exit 1
    }
}
catch {
    Write-Host "❌ Error sending test email:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}

Write-Host ""

# Test 3: Summary
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray
Write-Host "✅ ALL TESTS PASSED!" -ForegroundColor Green
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray
Write-Host ""
Write-Host "📧 Check your email inbox for the test email!" -ForegroundColor Cyan
Write-Host "   From: noreply@logicversion.ng" -ForegroundColor Cyan
Write-Host "   To: $testEmail" -ForegroundColor Cyan
Write-Host "   Subject: Test Email - AestheticClinic EMR" -ForegroundColor Cyan
Write-Host ""
Write-Host "Email sending is working correctly! ✅" -ForegroundColor Green
Write-Host ""
```

---

## Test 4: Monitor Application Startup

### PowerShell (Watch Startup Logs)

```powershell
# This script starts the app and shows if SMTP validates

Write-Host "Starting Application..." -ForegroundColor Cyan
Write-Host "Watching for SMTP Configuration validation..." -ForegroundColor Yellow
Write-Host ""

# Note: Run this AFTER starting the app in Visual Studio (F5)
# Then just watch the output for the SMTP validation message

# Or run this to build and see the output
dotnet build AestheticEMR/AestheticEMR.Server/AestheticEMR.Server.csproj

Write-Host ""
Write-Host "Look for this message in the output:" -ForegroundColor Green
Write-Host "✅ SMTP Configuration validated successfully:" -ForegroundColor Green
```

---

## Test 5: Test Forgot Password Flow

### Step-by-Step (Manual)

1. **Start the application:**
   ```
   Press F5 in Visual Studio
   ```

2. **Open login page:**
   ```
   http://localhost:5001
   ```

3. **Click "Forgot Password?" link**

4. **Enter test username:**
   ```
   admin
   ```

5. **Click "Send" button**

6. **Check the response:**
   - Should get HTTP 204 (success)
   - No error message

7. **Check your email:**
   - Look for email from: noreply@logicversion.ng
   - Subject: "Password Reset Request"
   - Click the reset link

---

## Test 6: Monitor Logs in Real-Time

### PowerShell (Tail Log File)

```powershell
# Find and watch the log file in real-time

Write-Host "Looking for log files..." -ForegroundColor Yellow

$logDir = "C:\Users\Administrator\source\repos\Medicals\AestheticClinic\Logs"

if (Test-Path $logDir) {
    $latestLog = Get-ChildItem $logDir -Filter "log-*.log" | 
                 Sort-Object LastWriteTime -Descending | 
                 Select-Object -First 1

    if ($latestLog) {
        Write-Host "Found log file: $($latestLog.Name)" -ForegroundColor Green
        Write-Host "Watching for email messages..." -ForegroundColor Yellow
        Write-Host ""

        # Watch the log file for email-related messages
        Get-Content $latestLog.FullName -Wait | 
            Where-Object { $_ -match "email|SMTP|Password Reset" } | 
            ForEach-Object {
                Write-Host $_ -ForegroundColor Green
            }
    }
    else {
        Write-Host "No log files found yet. Run the application first." -ForegroundColor Yellow
    }
}
else {
    Write-Host "Log directory not found at: $logDir" -ForegroundColor Red
}
```

---

## Quick Troubleshooting Commands

### Check if Application is Running

```powershell
# Test if the application is responding
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5001/api/debug/check-smtp-config" -UseBasicParsing
    Write-Host "✅ Application is running!" -ForegroundColor Green
}
catch {
    Write-Host "❌ Application is NOT running" -ForegroundColor Red
    Write-Host "Start it with F5 in Visual Studio" -ForegroundColor Yellow
}
```

### Check SMTP Connection

```powershell
# Test if we can reach the SMTP server
Write-Host "Testing SMTP server connectivity..." -ForegroundColor Yellow

$smtpServer = "mail.logicversion.ng"
$smtpPort = 8889

try {
    $socket = New-Object System.Net.Sockets.TcpClient
    $socket.Connect($smtpServer, $smtpPort)

    if ($socket.Connected) {
        Write-Host "✅ Can reach SMTP server!" -ForegroundColor Green
        Write-Host "   Server: $smtpServer" -ForegroundColor Green
        Write-Host "   Port: $smtpPort" -ForegroundColor Green
    }

    $socket.Close()
}
catch {
    Write-Host "❌ Cannot reach SMTP server" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
}
```

---

## Summary Commands

| Test | Command | Time |
|------|---------|------|
| **Check Config** | `Invoke-RestMethod "http://localhost:5001/api/debug/check-smtp-config"` | 5 sec |
| **Send Email** | See Test 2 above | 10 sec |
| **Full Test** | See Test 3 above | 1 min |
| **Forgot Password** | Go to http://localhost:5001 and click link | 2 min |

---

## How to Use These Scripts

### Copy & Paste into PowerShell

1. **Open PowerShell**
   - Windows Key → type "PowerShell" → press Enter

2. **Copy one of the scripts above**
   - Select the code block
   - Ctrl+C to copy

3. **Paste into PowerShell**
   - Right-click in PowerShell window
   - Or Ctrl+V

4. **Press Enter**
   - Script runs
   - Watch for results

---

## Expected Results

### Test 1: Configuration Check
```
Status: OK
All fields populated ✅
```

### Test 2: Send Test Email
```
Status: SUCCESS
Email arrived ✅
```

### Test 3: Full Test
```
Configuration: OK ✅
Email Sent: SUCCESS ✅
Overall: WORKING ✅
```

### Test 4: Forgot Password
```
Email received ✅
Can reset password ✅
```

---

## Troubleshooting

### "Cannot reach application"
```powershell
# Make sure app is running
# Press F5 in Visual Studio
# Then try again
```

### "SMTP Configuration NULL"
```powershell
# Check appsettings.json
# Make sure SmtpConfig section exists
# Verify all fields are populated
```

### "Connection timeout"
```powershell
# Check SMTP server is online
# Verify firewall allows port 8889
# Try: Test-NetConnection mail.logicversion.ng -Port 8889
```

### "Email not arriving"
```powershell
# Check spam folder
# Verify email address is correct
# Check SMTP credentials
```

---

**Ready to Test? Start with Test 1!** ✅
