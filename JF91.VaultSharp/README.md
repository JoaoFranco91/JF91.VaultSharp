### JF91.VaultSharp

WIP Minimalist Vault Client.

### 1: Install Nuget Package
```
dotnet add package JF91.VaultSharp
```

<br>

#### Available authentication methods:

- Token
- App Role

### Available features:

- Get all secrets from path
- Get secret name value
- Get secret name value and update appsettings.json (Currently limited to one nested property)


<br>

### How to use:

Add this to your appsettings.json:
```
"VaultSettings": {
    "AuthenticationMethod": "approle",
    "Host": "http://localhost:8200",
    "RoleId": "",
    "SecretId": "",
    "Token": ""
}
```
Possible values:

- AuthenticationMethod: approle, token

<br>

All methods are extensions methods from IConfiguration, which you can inject anywhere in your application and call them like this:

```
private readonly IConfiguration _configuration;

public MyController
(
    IConfiguration configuration
)
{
    _configuration = configuration;
}

[HttpGet(Name = "MyEndpoint")]
public ActionResult HelloWorld()
{
    var secrets = _configuration.GetVaultSecrets
    (
        "kv",
        "secretPath"
    );

    var secretValue = _configuration.GetVaultSecret
    (
        "kv",
        "secretPath",
        "secretName"
    );

    _configuration.InjectSecretToConfiguration
    (
        "kv",
        "secretPath",
        "secretName",
        "RootProperty:ChildProperty",
        "Development" // Application Environment
    );

    return "Hello World!";
}
```