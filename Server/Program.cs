using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.ServiceProcess;
using NoNameLib;
using NoNameLib.Configuration;
using NoNameLib.Logging;
using NoNameLib.Verification;
using Server.Configuration;

namespace Server
{
    class Program
    {
        private static string RootDirectory { get; set; }

        static void Main()
        {
            RootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (RootDirectory == null)
                throw new Exception("Unable to find Assembly root path. The fuck happened here?");

            var consoleMode = Environment.CommandLine.Contains("-console");

            Global.ApplicationInfo = new ApplicationInfo();
            Global.ApplicationInfo.ApplicationName = "Server";
            Global.ApplicationInfo.ApplicationVersion = "0.1";
            Global.ApplicationInfo.BasePath = RootDirectory;

            // Initialize validators
            var verifiers = new VerifierCollection();
            verifiers.Add(new WritePermissionVerifier(Path.Combine(RootDirectory, "cfg")));
            verifiers.Add(new WritePermissionVerifier(Path.Combine(RootDirectory, "logs")));
            if (!verifiers.Verify())
            {
                throw new VerificationException(verifiers.ErrorMessage);
            }

            // Set the configuration provider
            var provider = new XmlConfigurationProvider();
            provider.ManualConfigFilePath = Path.Combine(RootDirectory, "cfg", "Server.config");
            Global.ConfigurationProvider = provider;
            Global.ConfigurationInfo.Add(new ServerConfigInfo());

            // Setup logging
            if (consoleMode)
                Global.LoggingProvider = new ConsoleLoggingProvider();
            else
                Global.LoggingProvider = new AsyncLoggingProvider(Path.Combine(RootDirectory, "logs"), "server", 7);

            if (consoleMode)
            {
                var application = new Startup();
                application.StartServer();

                Console.WriteLine(@"The server started successfully, press key 'q' to stop it!");

                while (Console.ReadKey().KeyChar != 'q')
                {
                    //Console.WriteLine("");
                }

                /*Console.WriteLine(@"-----------------------------------------");
                Console.WriteLine(@"End of the line. Press enter to exit.");
                Console.ReadLine();*/
            }
            else
            {
                ServiceBase.Run(new Startup());
            }
        }
    }
}
