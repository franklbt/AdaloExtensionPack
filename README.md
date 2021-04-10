[![nuget](https://img.shields.io/nuget/v/AdaloExtensionPack.Core)](https://www.nuget.org/packages/AdaloExtensionPack.Core)

# AdaloExtensionPack

Helpers to connect C# Web app to Adalo tables API

# Usage 

In the Startup.cs file add in the method ConfigureServices() : 

     services.AddAdalo(x =>
     {
         x.Token = "[your API token here]";
         x.AppId = Guid.Parse("[your Adalo App Id]");
         x.AddTable<TableType>("[table Id]");
     });
     
Then inject `IAdaloTableService<TableType>` in your controller/services to connect to Adalo tables API.
