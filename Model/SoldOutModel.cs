using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace ActivityLog.Model
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [BsonIgnoreExtraElements]
    public class SoldOutModel
    {
        [BsonId]
        [JsonProperty("id")]
        public ObjectId ID { get; set; }

        [BsonElement("site_code")]
        [JsonProperty("site_code")]
        public string SiteCode { get; set; }

        [BsonElement("sku")]
        [JsonProperty("sku")]
        public string SKU { get; set; }

        [BsonElement("platform")]
        [JsonProperty("platform")]
        public string Platform { get; set; }

        [BsonElement("value")]
        [JsonProperty("value")]
        public bool Value { get; set; }

        [BsonElement("created_at")]
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("created_by")]
        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }
    }
}
