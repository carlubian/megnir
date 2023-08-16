using Megnir.Model;
using Megnir.Services;
using Snow;
using Snow.Attributes;

namespace Megnir;

public static class Program
{
    public static void Main(string[] args)
    {
        SnowRunner.Run(new Runner(), args);
    }
}

public class Runner : ISnowRunnable
{
    // Initialize services
#pragma warning disable CS8618, CS0649
    [Autowired]
    private ConfigurationService Config;

    [Autowired]
    private LogService Log;

    [Autowired]
    private ZipService Zip;

    [Autowired]
    private AzureService Azure;
#pragma warning restore CS8618, CS0649

    public void Run(string[]? args)
    {
        Log.Info("Program:Run", $"Megnir backup service is starting now");

        // Load Megnir configuration
        var configObj = Config.FromJsonFile<MegnirSettings>("appsettings.json");
        if (configObj.TryPickT1(out var failure, out var _))
            Log.FailureMode(failure, true);

        var config = configObj.AsT0;

        Log.Trace("Program:Run", "Initialization complete");
        Log.Info("Program:Run", $"Found {config.Jobs.Count()} backup jobs for {config.HostName}");

        // Create ZIP files for all the backup jobs
        var files = new List<(string file, string job)>();
        foreach (var job in config.Jobs)
        {
            var result = Zip.RunBackupJob(config.HostName, job);
            if (result.TryPickT1(out failure, out var _))
                Log.FailureMode(failure, false);

            files.Add((result.AsT0, job.Name));
        }

        // Upload ZIP files to Azure
        foreach (var file in files)
        {
            var result = Azure.Upload(file.file, config.HostName, file.job);
            if (result.TryPickT1(out failure, out var _))
                Log.FailureMode(failure, false);
        }

        Log.Info("Program:Run", "All files uploaded to Azure. Cleaning local cache...");

        // Delete local copies of the backup files
        foreach (var file in files)
        {
            File.Delete(file.file);
        }

        Log.Info("Program:Run", "Local file cache cleaned. Megnir execution is complete");
    }
}
