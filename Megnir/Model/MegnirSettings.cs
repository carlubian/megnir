namespace Megnir.Model;

public class MegnirSettings
{
    public string HostName { get; set; } = "UnknownNode";
    public IEnumerable<BackupJob> Jobs { get; set; } = Array.Empty<BackupJob>();
    public string Target { get; set; } = string.Empty;
    public AzureSettings AzureSettings { get; set; } = new();
}

public class BackupJob
{
    public string Name { get; set; } = "UnknownJob";
    public ElementsToBackup ElementsToBackup { get; set; } = new();
}

public class ElementsToBackup
{
    public IEnumerable<string> Files { get; set; } = Array.Empty<string>();
    public IEnumerable<string> Directories { get; set; } = Array.Empty<string>();
}

public class AzureSettings
{
    public string ConnectionString { get; set; } = string.Empty;
}
