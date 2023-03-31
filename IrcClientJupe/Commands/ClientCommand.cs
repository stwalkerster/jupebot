namespace IrcClientJupe.Commands;

using Castle.Core.Logging;
using Services.Interfaces;
using Stwalkerster.Bot.CommandLib.Attributes;
using Stwalkerster.Bot.CommandLib.Commands.CommandUtilities;
using Stwalkerster.Bot.CommandLib.Commands.CommandUtilities.Response;
using Stwalkerster.Bot.CommandLib.Model;
using Stwalkerster.Bot.CommandLib.Services.Interfaces;
using Stwalkerster.IrcClient.Interfaces;
using Stwalkerster.IrcClient.Model.Interfaces;

[CommandInvocation("client")]
[CommandFlag(Flag.Standard)]
public class ClientCommand : CommandBase
{
    private readonly IJupeManager jupeManager;

    public ClientCommand(
        string commandSource,
        IUser user,
        IList<string> arguments,
        ILogger logger,
        IFlagService flagService,
        IConfigurationProvider configurationProvider,
        IIrcClient client,
        IJupeManager jupeManager) : base(commandSource, user, arguments, logger, flagService, configurationProvider, client)
    {
        this.jupeManager = jupeManager;
    }

    [SubcommandInvocation("new")]
    [RequiredArguments(1)]
    protected IEnumerable<CommandResponse> New()
    {
        this.jupeManager.IntroduceClient(this.Arguments[0], "jupiter/" + this.Arguments[0]);
        this.jupeManager.Inject(this.Arguments[0], "JOIN " + this.CommandSource);

        yield break;
    }
    
    [SubcommandInvocation("inject")]
    [RequiredArguments(2)]
    protected IEnumerable<CommandResponse> Inject()
    {
        this.jupeManager.Inject(this.Arguments[0], string.Join(" ", this.Arguments.Skip(1)));

        yield break;
    }    
    
    [SubcommandInvocation("del")]
    [RequiredArguments(1)]
    protected IEnumerable<CommandResponse> Delete()
    {
        this.jupeManager.ExitClient(this.Arguments[0]);

        yield break;
    }
}