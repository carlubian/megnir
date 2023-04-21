using Megnir.Core.Services;
using Megnir.Model;

Console.WriteLine("Project Megnir is starting...");

var configService = new ConfigurationService<MegnirSettings>();
var config = configService.FromJsonFile("appsettings.json");
var zipService = new ZipService();
var azureService = new AzureService(config.AzureSettings.ConnectionString);

if (config is null)
{
    Console.WriteLine("Error parsing configuration. Execution will stop.");
    return;
}

Console.WriteLine("Initialization complete.");
Console.WriteLine($"Found {config.Jobs.Count()} jobs to backup.");

// Generate one zip file per job
List<(string path, string host, string job)> filesToUpload = new();
foreach (var job in config.Jobs)
{
    Console.WriteLine($"  Starting backup job {job.Name}...");

    filesToUpload.Add((zipService.SetOutputFile($"{DateTime.Now:yyyy-MM-dd_HHmmss}_{config.HostName}_{job.Name}.zip")
        .AddFiles(job.ElementsToBackup.Files)
        .AddDirectories(job.ElementsToBackup.Directories)
        .Compress(), config.HostName, job.Name));

    Console.WriteLine($"  Created ZIP file for job {job.Name}.");
}

// Upload all the files
foreach (var (path, host, job) in filesToUpload)
{
    Console.WriteLine($"  Uploading file {path}...");
    azureService.Upload(path, host, job);
}

Console.WriteLine("All files uploaded. Deleting local cache...");

// Delete local files
foreach (var (path, _1, _2) in filesToUpload)
{
    File.Delete(path);
}

Console.WriteLine("Backup complete. Bye!");
