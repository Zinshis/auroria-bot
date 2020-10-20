using AuroriaBot.Services.XIVAPI;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroriaBot.Discord.Commands
{
    public class FFXIVModule : ModuleBase<SocketCommandContext>
    {
        private readonly XIVAPIService _xivApi;

        public FFXIVModule(XIVAPIService xivApi)
        {
            _xivApi = xivApi;
        }


        [Command("xiv")]
        [Summary("Searches the xiv database for items with the specified name.")]
        public async Task XIVAsync([Remainder][Summary("Item name")] string itemName)
        {
            var response = await _xivApi.SearchItem(itemName);
            await ReplyAsync(response.Results.FirstOrDefault()?.Name ?? "No Results");
        }
    }
}
