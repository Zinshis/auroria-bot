# auroria-bot
This is the code repository for the Auroria server's personal bot.

## Adding the bot to a server
In order to add the bot to one of the servers you're an Admin of,
Click on the link below and select the server you wish to add the bot to:
- https://discord.com/api/oauth2/authorize?client_id=763851722120364073&permissions=19520&scope=bot

The bot requires the following permissions:
- View Channels
- Send Messages
- Embed Links
- Add Reactions

## Bot Commands
_Coming soon_


## Contributing

### Documentation
_Coming soon_


### Development
To start developing and test the bot on the **Auroria Bot Test** Discord server request access to this server by messaging **yunzel**.
Next you will also need to be added as contributor to the Git repository.

The following software and SDK's are required for development:
- [Git CLI](https://git-scm.com/downloads) or a [Git Desktop](https://desktop.github.com/) to download the code locally
- [Visual Studio 2019 Community Edition](https://visualstudio.microsoft.com/vs/) to develop the bot with C#

Once you have checked out the code, you can open the solution (.sln) file with Visual Studio.
To start debugging, you will first need to copy and rename the _appsettings.template.json_ to _appsettings.json_ in the AuroriaBot project.
Secondly you will need to paste the token of the test bot in the "Token" property value in the _appsettings.json_ file.
The test bot's token is pinned in the announcement channel in the **Auroria Bot Test** Discord server.

Note: only **1** person can debug at a time with the same bot (token).


### Discord Bot Community Resources
- Javascript: https://github.com/discordjs/discord.js
- Python: https://github.com/Rapptz/discord.py
- C#: https://github.com/discord-net/Discord.Net
