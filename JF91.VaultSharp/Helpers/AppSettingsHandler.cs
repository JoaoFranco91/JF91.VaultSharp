using System.Text.Json;
using Newtonsoft.Json;

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
            var filePath = Path.Combine
            (
                AppContext.BaseDirectory,
                environment.Any()
                    ? $"appsettings.{environment}.json"
                    : "appsettings.json"
            );

            string json = File.ReadAllText(filePath);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            var sectionPath = key.Split(":")[0];

            if (!string.IsNullOrEmpty(sectionPath))
            {
                var keyPath = key.Split(":")[1];
                jsonObj[sectionPath][keyPath] = value;
            }
            else
            {
                jsonObj[sectionPath] = value; // if no sectionpath just set the value
            }

            string output = JsonConvert.SerializeObject
            (
                jsonObj,
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