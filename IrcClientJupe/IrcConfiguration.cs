namespace IrcClientJupe
{
    using Stwalkerster.IrcClient.Interfaces;

    public class IrcConfiguration
    {
        public bool AuthToServices { get; set; }
        public string Hostname { get; set;}
        public string Nickname { get;  set;}
        public int Port { get;  set;}
        public string RealName { get; set; }
        public string Username { get;  set;}
        public string ServerPassword { get; set; }
        public bool Ssl { get;  set;}
        public string ClientName { get;  set;}
        public bool? RestartOnHeavyLag { get;  set;}
        public bool? ReclaimNickFromServices { get; set; }
        public string ServicesUsername { get;  set;}
        public string ServicesPassword { get;  set;}
        public string ServicesCertificate { get; set; }
        public int? PingInterval { get;  set;}
        public int? MissedPingLimit { get;  set;}
        
        public IIrcConfiguration ToConfiguration()
        {
            return new Stwalkerster.IrcClient.IrcConfiguration(
                this.Hostname,
                this.Port,
                this.AuthToServices,
                this.Nickname,
                this.Username,
                this.RealName,
                this.Ssl,
                this.ClientName,
                this.ServerPassword,
                this.ServicesUsername,
                this.ServicesPassword,
                this.ServicesCertificate,
                this.RestartOnHeavyLag.GetValueOrDefault(true),
                this.ReclaimNickFromServices.GetValueOrDefault(true),
                this.PingInterval.GetValueOrDefault(15),
                this.MissedPingLimit.GetValueOrDefault(3));
        }

        public IrcConfiguration Clone()
        {
            return new IrcConfiguration
            {
                Hostname = this.Hostname,
                Port = this.Port,
                AuthToServices = this.AuthToServices,
                Nickname = this.Nickname,
                Username = this.Username,
                RealName = this.RealName,
                Ssl = this.Ssl,
                ClientName = this.ClientName,
                ServerPassword = this.ServerPassword,
                ServicesUsername = this.ServicesUsername,
                ServicesPassword = this.ServicesPassword,
                ServicesCertificate = this.ServicesCertificate,
                RestartOnHeavyLag = this.RestartOnHeavyLag,
                ReclaimNickFromServices = this.ReclaimNickFromServices,
                PingInterval = this.PingInterval,
                MissedPingLimit = this.MissedPingLimit
            };
        }
    }
}