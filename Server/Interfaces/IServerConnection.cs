using System;

namespace Server.Interfaces
{
    interface IServerConnection
    {
        event EventHandler<IClientConnection> OnClientConnected;

        bool AcceptConnections(int port);

        void StopListening();
    }
}
