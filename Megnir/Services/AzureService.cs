using Azure.Storage.Files.DataLake;
using OneOf;
using Megnir.Failures;
using Megnir.Model;
using Snow.Attributes;

namespace Megnir.Services;

[Service]
internal class AzureService
{
#pragma warning disable CS8618
    [Autowired]
    private LogService Log;

    private DataLakeServiceClient Azure;
#pragma warning restore CS8618

    internal void Connect(string connectionString)
    {
        Azure = new DataLakeServiceClient(connectionString);
    }

    internal OneOf<Success, IFailureMode> Upload(string path, string host, string job)
    {
        if (Azure is null)
            return new ConnectionError();

        Log.Info($"Azure:{job}", $"Uploading file {path}");

        var client = Azure.GetFileSystemClient("RDSP-AI");
        client.CreateIfNotExists();
        var directory = client.GetDirectoryClient("Backups");
        directory.CreateIfNotExists();
        directory = directory.GetSubDirectoryClient(host);
        directory.CreateIfNotExists();
        directory = directory.GetSubDirectoryClient(job);
        directory.CreateIfNotExists();

        var file = directory.GetFileClient(new FileInfo(path).Name);

        try
        {
            var response = file.Upload(path);
            if (response.Value is null)
                throw new Exception();
        }
        catch
        {
            Log.Error($"Azure:{job}", $"File {path} couldn't be uploaded to Azure");
        }

        return Success.Instance;
    }
}
