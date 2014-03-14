using System.Collections.Concurrent;
using Server.Creatures;
using Server.Data;
using Server.Interfaces;
using Server.Logic.Exceptions;
using Server.Networking.WebSocket;

namespace Server.Networking
{
    public class Server
    {
        private IServerConnection serverConnection;
        private readonly ICreatureDataProvider creatureDataProvider;

        private readonly ConcurrentDictionary<long, Player> connectedPlayers = new ConcurrentDictionary<long, Player>(2, 150);

        public Server()
        {
            serverConnection = new WebSocketServerConnection();
            creatureDataProvider = new DummyCreatureDataProvider();
        }

        public void Start()
        {
            if (serverConnection == null)
                throw new StartupException(Startup.StartupExceptions.NoServerConnection, "No server connection type has been initialized");

            serverConnection.OnClientConnected += serverConnection_OnClientConnected;
            serverConnection.AcceptConnections(9001);
        }

        private void serverConnection_OnClientConnected(object sender, IClientConnection e)
        {
            var player = new Player(creatureDataProvider, e);

            connectedPlayers.TryAdd(player.UniqueId, player);
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
