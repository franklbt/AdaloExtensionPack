# AdaloExtensionPack.Core

[![nuget](https://img.shields.io/nuget/v/AdaloExtensionPack.Core)](https://www.nuget.org/packages/AdaloExtensionPack.Core) ![downloads](https://img.shields.io/nuget/dt/AdaloExtensionPack.Core)

Helpers library to connect C# Web app to [Adalo](https://adalo.com) tables API

# Tables

In the Startup.cs file, add in the method `ConfigureServices()` :
 
```csharp    
services.AddAdalo(x =>
{
    x.AddApplication("[your Adalo App Id]", "[your API token here]")
       .AddTable<SomeEntity>("[table Id]", cached: false)
       .AddTable<SomeEntity>("[table Id 2]", cached: false);
});
```        

Then inject `IAdaloTableService<SomeEntity>` in your controller/services to connect directly to Adalo tables API.

> Note: SomeEntity need to inherit from `AdaloExtensionPack.Core.Tables.Data.AdaloEntity`

### Cached tables

If the `cached` parameter is set to the `true` value in the call to `AddTable`, a cached table service and a controller with methods to create,
read, update and delete this type of entity will be generated:

![cached endpoints](https://i.imgur.com/ZGPUPYQ.png)

The generated controller can be used as an external collection in Adalo in replacement of the original table
to improve performance.
In this case, all the write operations must be done on the table controller to ensure the cache is always up to date.

Additionally, the entity list endpoint (`GET /tables/some-entities`) supports OData syntax to select, 
filter, order and take elements.
You can read more on this syntax [here](https://www.odata.org/getting-started/basic-tutorial/#queryData). 

You can access theses cached tables from your client code using this service interface: `IAdaloTableCacheService<SomeEntity>`

### Views

Views are filtered and mapped collections available through a controller GET action.

They are more easily usable from an Adalo perspective compare to filtered Get All cache API endpoint, since they do not require knowledge of OData query syntax.
Also, the views are more flexible since other mapping possibilities and aggregations can be made thanks to mapping function.

You can create a view by adding this code inside the `AddAdalo()` call:

```csharp    
x => x.AddApplication("[your Adalo App Id]", "[your API token here]")
    .AddView<SomeContext, SomeEntity, SomeProjection>(
        // Build a context which be reused in predicate and mapping. 
        // This can be a service or a shared state for example.
        serviceProvider => serviceProvider.GetRequiredService<SomeContext>(), 
        (ctx, entity) => entity.IsValid, // Predicate example
        (ctx, entity) => new SomeProjection(entity)); // Mapping
```  

This will generate this method:

![endpoints views](https://i.imgur.com/KBTHjBi.png)

### Security

To secure the generated controller tables and views, it is possible to require an API Key to request these endpoints.

You can do so by adding a call to `WithTableCacheControllerApiKey()` method:

```csharp
builder.Services.AddAdalo(opts => /* ... */)
    .WithTableCacheControllerApiKey("[api-key]");

// Or

builder.Services.AddAdalo(opts =>  /* ... */)
    .WithTableCacheControllerApiKey(x => {
        x.ApiKey = "api-key";
        x.ApiKeyHeaderName = "X-Api-Key";
    });
```
