using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroriaBot.Discord.Commands
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo) => ReplyAsync(echo);


        [Command("embedtest")]
        [Summary("embedtest")]
        public async Task EmbedTestAsync([Remainder][Summary("embedtest")] string test)
        {
            var onlineUsers = Context.Guild.Users.Where(x => x.Status == UserStatus.Online && !x.IsBot);
            

            var embedBuilder = new EmbedBuilder()
                .WithTitle("THIS IS A TEST");

            foreach (var user in onlineUsers)
            {
                embedBuilder.AddField(user.Username, 
                    $"Joined at {user.JoinedAt?.ToString("d", CultureInfo.CreateSpecificCulture("en-GB"))}");
            }

            await ReplyAsync(embed: embedBuilder.Build());
        }
    }
}




