using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;

namespace CrystalReportWebAPI.Utilities
{
    internal static class ConnectionStringResolver
    {
        private const string DefaultConnectionName = "DefaultConnection";
        private const string DefaultConnectionEnvironmentVariable = "ConnectionStrings__DefaultConnection";
        private const string UseWebConfigForDefaultConnectionSetting = "UseWebConfigForDefaultConnection";
        private const string UserSecretsId = "CrystalReportWebAPI-DefaultConnection";

        public static string ResolveById(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                return null;
            }

            if (string.Equals(connectionId, DefaultConnectionName, StringComparison.OrdinalIgnoreCase))
            {
                return ResolveDefaultConnection();
            }

            return ConfigurationManager.ConnectionStrings[connectionId]?.ConnectionString;
        }

        public static string ResolveDefaultConnection()
        {
            if (UseWebConfigForDefaultConnection())
            {
                var webConfigConnection = ConfigurationManager.ConnectionStrings[DefaultConnectionName]?.ConnectionString;
                if (!string.IsNullOrWhiteSpace(webConfigConnection))
                {
                    return webConfigConnection.Trim();
                }
            }

            var environmentConnection = Environment.GetEnvironmentVariable(DefaultConnectionEnvironmentVariable);
            if (!string.IsNullOrWhiteSpace(environmentConnection))
            {
                return environmentConnection.Trim();
            }

            var userSecretConnection = TryReadUserSecretConnectionString(DefaultConnectionName);
            if (!string.IsNullOrWhiteSpace(userSecretConnection))
            {
                return userSecretConnection.Trim();
            }

            if (!UseWebConfigForDefaultConnection())
            {
                var fallbackWebConfigConnection = ConfigurationManager.ConnectionStrings[DefaultConnectionName]?.ConnectionString;
                if (!string.IsNullOrWhiteSpace(fallbackWebConfigConnection))
                {
                    return fallbackWebConfigConnection.Trim();
                }
            }

            return null;
        }

        private static bool UseWebConfigForDefaultConnection()
        {
            var setting = ConfigurationManager.AppSettings[UseWebConfigForDefaultConnectionSetting];
            return bool.TryParse(setting, out var enabled) && enabled;
        }

        private static string TryReadUserSecretConnectionString(string connectionName)
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (string.IsNullOrWhiteSpace(appDataPath))
            {
                return null;
            }

            var secretsFilePath = Path.Combine(appDataPath, "Microsoft", "UserSecrets", UserSecretsId, "secrets.json");
            if (!File.Exists(secretsFilePath))
            {
                return null;
            }

            var json = File.ReadAllText(secretsFilePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            var document = JObject.Parse(json);
            var nestedValue = document["ConnectionStrings"]?[connectionName]?.Value<string>();
            if (!string.IsNullOrWhiteSpace(nestedValue))
            {
                return nestedValue;
            }

            var flatValue = document[$"ConnectionStrings:{connectionName}"]?.Value<string>();
            if (!string.IsNullOrWhiteSpace(flatValue))
            {
                return flatValue;
            }

            return null;
        }
    }
}
