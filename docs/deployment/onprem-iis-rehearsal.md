# On-Prem IIS Deployment Rehearsal

## Prerequisites
- Windows Server or Windows machine with IIS installed
- ASP.NET Core .NET 10 Hosting Bundle installed
- SQL Server reachable from the server
- Server certificate installed in `Local Computer > Personal`
- IIS site binding for `https` on port `443`

## Publish
From Visual Studio, publish `AestheticEMR.Server` using the `LocalIisRehearsal` publish profile.

Published output goes to:
- `publish/AestheticEMR.Server`

## Server Configuration
Use `AestheticEMR.Server/appsettings.Production.json` for server deployment.

Current production settings assume:
- SQL Server host: `Logic`
- Database: `Hospital_Aesthetic_EMR`
- HTTPS redirection enabled

Before deployment, verify these values on the target server:
- `ConnectionStrings:DefaultConnection`
- `OIDC:Certificates:Path`
- `OIDC:Certificates:Password`

If IIS terminates TLS, the app still requires HTTPS and relies on forwarded headers.

## IIS Setup
1. Create or use an application pool with `No Managed Code`.
2. Create a site or application pointing to the published folder.
3. Add bindings:
   - `http` on `80` optional
   - `https` on `443` required
4. Assign the correct certificate to the `https` binding.
5. Set environment variable for the site or app pool:
   - `ASPNETCORE_ENVIRONMENT=Production`

## Deploy
1. Stop the IIS site or place `app_offline.htm` in the site root.
2. Copy contents of `publish/AestheticEMR.Server` into the IIS site folder.
3. Start the site again.
4. Browse to the HTTPS URL.

## Verification
After deployment, verify:
- `https://server-name/` loads
- login succeeds
- SQL connection works
- `Logs/log-*.log` contains no transport security or SQL connection errors

## Common Failure Checks
### Login fails over HTTP
Use the HTTPS site URL only. OpenIddict rejects insecure token requests.

### Site works until reboot
This means the app was started manually with `dotnet AestheticEMR.Server.dll` instead of being hosted by IIS.

### SQL connection fails
Verify:
- SQL Server service is running
- server name `Logic` resolves on the target machine
- SQL login exists and has access to `Hospital_Aesthetic_EMR`
- firewall allows SQL traffic if SQL is remote

### HTTPS loads in IIS but auth still says HTTP
Verify:
- IIS is forwarding `X-Forwarded-Proto`
- app is running with the latest published build
- `ASPNETCORE_ENVIRONMENT=Production`

## Rehearsal Note
For rehearsal, `https://localhost:7085` is only valid while the published app is started manually. Permanent hosting after reboot should use IIS, not a manually started `dotnet` process.
