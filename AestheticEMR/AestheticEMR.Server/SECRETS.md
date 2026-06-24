# Secrets & Sensitive Configuration

`appsettings.json`, `appsettings.Development.json`, and `appsettings.Production.json`
contain **no secrets** — all sensitive values are empty strings, safe to commit.
Real values are supplied at runtime by the environment-specific store below.
No code changes are needed — ASP.NET Core's `IConfiguration` layers these automatically.

---

## Development — dotnet user-secrets

Secrets are stored **outside the repository** in your OS profile:

| OS | Location |
|----|----------|
| Windows | `%APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json` |
| Linux / macOS | `~/.microsoft/usersecrets/<UserSecretsId>/secrets.json` |

They are loaded automatically when `ASPNETCORE_ENVIRONMENT=Development`.

### Managing secrets locally

```powershell
# View all secrets
dotnet user-secrets list --project AestheticEMR.Server

# Set / update a secret
dotnet user-secrets set "SmtpConfig:Password" "your-password" --project AestheticEMR.Server

# Remove a secret
dotnet user-secrets remove "SmtpConfig:Password" --project AestheticEMR.Server
```

### Secret keys stored in user-secrets (Development)

| Key | Description |
|-----|-------------|
| `ConnectionStrings:DefaultConnection` | Full Hospital DB connection string (inc. password) |
| `ConnectionStrings:SmartHRConnection` | Full SmartHR DB connection string (inc. password) |
| `ConnectionStrings:AccountingConnection` | Full Accounting DB connection string (inc. password) |
| `RabbitMQ:Password` | RabbitMQ broker password |
| `SmtpConfig:Password` | SMTP mail server password |
| `SmsConfig:AccountSid` | Twilio SMS Account SID |
| `SmsConfig:AuthToken` | Twilio SMS auth token |
| `SmsConfig:FromPhoneNumber` | Twilio SMS from number |
| `WhatsAppConfig:AccountSid` | Twilio WhatsApp Account SID |
| `WhatsAppConfig:AuthToken` | Twilio WhatsApp auth token |
| `WhatsAppConfig:FromPhoneNumber` | Twilio WhatsApp from number |

---

## Production & Staging — OS Environment Variables

Set these on the host (server, container, CI runner).
ASP.NET Core maps `__` (double underscore) to `:` for nested keys.

| Environment variable | Config key |
|---|---|
| `ConnectionStrings__DefaultConnection` | `ConnectionStrings:DefaultConnection` |
| `ConnectionStrings__SmartHRConnection` | `ConnectionStrings:SmartHRConnection` |
| `ConnectionStrings__AccountingConnection` | `ConnectionStrings:AccountingConnection` |
| `RabbitMQ__Password` | `RabbitMQ:Password` |
| `SmtpConfig__Password` | `SmtpConfig:Password` |
| `SmsConfig__AccountSid` | `SmsConfig:AccountSid` |
| `SmsConfig__AuthToken` | `SmsConfig:AuthToken` |
| `SmsConfig__FromPhoneNumber` | `SmsConfig:FromPhoneNumber` |
| `WhatsAppConfig__AccountSid` | `WhatsAppConfig:AccountSid` |
| `WhatsAppConfig__AuthToken` | `WhatsAppConfig:AuthToken` |
| `WhatsAppConfig__FromPhoneNumber` | `WhatsAppConfig:FromPhoneNumber` |
| `OIDC__Certificates__Password` | `OIDC:Certificates:Password` |

### Windows Server (PowerShell)
```powershell
[System.Environment]::SetEnvironmentVariable("SmtpConfig__Password", "your-password", "Machine")
```

### Linux / Docker
```bash
export SmtpConfig__Password="your-password"
```

### Docker Compose
```yaml
environment:
  - SmtpConfig__Password=your-password
  - ConnectionStrings__DefaultConnection=Server=...;Password=...
```

---

## Azure — App Service Application Settings

Add each key from the table above in:

> **Azure Portal → App Service → Configuration → Application settings**

Use `__` (double underscore) as the separator exactly as shown above.

For higher security, store secrets in **Azure Key Vault** and reference them:
```
@Microsoft.KeyVault(SecretUri=https://your-vault.vault.azure.net/secrets/SmtpPassword/)
```

---

## Configuration priority order (highest wins)

1. **Environment variables** — Production / Staging / Azure
2. **dotnet user-secrets** — Development only (outside repo)
3. **appsettings.Development.json** — Dev non-secret settings
4. **appsettings.Production.json** — Prod non-secret settings
5. **appsettings.json** — Baseline (no secrets, always committed)
