using System.IO.Compression;

namespace Megnir.Core.Services;

public class ZipService
{
    private string Path = "backup.zip";
    private IEnumerable<string> Files { get; set; } = Array.Empty<string>();
    private IEnumerable<string> Directories { get; set; } = Array.Empty<string>();

    public ZipService SetOutputFile(string path)
    {
        Path = path;
        return this;
    }

    public ZipService AddDirectories(IEnumerable<string> directories)
    {
        Directories = directories;
        return this;
    }

    public ZipService AddFiles(IEnumerable<string> files)
    {
        Files = files;
        return this;
    }

    public string Compress()
    {
        var zip = ZipFile.Open(Path, ZipArchiveMode.Create);

        foreach (var file in Files)
        {
            zip.CreateEntryFromFile(file, NormalizePath(file));
        }

        foreach (var dir in Directories)
        {
            Compress(dir, zip);
        }

        zip.Dispose();
        return Path;
    }

    private void Compress(string dir, ZipArchive zip)
    {
        foreach (var file in Directory.EnumerateFiles(dir))
        {
            zip.CreateEntryFromFile(file, NormalizePath(file));
        }

        foreach (var subdir in Directory.EnumerateDirectories(dir))
        {
            Compress(subdir, zip);
        }
    }

    private string NormalizePath(string file)
    {
        var path = new FileInfo(file).DirectoryName;
        path = System.IO.Path.Combine(path, new FileInfo(file).Name);
        return path;
    }
}
