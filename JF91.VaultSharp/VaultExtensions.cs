using JF91.VaultSharp.Helpers;
using VaultSharp;
using VaultSharp.V1.Commons;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using Microsoft.Extensions.Configuration;
using VaultSharp.V1.AuthMethods.Token;

namespace JF91.VaultSharp;

public static class VaultExtensions
{
    public static string GetVaultSecret
    (
        this IConfiguration config,
        string mountPoint,
        string path,
        string key
    )
    {
        try
        {
            var vaultSettings = new VaultSettings();
            config.GetSection(nameof(VaultSettings)).Bind(vaultSettings);

            IAuthMethodInfo authMethod = vaultSettings.AuthenticationMethod.ToLower() == "approle"
                ? new AppRoleAuthMethodInfo
                (
                    vaultSettings.RoleId,
                    vaultSettings.SecretId
                )
                // WARNING: Should not be used in production. Use it only for testing purposes.
                : new TokenAuthMethodInfo
                (
                    vaultSettings.Token
                );

            var vaultClientSettings = new VaultClientSettings
            (
                vaultSettings.Host,
                authMethod
            );

            var vaultClient = new VaultClient(vaultClientSettings);

            Secret<SecretData> secrets;
            if (mountPoint.Any())
            {
                secrets = vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync
                (
                    path: path,
                    mountPoint: mountPoint
                ).Result;
            }
            else
            {
                secrets = vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync
                (
                    path: path
                ).Result;
            }

            return secrets.Data.Data.ElementAt(secrets.Data.Data.Keys.ToList().IndexOf(key)).Value.ToString();
        }
        catch (Exception ex)
        {
            throw new ArgumentNullException
            (
                "Error getting vault secret.",
                ex
            );
        }
    }

    public static IDictionary<string, string> GetVaultSecrets
    (
        this IConfiguration config,
        string mountPoint,
        string path
    )
    {
        try
        {
            var vaultSettings = new VaultSettings();
            config.GetSection(nameof(VaultSettings)).Bind(vaultSettings);

            IAuthMethodInfo authMethod = vaultSettings.AuthenticationMethod.ToLower() == "approle"
                ? new AppRoleAuthMethodInfo
                (
                    vaultSettings.RoleId,
                    vaultSettings.SecretId
                )
                // WARNING: Should not be used in production. Use it only for testing purposes.
                : new TokenAuthMethodInfo
                (
                    vaultSettings.Token
                );

            var vaultClientSettings = new VaultClientSettings
            (
                vaultSettings.Host,
                authMethod
            );

            var vaultClient = new VaultClient(vaultClientSettings);

            Secret<SecretData> secrets;
            if (mountPoint.Any())
            {
                secrets = vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync
                (
                    path: path,
                    mountPoint: mountPoint
                ).Result;
            }
            else
            {
                secrets = vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync
                (
                    path: path
                ).Result;
            }

            return secrets.Data.Data.ToDictionary
            (
                k => k.Key,
                v => v.Value.ToString()
            );
        }
        catch (Exception ex)
        {
            throw new ArgumentNullException
            (
                "Error getting vault secret.",
                ex
            );
        }
    }

    public static void InjectSecretToConfiguration
    (
        this IConfiguration config,
        string mountPoint,
        string secretPath,
        string key,
        string configPath,
        string environment
    )
    {
        try
        {
            var vaultSettings = new VaultSettings();
            config.GetSection(nameof(VaultSettings)).Bind(vaultSettings);

            IAuthMethodInfo authMethod = vaultSettings.AuthenticationMethod.ToLower() == "approle"
                ? new AppRoleAuthMethodInfo
                (
                    vaultSettings.RoleId,
                    vaultSettings.SecretId
                )
                // WARNING: Should not be used in production. Use it only for testing purposes.
                : new TokenAuthMethodInfo
                (
                    vaultSettings.Token
                );

            var vaultClientSettings = new VaultClientSettings
            (
                vaultSettings.Host,
                authMethod
            );

            var vaultClient = new VaultClient(vaultClientSettings);

            Secret<SecretData> secrets;
            if (mountPoint.Any())
            {
                secrets = vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync
                (
                    path: secretPath,
                    mountPoint: mountPoint
                ).Result;
            }
            else
            {
                secrets = vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync
                (
                    path: secretPath
                ).Result;
            }

            var secret = secrets.Data.Data.ElementAt(secrets.Data.Data.Keys.ToList().IndexOf(key))
                .Value
                .ToString();

            AppSettingsHandler.UpdateAppSetting
            (
                configPath,
                secret,
                environment
            );
        }
        catch (Exception ex)
        {
            throw new ArgumentNullException
            (
                "Error getting vault secret.",
                ex
            );
        }
    }
}