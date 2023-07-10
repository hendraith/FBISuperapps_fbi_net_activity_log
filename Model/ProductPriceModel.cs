using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ActivityLog.Model
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [BsonIgnoreExtraElements]
    public class ProductPriceModel
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

        [BsonElement("distribution_channel")]
        [JsonProperty("distribution_channel")]
        public int DistributionChannel { get; set; }

        [BsonElement("before")]
        [JsonProperty("before")]
        public double Before { get; set; }
        
        [BsonElement("after")]
        [JsonProperty("after")]
        public double After { get; set; }

        [BsonElement("price_condition")]
        [JsonProperty("price_condition")]
        public string PriceCondition { get; set; }

        [BsonElement("active")]
        [JsonProperty("active")]
        public bool Active { get; set; }

        [BsonElement("created_at")]
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("created_by")]
        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }
    }
}
