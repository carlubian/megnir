# Project Megnir
Very specific file backup service in C#.

## What does this program do?

* Run as a Docker container.
* Put a series of files and directories in zip files.
* Upload those zip files to an Azure Datalake.
* Set a cron job inside the Docker to run every week.

## What is out of scope for this program?

* Incremental backups.
* Run on or synchronize multiple machines.
* Send files to other targets.

## Known issues

* Backing up Windows directories produces ZIP files that cannot be opened with Windows File Explorer, as the drive letter ("C:") is made into a directory inside the ZIP.

## Instructions

TODO Create Docker and maybe volume for appsettings?

```json
{
  "HostName": "Node-1",
  "Jobs": [
    {
      "Name": "Grafana",
      "ElementsToBackup": {
        "Files": [
          "/var/tmp/file1.txt",
          "/var/tmp/file2.txt"
        ],
        "Directories": [
          "/usr/local/grafana",
          "/var/lib/grafana",
          "/usr/share/grafana"
        ]
      }
    }
  ],
  "Target": "AzureStorage",
  "AzureSettings": {
    "ConnectionString": "root:pzwrd@192.168.2.3:5005/endpoint"
  }
}
```

TODO Explain settings format.
