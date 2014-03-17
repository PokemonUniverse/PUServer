using System;
using NoNameLib.Net.Packet;
using Server.Interfaces;
using Server.Networking.Messages;
using SuperWebSocket;

namespace Server.Networking.WebSocket
{
    public class WebSocketServerConnection : WebSocketServer<ClientSession>, IServerConnection
    {
        #region IServerConnection

        public event EventHandler<IClientConnection> OnClientConnected;

        public bool AcceptConnections(int port)
        {
            if (!Setup(port))
            {
                NoNameLib.Logging.Logger.Error("WebSocketServerConnection", "AcceptConnections", "Failed to setup websocket");
                return false;
            }
            
            // Hookup events
            NewDataReceived += WebSocketServerConnection_NewDataReceived;

            // Start listening
            return Start();
        }

        void WebSocketServerConnection_NewDataReceived(ClientSession session, byte[] data)
        {
            var message = MessageFactory.CreateMessage(new Packet(data));
            if (message != null)
            {
                session.MessageReceived(message);
            }
        }

        public void StopListening()
        {
            this.Stop();
        }

        #endregion

        protected override void OnNewSessionConnected(ClientSession session)
        {
            if (OnClientConnected != null)
                OnClientConnected(this, session.Client);
        }
    }

}
