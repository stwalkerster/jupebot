namespace IrcClientJupe.Services;

using Stwalkerster.Bot.CommandLib.Model;
using Stwalkerster.Bot.CommandLib.Services.Interfaces;
using Stwalkerster.IrcClient.Model.Interfaces;

public class AccessControlService : IFlagService
{
    public bool UserHasFlag(IUser user, string flag, string locality)
    {
        return flag == Flag.Standard;
    }

    public IEnumerable<string> GetFlagsForUser(IUser user, string locality)
    {
        return new[] { Flag.Standard };
    }
}