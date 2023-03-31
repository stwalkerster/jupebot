namespace IrcClientJupe.Services;

using Interfaces;
using Microsoft.Extensions.Logging;
using Stwalkerster.IrcClient;
using Stwalkerster.IrcClient.Interfaces;
using Stwalkerster.IrcClient.Messages;

public class JupeManager : IJupeManager
{
    private readonly BotConfiguration botConfiguration;
    private readonly ILoggerFactory loggerFactory;
    private readonly ISupportHelper supportHelper;
    private readonly IIrcClient baseClient;

    private List<IrcClient> clients = new();

    public JupeManager(BotConfiguration botConfiguration, ILoggerFactory loggerFactory, ISupportHelper supportHelper, IIrcClient baseClient)
    {
        this.botConfiguration = botConfiguration;
        this.loggerFactory = loggerFactory;
        this.supportHelper = supportHelper;
        this.baseClient = baseClient;
    }
    
    public void IntroduceClient(string nickname, string? host)
    {
        var newConfiguration = this.botConfiguration.IrcConfiguration.Clone();
        newConfiguration.AuthToServices = false;
        newConfiguration.ClientName = Guid.NewGuid().ToString();
        newConfiguration.RealName = "JUPITER " + newConfiguration.ClientName;
        newConfiguration.Nickname = nickname;

        var client = new IrcClient(this.loggerFactory, newConfiguration.ToConfiguration(), this.supportHelper);
        client.WaitOnRegistration();
        client.Mode(client.Nickname, "+D");

        if (host != null)
        {
            this.baseClient.Send(new Message("CHGHOST", new[] { client.Nickname, host }));
        }
        
        this.clients.Add(client);
    }

    public void ExitClient(string nickname)
    {
        var firstOrDefault = this.clients.FirstOrDefault(x => x.Nickname == nickname);

        if (firstOrDefault != null)
        {
            firstOrDefault.Inject("QUIT");
            this.clients.Remove(firstOrDefault);
        }
    }

    public void Inject(string nickname, string data)
    {
        var firstOrDefault = this.clients.FirstOrDefault(x => x.Nickname == nickname);

        if (firstOrDefault != null)
        {
            firstOrDefault.Inject(data);
        }
    }
}