using System;

namespace Server.Interfaces
{
    interface IServerConnection
    {
        event EventHandler<IClientConnection> OnClientConnected;

        void AcceptConnections(int port);

        void StopListening();
    }
}
