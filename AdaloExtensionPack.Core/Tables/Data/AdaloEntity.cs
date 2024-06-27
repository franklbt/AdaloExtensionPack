using System;
using System.Dynamic;
using System.Text.Json.Serialization;

namespace AdaloExtensionPack.Core.Tables.Data
{
    public class AdaloEntity : IAdaloEntity
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; init; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset UpdatedAt { get; init; }
    }
}
