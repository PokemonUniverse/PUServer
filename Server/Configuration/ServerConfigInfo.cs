using NoNameLib.Configuration;

namespace Server.Configuration
{
    public class ServerConfigInfo : ConfigurationItemCollection
    {
        private const string SECTION = "ServerConfig";
        public ServerConfigInfo()
        {
            Add(new ConfigurationItem(SECTION, ServerConfigConstants.SERVER_PORT, "Port number on which the server will be running", 4000, typeof(int)));
        }
    }
}
