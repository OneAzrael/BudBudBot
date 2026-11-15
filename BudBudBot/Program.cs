// ─────────────────────────────────────────────────────────────────────────────
//  Your Discord bot. Run it with F5 in Visual Studio, or double-click run.bat.
//  While this program runs, your bot is online. Close it and the bot goes offline.
// ─────────────────────────────────────────────────────────────────────────────
using Discord;
using Discord.WebSocket;

// 1. The token is the bot's password. It's read from token.txt (next to this file)
//    or from the DISCORD_TOKEN environment variable. Never paste it into your code or share it.
string? token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
string tokenFile = Path.Combine(AppContext.BaseDirectory, "token.txt");
if (string.IsNullOrWhiteSpace(token) && File.Exists(tokenFile)) token = File.ReadAllText(tokenFile).Trim();
if (string.IsNullOrWhiteSpace(token))
{
    Console.WriteLine("No token found. Put your bot token in token.txt next to Program.cs, then run again.");
    return;
}

// 2. "Intents" say which events the bot wants to hear about. Guilds is enough for slash commands.
//    (Reading normal chat messages needs the Message Content intent, switched on in the Developer Portal.)
var client = new DiscordSocketClient(new DiscordSocketConfig { GatewayIntents = GatewayIntents.Guilds });

client.Log += message =>
{
    Console.WriteLine(message.ToString());
    return Task.CompletedTask;
};

// 3. When the bot is connected, tell Discord which slash commands it has.
//    Adding them per server makes them show up instantly.
client.Ready += async () =>
{
    ApplicationCommandProperties[] commands =
    {
        new SlashCommandBuilder().WithName("ping").WithDescription("Check that the bot is alive").Build(),
        new SlashCommandBuilder().WithName("hello").WithDescription("Say hello to the bot").Build(),
        new SlashCommandBuilder().WithName("roll").WithDescription("Roll a dice")
            .AddOption("sides", ApplicationCommandOptionType.Integer, "How many sides (default 6)", isRequired: false)
            .Build(),
        // ✏️ Add your own here, e.g.:
        // new SlashCommandBuilder().WithName("wishlist").WithDescription("Where to wishlist the game").Build(),
    };
    foreach (var guild in client.Guilds)
        await guild.BulkOverwriteApplicationCommandAsync(commands);
    Console.WriteLine($"Online as {client.CurrentUser.Username} in {client.Guilds.Count} server(s). Press Ctrl+C to stop.");
};

// 4. Someone used a slash command: answer it.
client.SlashCommandExecuted += async command =>
{
    switch (command.Data.Name)
    {
        case "ping":
            await command.RespondAsync($"Pong! 🏓 ({client.Latency} ms)");
            break;

        case "hello":
            await command.RespondAsync($"Hi {command.User.Mention}! 👋");
            break;

        case "roll":
            long sides = command.Data.Options.FirstOrDefault(o => o.Name == "sides")?.Value is long s && s > 1 ? s : 6;
            long result = Random.Shared.NextInt64(1, sides + 1);
            await command.RespondAsync($"🎲 You rolled **{result}** (1–{sides})");
            break;

        // ✏️ And answer your own commands here:
        // case "wishlist":
        //     await command.RespondAsync("Wishlist us on Steam: https://store.steampowered.com/app/...");
        //     break;
    }
};

// 5. Log in and stay online until the program is closed.
await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();
await Task.Delay(Timeout.Infinite);