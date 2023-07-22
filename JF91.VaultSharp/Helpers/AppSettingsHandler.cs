using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JF91.VaultSharp.Helpers;

public class AppSettingsHandler
{
    public static void UpdateAppSetting
    (
        string key,
        string value,
        string environment = ""
    )
    {
        try
        {
            key = key.Replace
            (
                ":",
                "."
            );

            var filePath = Path.Combine
            (
                AppContext.BaseDirectory,
                environment.Any()
                    ? $"appsettings.{environment}.json"
                    : "appsettings.json"
            );

            string json = File.ReadAllText(filePath);
            var jObject = JObject.Parse(json);

            var keyParts = key.Split(".");
            var parentPath = key.Substring
            (
                0,
                key.LastIndexOf(".")
            );
            
            var parentToken = jObject.SelectToken(parentPath);
            parentToken[keyParts.Last()] = value;

            string output = JsonConvert.SerializeObject
            (
                jObject,
                Formatting.Indented
            );

            File.WriteAllText
            (
                filePath,
                output
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine
            (
                "Error writing app settings | {0}",
                ex.Message
            );
        }
    }
}