using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroriaBot.Services.XIVAPI.Responses
{
    public class Pagination
    {
        [JsonProperty("Page", NullValueHandling = NullValueHandling.Ignore)]
        public int Page { get; set; }

        [JsonProperty("PageNext")]
        public int? PageNext { get; set; }

        [JsonProperty("PagePrev")]
        public int? PagePrev { get; set; }

        [JsonProperty("PageTotal", NullValueHandling = NullValueHandling.Ignore)]
        public int PageTotal { get; set; }

        [JsonProperty("Results", NullValueHandling = NullValueHandling.Ignore)]
        public int Results { get; set; }

        [JsonProperty("ResultsPerPage", NullValueHandling = NullValueHandling.Ignore)]
        public int ResultsPerPage { get; set; }

        [JsonProperty("ResultsTotal", NullValueHandling = NullValueHandling.Ignore)]
        public int ResultsTotal { get; set; }
    }
}
