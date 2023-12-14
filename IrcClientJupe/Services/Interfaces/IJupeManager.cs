namespace IrcClientJupe.Services.Interfaces;

public interface IJupeManager
{
    bool IntroduceClient(string nickname, string host);
    void ExitClient(string nickname);
    void Inject(string nickname, string data);
}