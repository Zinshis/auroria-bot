using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroriaBot.Services.XIVAPI.Responses
{
    public class SearchResult
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("Icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }

        [JsonProperty("Name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("Url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("UrlType", NullValueHandling = NullValueHandling.Ignore)]
        public string UrlType { get; set; }

        [JsonProperty("_", NullValueHandling = NullValueHandling.Ignore)]
        public string Dash { get; set; }

        [JsonProperty("_Score", NullValueHandling = NullValueHandling.Ignore)]
        public long? Score { get; set; }
    }
}
