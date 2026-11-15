# BudBudBot

A starter Discord bot in C# using [Discord.Net](https://docs.discordnet.dev). Made with TaskPilot.

## Run it
1. Install the [.NET 8 SDK](https://dotnet.microsoft.com/download) if you don't have it (Visual Studio installs it too).
2. Put your bot token in `token.txt` next to `Program.cs` (TaskPilot may have done this for you).
3. Double-click `run.bat`, or open `BudBudBot.sln` in Visual Studio and press **F5**.
4. In your Discord server, type `/ping`.

Your bot is online only while the program runs. Close TaskPilot's built-in bot first if it uses the same token,
or the two will fight over the commands.

## Where to go next
- Add a command: copy one of the `SlashCommandBuilder` lines in `Program.cs`, then add a `case` for it below.
- Buttons, menus and pop-up forms: https://docs.discordnet.dev/guides/int_basics/message-components/intro.html
- Reacting to normal messages needs the **Message Content Intent** (Developer Portal → Bot), then
  `GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent` and a `client.MessageReceived` handler.
- Welcome new members: turn on **Server Members Intent**, add `GatewayIntents.GuildMembers`, and handle `client.UserJoined`.
- Keep it online 24/7: run it on a small server or a cheap VPS instead of your PC.

## Keep your token secret
`token.txt` is in `.gitignore` so it won't be uploaded to GitHub. If your token ever leaks,
reset it in the Developer Portal (Bot → Reset Token).