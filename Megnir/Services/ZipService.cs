using OneOf;
using Megnir.Failures;
using Megnir.Model;
using Snow.Attributes;
using System.IO.Compression;

namespace Megnir.Services;

[Service]
internal class ZipService
{
#pragma warning disable CS8618
    [Autowired]
    private LogService Log;
#pragma warning restore CS8618

    internal OneOf<string, IFailureMode> RunBackupJob(string hostName, BackupJob job)
    {
        var fileName = $"{DateTime.Now:yyyy-MM-dd_HHmmss}_{hostName}_{job.Name}.zip";

        if (File.Exists(fileName))
        {
            Log.Warning($"Zip:{job.Name}", $"File {fileName} already exists and will be overwritten");
            File.Delete(fileName);
        }

        Log.Info($"Zip:{job.Name}", "Starting compression process");
        var zip = ZipFile.Open(fileName, ZipArchiveMode.Create);

        // Add files from the backup job
        foreach (var file in job.ElementsToBackup.Files)
        {
            if (!File.Exists(file))
            {
                Log.Warning($"Zip:{job.Name}", $"File {file} doesn't exist and will be ignored");
                continue;
            }

            Log.Trace($"Zip:{job.Name}", $"Add file {file} to backup");
            zip.CreateEntryFromFile(file, NormalizePath(file));
        }

        // Add directories from backup job
        foreach (var dir in job.ElementsToBackup.Directories)
        {
            if (!Directory.Exists(dir))
            {
                Log.Warning($"Zip:{job.Name}", $"Directory {dir} doesn't exist and will be ignored");
                continue;
            }

            Log.Trace($"Zip:{job.Name}", $"Found directory {dir} to backup");
            Compress(job.Name, dir, zip);
        }

        zip.Dispose();
        Log.Info($"Zip:{job.Name}", $"File {fileName} created for backup job");
        return fileName;
    }

    // Recursive compression for directories (depth-first search)
    private void Compress(string job, string dir, ZipArchive zip)
    {
        // Add files in this directory
        foreach (var file in Directory.EnumerateFiles(dir))
        {
            Log.Trace($"Zip:{job}", $"Add file {file} to backup");
            zip.CreateEntryFromFile(file, NormalizePath(file));
        }

        // Add subdirectories in this directory
        foreach (var subdir in Directory.EnumerateDirectories(dir))
        {
            Log.Trace($"Zip:{job}", $"Found directory {dir} to backup");
            Compress(job, subdir, zip);
        }
    }

    private string NormalizePath(string file)
    {
        var path = new FileInfo(file).DirectoryName;

        // Windows-specific fix: Remove drive letter colons (C: becomes C_)
        //   This causes the ZIP file to appear empty
        path = path?.Replace(':', '_');

        path = Path.Combine(path ?? string.Empty, new FileInfo(file).Name);
        return path;
    }
}
