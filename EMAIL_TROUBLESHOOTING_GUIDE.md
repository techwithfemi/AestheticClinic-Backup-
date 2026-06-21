# How to Check Email Sending Logs

## Location of Log Files
Your application logs are stored in:
```
C:\Users\Administrator\source\repos\Medicals\AestheticClinic\AestheticEMR\AestheticEMR.Server\Logs\
```

Log files are named with the pattern: `log-YYYYMMDD.log` (e.g., `log-20260621.log`)

## Check for Email Errors

### Using PowerShell:

```powershell
# Check the most recent log file for email-related entries
Get-ChildItem -Path "C:\Users\Administrator\source\repos\Medicals\AestheticClinic\AestheticEMR\AestheticEMR.Server\Logs" -Filter "*.log" -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1 | ForEach-Object { Select-String -Path $_.FullName -Pattern "email|Email|SMTP" }
```

### Using Windows File Explorer:

1. Navigate to: `AestheticEMR\AestheticEMR.Server\Logs`
2. Open the most recent `log-YYYYMMDD.log` file with Notepad
3. Use `Ctrl+F` to search for:
   - "email" (case-insensitive)
   - "SMTP" 
   - "ERR"
   - "error"

## What to Look For

### Successful Email Send
You should see log entries like:
```
[INF] Attempting to send email to omagebi3@gmail.com with subject 'Password Reset Request' from noreply@logicversiononline.com
[INF] Connected to SMTP server mail5005.smarterasp.net:8889
[INF] Authenticated with SMTP server as noreply@logicversiononline.com
[INF] Email sent successfully to omagebi3@gmail.com
```

### Email Send Failed
You would see log entries like:
```
[ERR] An error occurred whilst sending email to omagebi3@gmail.com with subject 'Password Reset Request'
[ERR] [Exception details showing the specific error]
```

## Common Issues and What They Mean

| Error | Cause | Solution |
|-------|-------|----------|
| "Cannot connect to host" | SMTP server unreachable | Check internet connection, verify host/port in appsettings.json |
| "Authentication failed" | Invalid username/password | Verify credentials in appsettings.json |
| "Invalid operation: SSL/TLS is required" | SSL setting mismatch | Check UseSSL setting in appsettings.json (should be `false` for port 8889) |
| "Connection timeout" | Network/firewall blocking | May need firewall rules or VPN configuration |
| "No such recipient" | Invalid email address | Verify user has valid email in database |

## Testing Email Manually

If logs show successful sending but emails aren't received:

1. **Check Spam/Junk Folder** in your Gmail
2. **Add to Contacts** to whitelist the sender
3. **Check Gmail Settings**:
   - Settings → Forwarding and POP/IMAP → Enable IMAP (if using IMAP client)
   - Settings → Filters and Blocked Addresses (check if emails are filtered)

## Debugging Steps

### Step 1: Reproduce the Issue
1. Start the application
2. Go to Login → Forgot Password
3. Enter your test email
4. Click "Send reset link"

### Step 2: Immediately Check Logs
1. Look at the log file from today
2. Search for "email" or your test email address
3. Note any error messages

### Step 3: Check Email
1. Check Inbox for the email
2. Check Spam/Junk folder
3. Wait 30 seconds and refresh

### Step 4: Analysis
- If logs show success but email doesn't arrive → Contact email provider (mail5005.smarterasp.net)
- If logs show error → Check credentials and SMTP settings
- If logs show no email entries → Check if the user exists with valid email in database

## Next Steps

If the email feature is still not working after checking logs:

1. **Contact SmartAsp.net Support** - Your email hosting provider
   - Ask them to verify:
     - DKIM/SPF/DMARC records are configured
     - Account credentials are correct
     - There are no IP restrictions

2. **Consider Alternative Email Service**:
   - SendGrid
   - Mailgun
   - AWS SES
   - Office 365
   - Gmail (with app-specific passwords)

3. **Implement Queue-Based Email** - Use Hangfire to queue emails and retry on failure:
   - Prevents immediate blocking
   - Shows delivery status in admin panel
   - Automatic retry logic

4. **Add Test Email Endpoint** - Create an admin endpoint to test email:
   ```
   POST /api/admin/test-email?to=youremail@gmail.com
   ```
