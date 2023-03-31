namespace IrcClientJupe.Services;

using Stwalkerster.Bot.CommandLib.Services.Interfaces;

public class ConfigProvider : IConfigurationProvider
{
    private readonly BotConfiguration config;

    public ConfigProvider(BotConfiguration config)
    {
        this.config = config;
    }

    public string CommandPrefix => this.config.CommandPrefix;
    public string DebugChannel => this.config.DefaultChannels.First();
    public bool AllowQuotedStrings => true;
}