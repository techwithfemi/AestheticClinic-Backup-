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

## Production — Secret variable names (all hosts)

ASP.NET Core reads environment variables and maps `__` (double underscore) to `:` for nested keys.
These are the variable names you set on every production host:

| Environment variable | Config key |
|---|---|
| `ASPNETCORE_ENVIRONMENT` | *(must be set to `Production`)* |
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

---

## Production Host 1 — SmarterASP.NET (Shared Hosting / IIS)

SmarterASP.NET is shared hosting — you have **no access to IIS Manager or the machine's
system environment**. The only way to pass environment variables is through `web.config`,
which is deployed alongside your app.

> ⚠️ `web.config` **must NOT be committed to git** because it will contain real passwords.
> A safe template (`web.config.template`) is committed instead.

### How it works

IIS passes `<environmentVariables>` entries directly to your ASP.NET Core process.
ASP.NET Core reads them exactly like OS environment variables.

### Steps

1. **After publishing**, open the generated `web.config` in the publish output folder.
2. Add your secrets inside the `<aspNetCore>` element as shown in `web.config.template`.
3. Upload the completed `web.config` to SmarterASP.NET via:
   - Their **File Manager** in the control panel, or
   - **FTP** (FileZilla etc.) into your site's root folder.
4. Restart the application pool from the SmarterASP.NET control panel.

### `web.config.template` (committed — blank values)

See `web.config.template` in the project root. Copy it to `web.config`, fill in real values,
then upload. **Never commit the filled-in `web.config`.**

---

## Production Host 2 — Customer Windows 11 Server (IIS)

On a machine you control you have two options. **Option A (web.config) is recommended**
because it is per-application and does not require a server restart.

### Option A — `web.config` environment variables (recommended)

Same mechanism as SmarterASP.NET above, but since you control IIS Manager you can
also edit `web.config` directly on the server.

1. Publish the app to a folder (e.g. `C:\inetpub\AestheticEMR`).
2. Copy `web.config.template` → `web.config` and fill in real values.
3. In IIS Manager, set the application pool identity to a service account with DB access.
4. Restart the site.

### Option B — IIS Manager GUI (Application → Configuration Editor)

1. Open **IIS Manager**.
2. Select your site → **Configuration Editor**.
3. Navigate to `system.webServer/aspNetCore`.
4. Open `environmentVariables` collection.
5. Add each variable from the table above.
6. Click **Apply** — no server restart needed.

### Option C — Windows Machine-level Environment Variables

Sets the variable for the **entire machine** (all apps). Use only if you have a
dedicated server running only this app.

```powershell
# Run as Administrator — persists across reboots
[System.Environment]::SetEnvironmentVariable(
    "ConnectionStrings__DefaultConnection",
    "Server=Logic;Database=Hospital;User ID=smart;Password=REAL_PASSWORD;TrustServerCertificate=true;MultipleActiveResultSets=true",
    "Machine"
)

[System.Environment]::SetEnvironmentVariable("SmtpConfig__Password",    "REAL_PASSWORD", "Machine")
[System.Environment]::SetEnvironmentVariable("RabbitMQ__Password",       "REAL_PASSWORD", "Machine")
[System.Environment]::SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT",   "Production",    "Machine")
```

> After setting machine-level variables you **must restart the IIS application pool**
> (or `iisreset`) for the new values to be picked up.

```powershell
# Restart just the app pool (safer than iisreset)
Import-Module WebAdministration
Restart-WebAppPool -Name "AestheticEMR"
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

1. **Environment variables** — from `web.config`, IIS, OS, or Azure App Settings
2. **dotnet user-secrets** — Development only (outside repo)
3. **appsettings.Production.json** — Prod non-secret settings
4. **appsettings.Development.json** — Dev non-secret settings
5. **appsettings.json** — Baseline (no secrets, always committed)
