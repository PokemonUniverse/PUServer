using NoNameLib.Configuration;
using NoNameLib.Logging;
using Server.Configuration;
using Server.Creatures;
using Server.Data;
using Server.Interfaces;
using Server.Logic.Exceptions;
using Server.Logic.Managers;
using Server.Networking.WebSocket;

namespace Server.Networking
{
    public class Server
    {
        private IServerConnection serverConnection;
        private readonly ICreatureDataProvider creatureDataProvider;

        public Server()
        {
            serverConnection = new WebSocketServerConnection();
            creatureDataProvider = new DummyCreatureDataProvider();
        }

        public void Start()
        {
            if (serverConnection == null)
                throw new StartupException(Startup.StartupExceptions.NoServerConnection, "No server connection type has been initialized");

            var port = ConfigurationManager.GetInt(ServerConfigConstants.SERVER_PORT);
            if (port <= 0 || port >= 65535)
                throw new StartupException(Startup.StartupExceptions.InvalidPortNumber, "Server port with number '{0}' is invalid", port);

            serverConnection.OnClientConnected += serverConnection_OnClientConnected;
            serverConnection.AcceptConnections(port);

            Logger.Info("Server", "Start", "Server accepting connections on port {0}", port);
        }

        private void serverConnection_OnClientConnected(object sender, IClientConnection e)
        {
            // Create new player object
            var player = new Player(creatureDataProvider, e);

            GlobalManager.GetManager<CreatureManager>().AddPlayer(player);
        }

        public void Stop()
        {
            if (serverConnection != null)
            {
                serverConnection.StopListening();
                serverConnection = null;
            }
        }
    }
}
