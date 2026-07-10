// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Account;
using AestheticEMR.Core.Services;
using AestheticEMR.Core.Services.Account;
using AestheticEMR.Core.Services.Accounting;
using AestheticEMR.Core.Services.Accounting.Interfaces;
using AestheticEMR.Core.Services.Aesthetics;
using AestheticEMR.Core.Services.Dental;
using AestheticEMR.Core.Services.Dental.Interfaces;
using AestheticEMR.Core.Services.Employees;
using AestheticEMR.Core.Services.Employees.Interfaces;
using AestheticEMR.Core.Services.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Core.Services.Shop;
using DataAccess.DbAccess;
using DataAccess.Services;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.Authorization.Requirements;
using AestheticEMR.Server.Configuration;
using AestheticEMR.Server.Services;
using AestheticEMR.Server.Services.Email;
using AestheticEMR.Server.Services.Logging;
using AestheticEMR.Server.Services.Sms;
using AestheticEMR.Server.Services.WhatsApp;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi;
using OpenIddict.Validation.AspNetCore;
using Quartz;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using static OpenIddict.Abstractions.OpenIddictConstants;
using AestheticEMR.Core.Services.Legacy.Messaging;
using AestheticEMR.Core.Services.Legacy.Messaging.Consumers;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Claims;
using AestheticEMR.Server.Services.Reporting;

var builder = WebApplication.CreateBuilder(args);
var enableHttpsRedirection = builder.Configuration.GetValue("HttpsRedirection:Enabled", true);
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var columnOptions = new ColumnOptions
{
    AdditionalColumns =
    [
        new SqlColumn { ColumnName = "UserId", PropertyName = "UserId", DataType = SqlDbType.NVarChar, DataLength = 128, AllowNull = true },
        new SqlColumn { ColumnName = "RequestPath", PropertyName = "RequestPath", DataType = SqlDbType.NVarChar, DataLength = 512, AllowNull = true },
        new SqlColumn { ColumnName = "Clinic", PropertyName = "Clinic", DataType = SqlDbType.NVarChar, DataLength = 128, AllowNull = true }
    ]
};

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.MSSqlServer(
            connectionString: defaultConnectionString,
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = true
            },
            columnOptions: columnOptions,
            restrictedToMinimumLevel: LogEventLevel.Information);

    if (context.HostingEnvironment.IsDevelopment())
    {
        loggerConfiguration.WriteTo.Console();
    }
});


/************* ADD SERVICES *************/

var connectionString = defaultConnectionString;

var accountingConnectionString = builder.Configuration.GetConnectionString("AccountingConnection") ??
                throw new InvalidOperationException("Connection string 'AccountingConnection' not found.");

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly));
    options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
    options.UseOpenIddict();
});

builder.Services.AddDbContext<AccountingDbContext>(options =>
{
    options.UseSqlServer(accountingConnectionString);
});

// Employees module uses the Hospital DB (ApplicationDbContext / DefaultConnection)
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// Cross-DB Dapper channel. Pass "Default" or "AccountingConnection" as connectionId.
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped(typeof(IServicesData<>), typeof(ServicesData<>));

// Accounting module - journal entries (AccountingConnection via Dapper)
builder.Services.AddScoped<IJournalEntryService, JournalEntryService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IIncomeService, IncomeService>();
builder.Services.AddScoped<IChartOfAccountService, ChartOfAccountService>();
builder.Services.AddHttpClient(nameof(LegacyCrystalReportProxyService), (sp, client) =>
{
    var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<AppSettings>>().Value;
    var timeoutSeconds = settings.LegacyReportService?.TimeoutSeconds ?? 120;
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds <= 0 ? 120 : timeoutSeconds);
});
builder.Services.AddScoped<ILegacyCrystalReportProxyService, LegacyCrystalReportProxyService>();
// Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure Identity options and password complexity here
builder.Services.Configure<IdentityOptions>(options =>
{
    // User settings
    options.User.RequireUniqueEmail = true;

    // Password settings
    /*
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 10;
    */

    // Configure Identity to use the same JWT claims as OpenIddict
    options.ClaimsIdentity.UserNameClaimType = Claims.Name;
    options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
    options.ClaimsIdentity.RoleClaimType = Claims.Role;
    options.ClaimsIdentity.EmailClaimType = Claims.Email;
});

// Configure OpenIddict periodic pruning of orphaned authorizations/tokens from the database.
builder.Services.AddQuartz(options =>
{
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});

// Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<ApplicationDbContext>();

        options.UseQuartz();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("connect/token");

        options.AllowPasswordFlow()
               .AllowRefreshTokenFlow();

        options.RegisterScopes(
            Scopes.Profile,
            Scopes.Email,
            Scopes.Address,
            Scopes.Phone,
            Scopes.Roles);

        if (builder.Environment.IsDevelopment())
        {
            options.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate();
        }
        else
        {
            var oidcCertFileName = builder.Configuration["OIDC:Certificates:Path"];
            var oidcCertFilePassword = builder.Configuration["OIDC:Certificates:Password"];

            if (string.IsNullOrWhiteSpace(oidcCertFileName))
            {
                // You must configure persisted keys for Encryption and Signing.
                // See https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
                options.AddEphemeralEncryptionKey()
                       .AddEphemeralSigningKey();
            }
            else
            {
                var oidcCertificate = X509CertificateLoader.LoadPkcs12FromFile(oidcCertFileName, oidcCertFilePassword);

                options.AddEncryptionCertificate(oidcCertificate)
                       .AddSigningCertificate(oidcCertificate);
            }
        }

        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough()
               .DisableTransportSecurityRequirement();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthPolicies.ViewAllUsersPolicy,
        policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ViewUsers))
    .AddPolicy(AuthPolicies.ManageAllUsersPolicy,
        policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ManageUsers))
    .AddPolicy(AuthPolicies.ViewAllRolesPolicy,
        policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ViewRoles))
    .AddPolicy(AuthPolicies.ViewRoleByRoleNamePolicy,
        policy => policy.Requirements.Add(new ViewRoleAuthorizationRequirement()))
    .AddPolicy(AuthPolicies.ManageAllRolesPolicy,
        policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ManageRoles))
    .AddPolicy(AuthPolicies.AssignAllowedRolesPolicy,
        policy => policy.Requirements.Add(new AssignRolesAuthorizationRequirement()))
    .AddPolicy(AuthPolicies.ViewAuditLogsPolicy, policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ViewAuditLogs)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageUsers)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageRoles)))
    .AddPolicy(AuthPolicies.ViewAccountingPolicy, policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ViewAccounting)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageAccounting)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageUsers)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageRoles)
            || context.User.IsInRole("Admin")
            || context.User.IsInRole("administrator")
            || context.User.IsInRole("Accounting")))
    .AddPolicy(AuthPolicies.ManageAccountingPolicy, policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageAccounting)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageUsers)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageRoles)
            || context.User.IsInRole("Admin")
            || context.User.IsInRole("administrator")
            || context.User.IsInRole("Accounting")))
    .AddPolicy(AuthPolicies.ViewEmployeesPolicy, policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ViewEmployees)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageEmployees)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageUsers)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageRoles)
            || context.User.IsInRole("Admin")
            || context.User.IsInRole("administrator")
            || context.User.IsInRole("Employees")))
    .AddPolicy(AuthPolicies.ManageEmployeesPolicy, policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageEmployees)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageUsers)
            || context.User.HasClaim(CustomClaims.Permission, ApplicationPermissions.ManageRoles)
            || context.User.IsInRole("Admin")
            || context.User.IsInRole("administrator")
            || context.User.IsInRole("Employees")));

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = OidcServerConfig.ServerName, Version = "v1" });
    c.OperationFilter<SwaggerAuthorizeOperationFilter>();
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("/connect/token", UriKind.Relative)
            }
        }
    });
});


/************* Configurations *************/
// Add configuration for mapping between the database and the application
builder.Services.AddAutoMapper(options =>
{
    options.AddProfile(new MappingProfile());
}, typeof(Program).Assembly);

// Configurations
builder.Services.Configure<AppSettings>(builder.Configuration);

/************* Business Services *************/
// Add your business services here
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IAestheticService, AestheticService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IDentalService, DentalService>();
builder.Services.AddScoped<IHRetainershipService, HRetainershipService>();
builder.Services.AddScoped<IHPatientService, HPatientService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IServiceTariffService, ServiceTariffService>();
builder.Services.AddScoped<IAccountingReportLookupService, AccountingReportLookupService>();
builder.Services.AddSingleton<IEmrAppDefaultsService, EmrAppDefaultsService>();
builder.Services.AddSingleton<EmrAppDefaultsStartupService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<EmrAppDefaultsStartupService>());

builder.Services.AddSingleton<IBillingCrossDatabaseSyncStrategyProvider, BillingCrossDatabaseSyncStrategyProvider>();

var enableMessageBus = builder.Configuration.GetValue("BillingSync:EnableMessageBus", false);

// Register both sync implementations; the factory chooses at runtime based on startup strategy
builder.Services.AddScoped<SqlServerSameInstanceBillingCrossDatabaseSyncService>();

if (enableMessageBus)
{
    builder.Services.AddScoped<MassTransitBillingCrossDatabaseSyncService>();
    builder.Services.AddScoped<BillingEventPublisher>();
}

builder.Services.AddScoped<IBillingCrossDatabaseSyncService>(sp =>
{
    var provider = sp.GetRequiredService<IBillingCrossDatabaseSyncStrategyProvider>();
    return provider.CurrentStatus.EffectiveMode == "MessageBusEventualSync" && enableMessageBus
        ? sp.GetRequiredService<MassTransitBillingCrossDatabaseSyncService>()
        : sp.GetRequiredService<SqlServerSameInstanceBillingCrossDatabaseSyncService>();
});

