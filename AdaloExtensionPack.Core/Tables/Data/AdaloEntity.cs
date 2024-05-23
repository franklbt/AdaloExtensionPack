using System.Text.Json.Serialization;

namespace AdaloExtensionPack.Core.Tables.Data
{
    public class AdaloEntity
    {
        [JsonPropertyName("id")] 
        public int Id { get; set; }
        
        [JsonPropertyName("created_at")] 
        public string CreatedAt { get; init; }

        [JsonPropertyName("updated_at")] 
        public string UpdatedAt { get; init; }
    }
}
