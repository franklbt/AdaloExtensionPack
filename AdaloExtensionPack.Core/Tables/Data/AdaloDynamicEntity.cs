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


    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; init; }
}