using Azure.Storage.Files.DataLake;

namespace Megnir.Core.Services;

public class AzureService
{
    private DataLakeServiceClient Azure;

	public AzureService(string conStr)
	{
		Azure = new(conStr);
	}

	public void Upload(string path, string host, string job)
	{
		var client = Azure.GetFileSystemClient("armali");
		client.CreateIfNotExists();
		var directory = client.GetDirectoryClient("Backups");
		directory.CreateIfNotExists();
		directory = directory.GetSubDirectoryClient(host);
        directory.CreateIfNotExists();
        directory = directory.GetSubDirectoryClient(job);
        directory.CreateIfNotExists();

		var file = directory.GetFileClient(new FileInfo(path).Name);
		file.Upload(path);
    }
}
