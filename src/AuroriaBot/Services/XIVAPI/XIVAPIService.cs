using AuroriaBot.Services.XIVAPI.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuroriaBot.Services.XIVAPI
{
    public class XIVAPIService
    {
        private readonly HttpClient _httpClient;

        public XIVAPIService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://xivapi.com")
            };
        }

        public async Task<SearchResponse> SearchItem(string itemName)
        {
            var response = await _httpClient.GetAsync($"search?indexes=Item&string={itemName}");
            return JsonConvert.DeserializeObject<SearchResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}
