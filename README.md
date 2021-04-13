[![nuget](https://img.shields.io/nuget/v/AdaloExtensionPack.Core)](https://www.nuget.org/packages/AdaloExtensionPack.Core)

# AdaloExtensionPack

Helpers to connect C# Web app to Adalo tables API

# Usage 

### Classic table

In the Startup.cs file add in the method ConfigureServices() : 

     services.AddAdalo(x =>
     {
         x.Token = "[your API token here]";
         x.AppId = Guid.Parse("[your Adalo App Id]");
         x.AddTable<SomeEntity>("[table Id]", cached: false);
     });
     
Then inject `IAdaloTableService<SomeEntity>` in your controller/services to connect to Adalo tables API.

> Note: SomeEntity need to inherit from `AdaloExtensionPack.Core.Adalo.AdaloEntity`

### Cached table

If the `cached` parameter is set to the `true` value in the call to `AddTable`, a controller with methods to create, read, update and delete this type of entities will be generated:

![cached endpoints](https://i.imgur.com/ZGPUPYQ.png)

Theses controllers can be used as external collections in Adalo to improve performances of your tables.

You can also access theses cached tables from your code with the class `AdaloTableCacheService<SomeEntity>`

### Views

Views are filtered and mapped collections available through a controller GET action.

You can create a view by add this line inside the `AddAdalo()` call:

    x.AddView<SomeContext, SomeEntity, SomeProjection>(
        serviceProvider => new SomeContext(serviceProvider), //Build a context which be reused in predicate and mapping
        (ctx, entity) => true, // Predicate
        (ctx, entity) => new SomeProjection(entity)); // Mapping
        
This will generate this method:
        
![endponts views](https://i.imgur.com/KBTHjBi.png)
