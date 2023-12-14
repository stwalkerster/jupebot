namespace IrcClientJupe;

public class BotConfiguration
{
    public List<string> DefaultChannels { get; set; }
    public string CommandPrefix { get; set; }
    
    public string ClientMode { get; set; }
    public bool DoChghost { get; set; }

    public string OperUser { get; set; }
    public string OperPass { get; set; }
    public IrcConfiguration IrcConfiguration { get; set; }
}