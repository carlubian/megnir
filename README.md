# Project Megnir
Very specific file backup service in C#.

## What does this program do?

* Run as a Docker container.
* Put a series of files and directories in zip files.
* Upload those zip files to an Azure Datalake.
* Be launched periodically via cron in the host machine.

## What is out of scope for this program?

* Incremental backups.
* Run on or synchronize multiple machines.
* Send files to other targets.

## Known issues

* The cron has to be manually set in the host machine to run Megnir in a time interval. The Docker only does a backup interation when it's executed.

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

TODO Script file and cron job:

```sh
#!/bin/bash

# Run the Megnir backup service
docker run -d -v /home/user/utils/megnir.json:/app/appsettings.json -v /home/user/volumes/grafana:/data/volumes/grafana megnir:1.2.3-amd64
```

```cron
0 2 * * 0 /home/user/utils/megnir.sh
```
