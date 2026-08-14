# CrystalReportWebAPI Secrets

This project uses the `UseWebConfigForDefaultConnection` setting in `Web.config` to decide how `DefaultConnection` is resolved.

## Resolution modes

### 1. `UseWebConfigForDefaultConnection=true`

`DefaultConnection` is read from `Web.config`:

```xml
<appSettings>
  <add key="UseWebConfigForDefaultConnection" value="true" />
</appSettings>

<connectionStrings>
  <add name="DefaultConnection" connectionString="Server=YOUR_SERVER;Initial Catalog=hospital;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=true;MultipleActiveResultSets=true;" providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 2. `UseWebConfigForDefaultConnection=false`

`DefaultConnection` is resolved in this order:

1. Environment variable: `ConnectionStrings__DefaultConnection`
2. User secrets file in `%APPDATA%\Microsoft\UserSecrets\CrystalReportWebAPI-DefaultConnection\secrets.json`
3. `Web.config` `DefaultConnection` as a final fallback

## Development

Set this in `Web.config`:

```xml
<add key="UseWebConfigForDefaultConnection" value="false" />
```

Create this file:

`%APPDATA%\Microsoft\UserSecrets\CrystalReportWebAPI-DefaultConnection\secrets.json`

Example content:

```json
{
  "ConnectionStrings:DefaultConnection": "Server=YOUR_SERVER;Initial Catalog=hospital;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=true;MultipleActiveResultSets=true;"
}
```

This shape also works:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Initial Catalog=hospital;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=true;MultipleActiveResultSets=true;"
  }
}
```

## Production

Set this in `Web.config`:

```xml
<add key="UseWebConfigForDefaultConnection" value="false" />
```

Set this environment variable on the host:

- `ConnectionStrings__DefaultConnection`

Example value:

`Server=YOUR_SERVER;Initial Catalog=hospital;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=true;MultipleActiveResultSets=true;`

## Notes

- Serilog uses `DefaultConnection` for writing to the `Logs` table.
- Report endpoints that receive `xDbConnection=DefaultConnection` or header `X-Db-Connection: DefaultConnection` use the same resolver.
- If `UseWebConfigForDefaultConnection=false` and external values are missing, `Web.config` `DefaultConnection` is used as a final fallback.
- If no `DefaultConnection` is available anywhere, SQL logging is disabled and connection-id-based report resolution will fail.