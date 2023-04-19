using Megnir.Core.Services;
using Megnir.Model;

Console.WriteLine("Project Megnir is starting...");

var configService = new ConfigurationService<MegnirSettings>();
var config = configService.FromJsonFile("appsettings.json");
var zipService = new ZipService();

if (config is null)
{
    Console.WriteLine("Error parsing configuration. Execution will stop.");
    return;
}

Console.WriteLine("Initialization complete.");
Console.WriteLine($"Found {config.Jobs.Count()} jobs to backup.");

// Generate one zip file per job
List<string> filesToUpload = new();
foreach (var job in config.Jobs)
{
    Console.WriteLine($"Starting backup job {job.Name}...");

    filesToUpload.Add(zipService.SetOutputFile($"{DateTime.Now:yyyy-MM-dd_HH:mm:ss}_{config.HostName}_{job.Name}.zip")
        .AddFiles(job.ElementsToBackup.Files)
        .AddDirectories(job.ElementsToBackup.Directories)
        .Compress());

    Console.WriteLine($"Created ZIP file for job {job.Name}.");
}

// Upload all the files
foreach (var zip in filesToUpload)
{
    Console.WriteLine($"Uploading file {zip}...");
}

Console.WriteLine("All files uploaded. Deleting local cache...");

// Delete local files
foreach (var zip in filesToUpload)
{
    File.Delete(zip);
}

Console.WriteLine("Backup complete. Bye!");
