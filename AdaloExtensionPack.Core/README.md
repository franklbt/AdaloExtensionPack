# AdaloExtensionPack

[![nuget](https://img.shields.io/nuget/v/AdaloExtensionPack.Core)](https://www.nuget.org/packages/AdaloExtensionPack.Core) ![downloads](https://img.shields.io/nuget/dt/AdaloExtensionPack.Core)

Helpers to connect C# Web app to Adalo tables API

# Usage

### Classic table

In the Startup.cs file add in the method ConfigureServices() :

     services.AddAdalo(x =>
     {
         x.AddApplication("[your Adalo App Id]", "[your API token here]")
            .AddTable<SomeEntity>("[table Id]", cached: false)
            .AddTable<SomeEntity>("[table Id 2]", cached: true);
     });

Then inject `IAdaloTableService<SomeEntity>` in your controller/services to connect directly to Adalo tables API,
or `IAdaloTableCacheService<SomeEntity>` to add a cache layer between Adalo and your app.

> Note: SomeEntity need to inherit from `AdaloExtensionPack.Core.Adalo.AdaloEntity`

### Cached tables Services and Controllers

If the `cached` parameter is set to the `true` value in the call to `AddTable`, a service and a controller with methods to create,
read, update and delete this type of entity will be generated:

![cached endpoints](https://i.imgur.com/ZGPUPYQ.png)

The generated controller can be used as external collection in Adalo to improve performances of your tables.

Additionally, the entity list endpoint (GET /tables/some-entities) support OData syntax to select, filter, order and take elements. 
You can read more on this syntax [here](https://www.odata.org/getting-started/basic-tutorial/#queryData)

You can access theses cached tables from your client code using this service interface: `AdaloTableCacheService<SomeEntity>`

### Views

Views are filtered and mapped collections available through a controller GET action.

They are more easily usable from an Adalo perspective compare to filtered Get All cache API endpoint, since they do not require knowledge of OData query syntax.

You can create a view by add this code inside the `AddAdalo()` call:

    x.AddApplication("[your Adalo App Id]", "[your API token here]")
        .AddView<SomeContext, SomeEntity, SomeProjection>(
            // Build a context which be reused in predicate and mapping. This can be a service or a shared state for example.
            serviceProvider => serviceProvider.GetRequiredService<SomeContext>(), 
            (ctx, entity) => entity.IsValid, // Predicate example
            (ctx, entity) => new SomeProjection(entity)); // Mapping

This will generate this method:

![endponts views](https://i.imgur.com/KBTHjBi.png)
