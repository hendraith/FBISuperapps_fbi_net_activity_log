using ActivityLog.Model;
using Newtonsoft.Json;

namespace ActivityLog.Dto.SoldOut
{
    public class SoldOutResponse
    {
        [JsonProperty("total_data")]
        public long TotalData { get; set; }

        [JsonProperty("total_filtered_data")]
        public long TotalFilteredData { get; set; }

        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }

        [JsonProperty("last_page")]
        public int LastPage { get; set; }

        [JsonProperty("data")]
        public List<SoldOutModel> Data { get; set; }
    }

}
