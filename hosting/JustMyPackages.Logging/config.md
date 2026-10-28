```
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "System": "Error",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information",
        "Microsoft.AspNetCore.Hosting.Diagnostics": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Async",
        "Args": {
          "configure": {
            "ClefFile": {
              "Name": "File",
              "Args": {
                "path": "Logs/{ServiceName}-.clef", // insert name of your service
                "rollingInterval": "Infinite",
                "fileSizeLimitBytes": 104857600,
                "retainedFileCountLimit": 5,
                "rollOnFileSizeLimit": true,
                "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact",
                "shared": true // needed for windows services
              }
            },
            "TxtFile": {
              "Name": "File",
              "Args": {
                "path": "Logs/{ServiceName}-.txt", // insert name of your service
                "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}",
                "rollingInterval": "Infinite",
                "fileSizeLimitBytes": 104857600,
                "retainedFileCountLimit": 5,
                "rollOnFileSizeLimit": true,
                "shared": true // needed for windows services
              }
            },
            "Console": {
              "Name": "Console",
              "Args": {
                "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}"
              }
            }
            //"Seq": {
            //  "Name": "Seq",
            //  "Args": {
            //    "serverUrl": "http://localhost:5341"
            //  }
            //}
          }
        }
      }
    ]
    //"Filter": [
    //  {
    //    "Name": "ByIncludingOnly",
    //    "Args": {
    //      "expression": "RequestPath like '/health%'"
    //    }
    //  }
    //]
  }

```