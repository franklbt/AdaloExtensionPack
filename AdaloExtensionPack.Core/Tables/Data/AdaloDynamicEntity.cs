using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdaloExtensionPack.Core.Tables.Data;

public class AdaloDynamicEntity : Dictionary<string, object>, IAdaloEntity
{
    public AdaloDynamicEntity()
    {
    }

    public AdaloDynamicEntity(IDictionary<string, object> dictionary) : base(dictionary)
    {
    }

    [JsonIgnore]
    public int Id
    {
        get => this["id"] as int? ?? 0;
        set => this["id"] = value;
    }

    [JsonIgnore]
    public DateTimeOffset CreatedAt
    {
        get => this["created_at"] as DateTimeOffset? ?? DateTimeOffset.MinValue;
        init => this["created_at"] = value;
    }

    [JsonIgnore]
    public DateTimeOffset UpdatedAt
    {
        get => this["updated_at"] as DateTimeOffset? ?? DateTimeOffset.MinValue;
        init => this["updated_at"] = value;
    }
}
