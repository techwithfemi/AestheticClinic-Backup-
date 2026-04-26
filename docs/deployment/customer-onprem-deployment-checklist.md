# Customer On-Prem Deployment Checklist

Use this checklist during deployment at the customer site.

## 1. Capture Environment Details
- Customer server name: `____________________`
- IIS site name: `____________________`
- IIS physical path: `____________________`
- Public/Internal URL: `https://____________________`
- SQL Server host: `____________________`
- SQL Database name: `Hospital_Aesthetic_EMR`
- SQL login user: `____________________`
- Certificate subject/Common Name: `____________________`
- Certificate thumbprint: `____________________`

## 2. Server Prerequisites
- [ ] IIS installed
- [ ] ASP.NET Core .NET 10 Hosting Bundle installed
- [ ] SQL Server reachable from the web server
- [ ] HTTPS certificate installed in `Local Computer > Personal`
- [ ] Port `443` allowed through firewall
- [ ] DNS or host name resolves correctly

## 3. Application Files
Publish from:
- `AestheticEMR.Server`

Publish profile:
- `LocalIisRehearsal`

Publish output:
- `publish\AestheticEMR.Server`

Deploy these contents to the IIS site folder.

## 4. Production Configuration
Verify `appsettings.Production.json` before copying files.

Required values:
- `ConnectionStrings:DefaultConnection`
- `HttpsRedirection:Enabled = true`
- `OIDC:Certificates:Path` if using a file-based cert for signing/encryption
- `OIDC:Certificates:Password` if required

Current expected database setting format:

`Server=<sql-server>;Database=Hospital_Aesthetic_EMR;User ID=<user>;Password=<password>;TrustServerCertificate=true;MultipleActiveResultSets=true`

## 5. IIS Setup
- [ ] Create/use an application pool
- [ ] Set application pool to `No Managed Code`
- [ ] Create/use site pointing to deployed folder
- [ ] Add `https` binding on port `443`
- [ ] Assign the correct certificate
- [ ] Optional: add `http` binding on port `80`
- [ ] Set environment variable `ASPNETCORE_ENVIRONMENT=Production`

## 6. Deployment Steps
- [ ] Stop the IIS site or place `app_offline.htm` in the site root
- [ ] Copy `publish\AestheticEMR.Server\*` to the IIS physical path
- [ ] Remove `app_offline.htm` if used
- [ ] Start the IIS site
- [ ] Browse to the HTTPS URL

## 7. Validation
- [ ] Home page loads over HTTPS
- [ ] Login page loads over HTTPS
- [ ] User login succeeds
- [ ] Dashboard loads
- [ ] Database-backed pages load correctly
- [ ] No HTTP-to-HTTPS/auth transport errors in logs

## 8. Log Locations
Check application logs in:
- `Logs\log-*.log`

Check Windows/IIS if needed:
- Event Viewer
- IIS logs
- stdout logs if enabled for troubleshooting

## 9. Known Failure Checks
### If login fails immediately
- Verify the site URL is `https://...`
- Verify IIS HTTPS binding is correct
- Verify the latest published build was copied
- Verify `ASPNETCORE_ENVIRONMENT=Production`

### If the site cannot reach SQL Server
- Verify SQL Server service is running
- Verify server name resolves from the IIS server
- Verify SQL login exists and has DB access
- Verify firewall/network rules

### If the site works until reboot only
- The app was run manually with `dotnet AestheticEMR.Server.dll`
- Permanent hosting must be through IIS

## 10. Deployment Signoff
- Deployment date/time: `____________________`
- Deployed by: `____________________`
- Customer contact: `____________________`
- Outcome: `____________________`
- Notes: `____________________________________________________________`
