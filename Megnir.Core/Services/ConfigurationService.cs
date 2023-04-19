using System.Text.Json;

namespace Megnir.Core.Services;

public class ConfigurationService<T> where T: class, new()
{
    public T? FromJsonFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return default;

        // Read all file contents
        var text = File.ReadAllText(path);

        // Deserialize model class
        var model = JsonSerializer.Deserialize<T>(text);

        return model;
    }
}
