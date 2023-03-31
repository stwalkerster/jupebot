namespace IrcClientJupe
{
    using System.Reflection;
    using Castle.Facilities.Logging;
    using Castle.Facilities.Startable;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.Services.Logging.Log4netIntegration;
    using Castle.Windsor;
    using Microsoft.Extensions.Logging;
    using Stwalkerster.Bot.CommandLib.Commands.CommandUtilities;
    using Stwalkerster.Bot.CommandLib.Commands.Interfaces;
    using Stwalkerster.Bot.CommandLib.Services;
    using Stwalkerster.Bot.CommandLib.Services.Interfaces;
    using Stwalkerster.Bot.CommandLib.TypedFactories;
    using Stwalkerster.IrcClient;
    using Stwalkerster.IrcClient.Interfaces;
    using Stwalkerster.IrcClient.Messages;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    public class Program : IApplication
    {
        public static void Main(string[] args)
        {
            string configurationFile = "configuration.yml";

            if (args.Length >= 1)
            {
                configurationFile = args[0];
            }

            if (!File.Exists(configurationFile))
            {
                var fullPath = Path.GetFullPath(configurationFile);

                Console.WriteLine("Configuration file at {0} does not exist!", fullPath);
                return;
            }

            var botConfig = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build()
                .Deserialize<BotConfiguration>(File.ReadAllText(configurationFile));
            
            var container = new WindsorContainer();
            container.Register(
                Component.For<BotConfiguration>().Instance(botConfig),
                Component.For<IIrcConfiguration>().Instance(botConfig.IrcConfiguration.ToConfiguration())
            );
            
            var loggerFactory = new LoggerFactory().AddLog4Net("log4net.xml");
            
            // Facilities
            container.AddFacility<LoggingFacility>(f => f.LogUsing<Log4netFactory>().WithConfig("log4net.xml"));
            container.AddFacility<StartableFacility>(f => f.DeferredStart());
            container.AddFacility<TypedFactoryFacility>();
            
            container.Register(
                Classes.FromAssemblyContaining<CommandBase>().BasedOn<ICommand>().LifestyleTransient(),
                Component.For<ICommandTypedFactory>().AsFactory(),
                Classes.FromAssemblyContaining<CommandParser>()
                    .InSameNamespaceAs<CommandParser>()
                    .WithServiceAllInterfaces(),

                Component.For<ILoggerFactory>().Instance(loggerFactory),
                Component.For<ILogger<SupportHelper>>().UsingFactoryMethod(loggerFactory.CreateLogger<SupportHelper>),
                Component.For<ILogger<CommandParser>>().UsingFactoryMethod(loggerFactory.CreateLogger<CommandParser>),
                Classes.FromAssembly(Assembly.GetCallingAssembly()).InNamespace("IrcClientJupe.Services").WithServiceAllInterfaces(),
                Classes.FromAssembly(Assembly.GetCallingAssembly()).BasedOn<ICommand>().LifestyleTransient(),
                Component.For<ISupportHelper>().ImplementedBy<SupportHelper>(),
                Component.For<IIrcClient>().ImplementedBy<IrcClient>(),
                Component.For<IApplication>().ImplementedBy<Program>()
            );

            container.Resolve<IApplication>();
        }

        public Program(IIrcClient client, ICommandHandler commandHandler, BotConfiguration config)
        {
            client.ReceivedMessage += commandHandler.OnMessageReceived;
            
            client.WaitOnRegistration();
            client.Send(new Message("OPER", new[] { config.OperUser, config.OperPass }));
            
            config.DefaultChannels.ForEach(client.JoinChannel);
        }
        
        public void Stop()
        {
        }

        public void Run()
        {
        }
    }
}