builder.Services.AddScoped<IBillingService, BillingService>();

// Receipt -> Accounting posting (same-instance), uses the InsertTranxaction sproc like invoices
builder.Services.AddScoped<IReceiptAccountingPostingService, ReceiptAccountingPostingService>();

// Inventory -> Accounting posting (same-instance), posts COGS/Inventory transactions for product usage
builder.Services.AddScoped<IInventoryAccountingService, InventoryAccountingService>();

if (enableMessageBus)
{
    // MassTransit ΓÇô only registered when message bus sync is enabled
    var rabbitMqHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
    var rabbitMqUser = builder.Configuration["RabbitMQ:Username"] ?? "guest";
    var rabbitMqPass = builder.Configuration["RabbitMQ:Password"] ?? "guest";

    builder.Services.AddMassTransit(x =>
    {
        // EF Core Outbox: messages are persisted atomically with the billing save
        x.AddEntityFrameworkOutbox<ApplicationDbContext>(o =>
        {
            o.UseSqlServer();
            o.UseBusOutbox();
        });

        x.AddConsumer<AccountingBillingUpsertedConsumer>();
        x.AddConsumer<AccountingBillingDeletedConsumer>();

        x.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(rabbitMqHost, h =>
            {
                h.Username(rabbitMqUser);
                h.Password(rabbitMqPass);
            });

            cfg.ConfigureEndpoints(ctx);
        });
    });
}

// Other Services
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IWhatsAppSender, WhatsAppSender>();
builder.Services.AddScoped<ISmsSender, SmsSender>();
builder.Services.AddScoped<ISmsTemplateService, SmsTemplateService>();
builder.Services.AddScoped<IUserIdAccessor, UserIdAccessor>();

// SMS/Birthday Services
builder.Services.AddHostedService<BirthdaySmsHostedService>();

// SMTP Configuration Validation
builder.Services.AddHostedService<SmtpConfigValidationService>();

// Serilog SQL logs retention cleanup
builder.Services.AddHostedService<SerilogLogRetentionHostedService>();

// Auth Handlers
builder.Services.AddSingleton<IAuthorizationHandler, ViewUserAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ManageUserAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ViewRoleAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AssignRolesAuthorizationHandler>();

// DB Creation and Seeding
builder.Services.AddTransient<IDatabaseSeeder, DatabaseSeeder>();

//Email Templates
EmailTemplates.Initialize(builder.Environment);

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? httpContext.User.FindFirst("sub")?.Value;

        var clinic = httpContext.User.FindFirst("clinic")?.Value
            ?? httpContext.User.FindFirst("Clinic")?.Value;

        diagnosticContext.Set("UserId", userId);
        diagnosticContext.Set("RequestPath", httpContext.Request.Path.Value);
        diagnosticContext.Set("Clinic", clinic);
    };
});

/************* CONFIGURE REQUEST PIPELINE *************/

app.UseForwardedHeaders();

app.UseDefaultFiles();
app.MapStaticAssets();
app.UseStaticFiles();
//app.MapFallbackToFile("index.html"); // in line 491

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Swagger UI - AestheticEMR";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{OidcServerConfig.ServerName} V1");
        c.OAuthClientId(OidcServerConfig.SwaggerClientID);
    });

    IdentityModelEventSource.ShowPII = true;
}
else
{
    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (enableHttpsRedirection)
{
    app.UseHttpsRedirection();
}

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

/************* STRATEGY INITIALIZATION *************/

var strategyProvider = app.Services.GetRequiredService<IBillingCrossDatabaseSyncStrategyProvider>();
await strategyProvider.InitializeAsync();


// Switch sync service implementation based on startup-resolved topology
var effectiveMode = strategyProvider.CurrentStatus.EffectiveMode;

if (effectiveMode == "MessageBusEventualSync")
{
    // Separate-machines: MassTransit Outbox + consumers
    // Re-create a fresh scope to register scoped services post-build is not possible;
    // instead, the conditional registration is deferred to a keyed factory.
    // MassTransit is registered only once at startup.
    // The scoped factory below resolves the correct implementation.
}

/************* SEED DATABASE *************/

var enableDatabaseMigrations = app.Configuration.GetValue("DatabaseMigrations:Enabled", true);
var enableDatabaseSeeding = app.Configuration.GetValue("DatabaseSeeding:Enabled", true);

using var scope = app.Services.CreateScope();
try
{
    if (enableDatabaseMigrations)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }
    else
    {
        app.Logger.LogInformation("Database migrations are disabled by configuration.");
    }

    if (enableDatabaseSeeding)
    {
        var dbSeeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
        await dbSeeder.SeedAsync();

        await OidcServerConfig.RegisterClientApplicationsAsync(scope.ServiceProvider);
    }
    else
    {
        app.Logger.LogInformation("Database seeding is disabled by configuration.");
    }
}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "An error occurred whilst applying migrations and/or seeding database");

    throw;
}



/************* RUN APP *************/

app.Run();













































































































































































