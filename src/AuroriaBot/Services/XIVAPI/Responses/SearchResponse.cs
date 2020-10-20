using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroriaBot.Services.XIVAPI.Responses
{
    public class SearchResponse
    {
        [JsonProperty("Pagination", NullValueHandling = NullValueHandling.Ignore)]
        public Pagination Pagination { get; set; }

        [JsonProperty("Results", NullValueHandling = NullValueHandling.Ignore)]
        public List<SearchResult> Results { get; set; }

        [JsonProperty("SpeedMs", NullValueHandling = NullValueHandling.Ignore)]
        public long? SpeedMs { get; set; }
    }
}
