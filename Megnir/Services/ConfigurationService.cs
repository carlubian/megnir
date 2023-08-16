using OneOf;
using Megnir.Failures;
using Snow.Attributes;
using System.Text.Json;

namespace Megnir.Services;

[Service]
internal class ConfigurationService
{
    public OneOf<T, IFailureMode> FromJsonFile<T>(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return new MissingFile(path);

        // Read all file contents
        var text = File.ReadAllText(path);

        // Deserialize model class
        var model = JsonSerializer.Deserialize<T>(text);

        if (model is null)
            return new InvalidFormat(path, "JSON");

        return model;
    }
}
